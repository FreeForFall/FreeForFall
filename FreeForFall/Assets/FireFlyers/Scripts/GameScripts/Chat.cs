using UnityEngine;
using AssemblyCSharp;

public class Chat : MonoBehaviour
{
    int count;
    private string _username;

    void Start()
    {
        _username = GameObject.Find("NetworkManager").GetComponent<Networking>()
           .Player.GetComponent<PhotonView>().owner.NickName;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            NetworkEventHandlers.Broadcast(Constants.EVENT_IDS.CHAT_MESSAGE, 
                new object[]
                {
                    (object)_username,
                    (object)"Sent " + count++.ToString()
                }
            );
        }        
    }
}
