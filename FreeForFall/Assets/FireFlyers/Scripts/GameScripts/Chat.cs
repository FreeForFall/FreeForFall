using UnityEngine;
using AssemblyCSharp;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;

public class Chat : MonoBehaviour
{
    int count;
    private string _username;
    private GameEngine _engine;
    private float _timeSinceLastShown;
    private float _timeSinceLastMessage;
    private Text _chatText;
    private Canvas _canvas;
    private List<string> _chatMessages;
    private bool _inPostRound;

    void Start()
    {
        _inPostRound = false;
        _timeSinceLastShown = 0f;
        _timeSinceLastMessage = 0f;
        _engine = GameObject.Find("NetworkManager").GetComponent<Networking>().Engine;
        var p = _engine.Player;
        _username = p.GetComponent<PhotonView>().owner.NickName;
        _chatText = GameObject.Find("ChatManager/Canvas/ChatText").GetComponent<Text>();
        _canvas = GameObject.Find("ChatManager/Canvas").GetComponent<Canvas>();        
        _chatMessages = new List<string>();

        
    }

    public void ReceiveMessage(string name, string content)
    {
        _chatMessages.Add(name + " - " + content + "\n");
        updateDisplay();
        ShowChat();
    }


    private void updateDisplay()
    {
        if (_chatMessages.Count < 9)
        {
            _chatText.text += _chatMessages.Last();
            return;
        }

        while (_chatMessages.Count > 9)
            _chatMessages.RemoveAt(0);
       
        _chatText.text = "";
        foreach (var msg in _chatMessages)
        {
            _chatText.text += msg;
        }

    }

    void Update()
    {
        /*
         * On windows, the up/down arrows are the 7th axis
         * the left/right arrows are the 6th axis
         * On Linux : 8th and 7th
         * */
        _timeSinceLastShown += Time.deltaTime;
        _timeSinceLastMessage += Time.deltaTime;
        if (_timeSinceLastShown > 3f && !_inPostRound)
        {
            HideChat();
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
      
        if (leftRight == 0f)
        {
            message += "WELL PLAYED";
        }
        else if (upDown == 0f)
        {
            message += "GIT GUD";
        }
        else
            message += "BOTH AYY";

        NetworkEventHandlers.Broadcast(Constants.EVENT_IDS.CHAT_MESSAGE,
            new object[]
            {
                (object)_username,
                (object)message
            }
        );
        ReceiveMessage(_username, message);
        _timeSinceLastMessage = 0f;
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
