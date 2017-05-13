using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AssemblyCSharp;

public class PlayerCollector : MonoBehaviour
{
	private Networking _networking;

	void Start ()
	{
		_networking = GameObject.Find ("NetworkManager").GetComponent<Networking> ();
		Debug.Log (_networking.Player);
	}

	void OnCollisionEnter (Collision col)
	{
		if (col.gameObject.tag != "Player")
		{
			return;
		}
		GameObject p = col.gameObject.name != "Player" 
			? col.gameObject.transform.parent.gameObject 
			: col.gameObject;// I don't think this is ever going to happen but we never know
		if (p == _networking.Player)
		{
			_networking.Engine.SwitchToSpecView ();
		}
		if (PhotonNetwork.isMasterClient)
		{
			_networking.Engine.PlayerLost (p);
		}
		// Add server stuff
		Destroy (p);
	}
}
