using UnityEngine;
using AssemblyCSharp;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;
using System;

public class Chat : MonoBehaviour
{
    int count;
    private string _username;
    private GameEngine _engine;
    private float _timeSinceLastShown;
    private float _timeSinceLastMessage;
    private Text _chatText;
    private Canvas _canvas;
    private InputField _input;
    private List<string> _chatMessages;
    private bool _isWriting = false;
    private bool _inMenu = false;

        
    public bool InMenu
    {
        get
        {
            return _inMenu;
        }
        set
        {
            _inMenu = value;
        }
    }

    void Start()
    {
        _timeSinceLastShown = 0f;
        _timeSinceLastMessage = 0f;
        _engine = GameObject.Find("NetworkManager").GetComponent<Networking>().Engine;
        _username = PhotonNetwork.playerName;
        _chatText = GameObject.Find("ChatManager/Canvas/ChatText").GetComponent<Text>();
        _canvas = GameObject.Find("ChatManager/Canvas").GetComponent<Canvas>();        
        _chatMessages = new List<string>();
        _input = _canvas.transform.Find("ChatPanel/InputField").gameObject.GetComponent<InputField>();
        _input.onValueChanged.AddListener(s => _isWriting = true);
        _input.onEndEdit.AddListener(s =>
            {
                SendChatMessage(_username, s);
                _isWriting = false;
            }); 
    }

    public void ReceiveMessage(string name, string content)
    {
        _chatMessages.Add(name + " - " + content + "\n");
        updateDisplay();
        ShowChat();
    }


    private void updateDisplay()
    {
        if (_chatMessages.Count < 10)
        {
            var msg = _chatMessages.Last();
            string[] spl = msg.Split('-');
            _chatText.text += "<color=#ffa500ff>" + spl[0] + "</color>";
            for (int i = 1; i < spl.Length; i++)
                _chatText.text += "-" + spl[i];
            return;
        }

        while (_chatMessages.Count > 10)
            _chatMessages.RemoveAt(0);
       
        _chatText.text = "";
        foreach (var msg in _chatMessages)
        {

            Debug.Log(msg);
            string[] spl = msg.Split('-');
            _chatText.text += "<color=#ffa500ff>" + spl[0] + "</color>";
            for (int i = 1; i < spl.Length; i++)
                _chatText.text += "-" + spl[i];
        }

    }

    void Update()
    {
        /*
         * On windows, the up/down arrows are the 7th axis
         * the left/right arrows are the 6th axis
         * On Linux : 8th and 7th
         * */

        if (Input.GetKey(KeyCode.F1) || Input.GetKey(KeyCode.Joystick1Button7))
        {
            _engine.Network.menu();
        }
        _timeSinceLastShown += Time.deltaTime;
        _timeSinceLastMessage += Time.deltaTime;
        if (!_inMenu && _timeSinceLastShown > 3f && !_isWriting && !Input.GetKey(KeyCode.Tab))
        {
            HideChat();
        }
        if (Input.GetKey(KeyCode.Tab))
        {
            ShowChat(); 
            _input.Select();
        }   

        if (_timeSinceLastMessage < 0.5f)
            return;

        float upDown = 0f;
        float leftRight = 0f;
        string message = "";

       

#if UNITY_STANDALONE_WIN
        leftRight = Input.GetAxis("WindowsLeftRightDPAD");
        upDown = Input.GetAxis("WindowsUpDownDPAD");
#endif
#if UNITY_STANDALONE_LINUX
        leftRight = Input.GetAxis("LinuxLeftRightDPAD");
        upDown = Input.GetAxis("LinuxUpDownDPAD");
#endif

        if (leftRight == 0f && upDown == 0f)
            return;
        else if (leftRight == 1f && upDown == 0f)
            message = "WELL PLAYED!!!";
        else if (leftRight == -1f && upDown == 0f)
            message = "YOU GOT ME !!!";
        else if (leftRight == 0f && upDown == 1f)
            message = "AWESOME !!!";
        else if (leftRight == 0f && upDown == -1f)
            message = "GG !!!";
        else
            return;



        SendChatMessage(_username, message);

        _timeSinceLastMessage = 0f;
    }

    public void SendChatMessage(string username, string message)
    {
        _isWriting = false;
        _input.text = "";
        NetworkEventHandlers.Broadcast(Constants.EVENT_IDS.CHAT_MESSAGE,
            new object[]
            {
                (object)username,
                (object)message
            }
        );
        ReceiveMessage(username, message);
    }
 
    public void ShowChat()
    {
        _timeSinceLastShown = 0f;
        _canvas.enabled = true;
    }

    public void HideChat()
    {
        _canvas.enabled = false;
    }
}
