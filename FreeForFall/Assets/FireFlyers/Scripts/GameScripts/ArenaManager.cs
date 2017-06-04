using UnityEngine;
using AssemblyCSharp;

public class ArenaManager : MonoBehaviour
{
    void Start()
    {
        Debug.Log("ARENA MANAGER START");
        var e = GameObject.Find("NetworkManager").GetComponent<Networking>().Engine;

        e.ReadyMap();
        if (PhotonNetwork.offlineMode)
        {
            e.SpawnPlayers();
            e.Network.RemoveWalls();
            return;
        }
        else if (!PhotonNetwork.isMasterClient)
        {
            NetworkEventHandlers.Broadcast(Constants.EVENT_IDS.SCENE_LOADED);
            return;
        }
        else
            NetworkEventHandlers.Broadcast(Constants.EVENT_IDS.LOAD_SCENE, e.MapID);
    }
}
