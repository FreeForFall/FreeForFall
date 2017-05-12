using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AssemblyCSharp;

public class PowerupController : MonoBehaviour
{
	private NetworkScript _networking;
	private GameObject[] _spawners;

	private float _timeSinceLastSpawn;

	void Start ()
	{
		_timeSinceLastSpawn = 0f;
		_networking = GameObject.Find ("NetworkManager").GetComponent<NetworkScript> ();
		_spawners = GameObject.FindGameObjectsWithTag ("PowerupSpawner");
	}

	void Update ()
	{
		_timeSinceLastSpawn += Time.deltaTime;
		if (_timeSinceLastSpawn >= Constants.POWERUP_SPAWN_CD)
		{
			foreach (var g in _spawners)
			{
				var p = g.GetComponent<PowerupSpawner> ();
				if (p.Spawned)
					continue;
				int id = spawnRandomPowerup (g.transform.position);
				_networking.HandlePowerupSpawn (g.transform.position, id);
				p.Spawned = true;
			}
			_timeSinceLastSpawn = 0f;
		}
	}

	private int spawnRandomPowerup (Vector3 position)
	{
		//Debug.LogWarning ("Spawning a powerup");
		// This is pretty bad and should be changed.
		// Remember to change the value when adding new powerups. 
		int id = Random.Range (0, 3);
		NetworkEventHandlers.SendEvent (new SpawnPowerupEvent (position, id));
		return id;
	}
}
