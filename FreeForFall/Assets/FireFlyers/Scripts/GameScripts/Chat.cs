using UnityEngine;
using AssemblyCSharp;
using UnityEngine.UI;

public class Chat : MonoBehaviour
{
    int count;
    private string _username;
    private GameEngine _engine;
    private float _timeSinceLastShown;
    private Text _chatText;
    private Image _chatPanel;

    void Start()
    {
        _timeSinceLastShown = 0f;
        _engine = GameObject.Find("NetworkManager").GetComponent<Networking>().Engine;
        var p = _engine.Player;
        _username = p.GetComponent<PhotonView>().owner.NickName;
        _chatText = p.transform.Find("Canvas").Find("ChatText").GetComponent<Text>();
        _chatPanel = p.transform.Find("Canvas").Find("ChatPanel").GetComponent<Image>();
    }

    void Update()
    {
        _timeSinceLastShown += Time.deltaTime;
        if (_timeSinceLastShown > 3f)
            HideChat();
        if (Input.GetKeyDown(KeyCode.C))
        {
            NetworkEventHandlers.Broadcast(Constants.EVENT_IDS.CHAT_MESSAGE,
                new object[]
                {
                    (object)_username,
                    (object)"Sent " + count++.ToString()
                }
            );
            _engine.ReceiveChatMessage(_username, "Sent " + count.ToString());
        }
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
