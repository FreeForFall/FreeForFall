using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerupSpawner : MonoBehaviour
{
	public bool Spawned = false;

	void OnTriggerEnter (Collider c)
	{
		if (c.gameObject.tag != "Player")
			return;
		Spawned = Spawned ? false : true;
	}
}
