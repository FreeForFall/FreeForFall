using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AssemblyCSharp;

public class PlayerCollector : MonoBehaviour
{
    private Networking _networking;

    void Start()
    {
        _networking = GameObject.Find("NetworkManager").GetComponent<Networking>();
    }

    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.tag != "Player")
        {
            return;
        }

        GameObject p = col.transform.root.gameObject;
        if (p.GetComponent<NetworkPlayerController>() != null)
        {
            if (p == _networking.Player)
                _networking.Engine.SwitchToSpecView();
            if (PhotonNetwork.isMasterClient)
                _networking.Engine.PlayerLost(p);
        }
        Destroy(p);
    }
}
