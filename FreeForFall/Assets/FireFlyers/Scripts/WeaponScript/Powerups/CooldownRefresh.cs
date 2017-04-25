using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CooldownRefresh : MonoBehaviour
{
	void OnTriggerEnter (Collider c)
	{
		Debug.LogWarning ("Something collided with the powerup");
		if (c.gameObject.tag == "Player")
		{
			Debug.LogWarning ("Doing CooldownRefresh on " + c.gameObject.name);
			doPowerup (c.gameObject);
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
