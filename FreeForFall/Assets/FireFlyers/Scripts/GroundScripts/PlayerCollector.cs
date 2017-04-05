using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AssemblyCSharp;

public class PlayerCollector : MonoBehaviour
{
	void OnCollisionEnter (Collision col)
	{
		if (col.gameObject.tag != "Player")
		{
			Debug.Log ("Collided with something that wasn't a player");
			return;
		}
		Debug.Log ("Collided with a player");
		NetworkEventHandlers.SendEvent (new PlayerLostEvent ());
		GameObject.Find ("NetworkManager").GetComponent<NetworkScript> ().handlePlayerLost ();
	}
}
