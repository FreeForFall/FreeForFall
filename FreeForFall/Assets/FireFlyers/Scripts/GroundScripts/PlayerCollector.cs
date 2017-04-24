using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AssemblyCSharp;

public class PlayerCollector : MonoBehaviour
{
	private GameObject _localPlayer;
	private NetworkScript _networking;

	void Start ()
	{
		_localPlayer = GameObject.Find ("NetworkManager").GetComponent<NetworkScript> ().Player;
		_networking = GameObject.Find ("NetworkManager").GetComponent<NetworkScript> ();
	}

	void OnCollisionEnter (Collision col)
	{
		if (col.gameObject.tag != "Player")
		{
			Debug.Log ("Collided with something that wasn't a player");
			return;
		}
		Debug.LogWarning ("Collided with a player");
		GameObject p = col.gameObject.name != "Player" 
			? col.gameObject.transform.parent.gameObject 
			: col.gameObject;// I don't think this is ever going to happen but we never know
		if (p == _localPlayer)
		{
			Debug.LogWarning ("The local player collided");
			_networking.SwitchToSpecView ();
		}
		if (PhotonNetwork.isMasterClient)
			_networking.handlePlayerLost ();
		else
			NetworkEventHandlers.SendEvent (new PlayerLostEvent ());
		
	}
}
