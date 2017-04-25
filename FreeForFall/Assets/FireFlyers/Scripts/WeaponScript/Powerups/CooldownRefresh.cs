using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CooldownRefresh : MonoBehaviour
{
	public GameObject LocalPlayer;

	void OnTriggerEnter (Collider c)
	{
		Debug.LogWarning ("Something collided with the powerup");
		if (c.gameObject.tag == "Player")
		{
			if (c.gameObject == LocalPlayer)
			{
				Debug.LogWarning ("Doing CooldownRefresh on " + c.gameObject.name);
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
		if (c.name == "bottom")
		{
			c.transform.parent.GetComponentInChildren<ShooterB> ().RefreshCooldowns ();
		}
		else
		{
			c.GetComponentInChildren<ShooterB> ().RefreshCooldowns ();
		}
	}
}
