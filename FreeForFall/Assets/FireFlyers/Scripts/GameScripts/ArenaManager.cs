using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AssemblyCSharp;

public class ArenaManager : MonoBehaviour
{
	void Start ()
	{
		if (!PhotonNetwork.isMasterClient)
		{
			NetworkEventHandlers.Broadcast (Constants.EVENT_IDS.SCENE_LOADED);
			return;
		}
		var e = GameObject.Find ("NetworkManager").GetComponent<Networking> ().Engine;
		NetworkEventHandlers.Broadcast (Constants.EVENT_IDS.LOAD_SCENE, e.MapID);
		if (PhotonNetwork.offlineMode)
		{
			e.ReadyMap ();
			e.Network.RemoveWalls ();
		}
	}
}
