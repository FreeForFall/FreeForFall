using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedBoost : MonoBehaviour
{
	public GameObject LocalPlayer;

	void OnTriggerEnter (Collider c)
	{
		Debug.LogWarning ("Something collided with the powerup");
		if (c.gameObject.tag == "Player")
		{
			if (c.gameObject == LocalPlayer)
			{
				Debug.LogWarning ("Doing SpeedBoost on " + c.gameObject.name);
				doPowerup (c.gameObject);
			}
			else
			{
				Debug.LogWarning ("Someone else collided with the powerup, removing it");
			}
			Destroy (gameObject);
		}
	}

	private void doPowerup (GameObject c)
	{
		Debug.LogWarning ("Giving the player a speed boost");
		c.GetComponent<PlayerController> ().SpeedBoost (20);
	}
}
