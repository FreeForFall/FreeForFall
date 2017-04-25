using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImpairVision : MonoBehaviour
{
	void OnTriggerEnter (Collider c)
	{
		Debug.LogWarning ("Something collided with the powerup");
		if (c.gameObject.tag == "Player")
		{
			Debug.LogWarning ("Doing SpeedBoost on " + c.gameObject.name);
			doPowerup (c.gameObject);
			Destroy (gameObject);
		}
	}

	private void doPowerup (GameObject c)
	{
		Debug.LogWarning ("NOT IMPLEMENTED : Impairing the vision");
	}
}
