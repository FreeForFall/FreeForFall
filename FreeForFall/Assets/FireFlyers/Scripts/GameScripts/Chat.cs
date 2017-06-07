using UnityEngine;
using AssemblyCSharp;
using UnityEngine.UI;

public class Chat : MonoBehaviour
{
    int count;
    private string _username;
    private GameEngine _engine;
    private float _timeSinceLastShown;
    private float _timeSinceLastMessage;
    private Text _chatText;
    private Image _chatPanel;

    void Start()
    {
        _timeSinceLastShown = 0f;
        _timeSinceLastMessage = 0f;
        _engine = GameObject.Find("NetworkManager").GetComponent<Networking>().Engine;
        var p = _engine.Player;
        _username = p.GetComponent<PhotonView>().owner.NickName;
        _chatText = p.transform.Find("Canvas").Find("ChatText").GetComponent<Text>();
        _chatPanel = p.transform.Find("Canvas").Find("ChatPanel").GetComponent<Image>();
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
        if (_timeSinceLastShown > 3f)
            HideChat();

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

        Debug.Log(upDown);
        Debug.Log(leftRight);

        if (leftRight == 0f && upDown == 0f)
            return;
      
        if (leftRight == 0f)
        {
            message += upDown > 0f ? "GIT GUD" : "WELL PLAYED";
        }
        else if (upDown == 0f)
        {
            message += leftRight > 0f ? "DUDIN KING" : "NICE MEME";
        }
        else
            return;

        NetworkEventHandlers.Broadcast(Constants.EVENT_IDS.CHAT_MESSAGE,
            new object[]
            {
                (object)_username,
                (object)message
            }
        );
        _engine.ReceiveChatMessage(_username, message);

        _timeSinceLastMessage = 0f;
        /* if (Input.GetKeyDown(KeyCode.C)) */
        /* { */
        /*     NetworkEventHandlers.Broadcast(Constants.EVENT_IDS.CHAT_MESSAGE, */
        /*         new object[] */
        /*         { */
        /*             (object)_username, */
        /*             (object)"Sent " + count++.ToString() */
        /*         } */
        /*     ); */
        /*     _engine.ReceiveChatMessage(_username, "Sent " + count.ToString()); */
        /* } */
    }
 
    public void ShowChat()
    {
        _timeSinceLastShown = 0f;
        _chatPanel.enabled = true;
        _chatText.enabled = true;
    }

    public void HideChat()
    {
        _chatPanel.enabled = false;
        _chatText.enabled = false;
    }
}
