using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CooldownRefresh : MonoBehaviour
{
	public void DoPowerup (GameObject c)
	{
		Debug.LogWarning ("Refreshing the powerups");
		c.GetComponentInChildren<ShooterB> ().RefreshCooldowns ();
	}
}
