using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AssemblyCSharp;

public class PowerupController : MonoBehaviour
{
	protected NetworkScript _networking;
	protected GameObject _localPlayer;

	private float _timeSinceLastSpawn;
	private const float _spawnCD = 1f;

	void Start ()
	{
		_timeSinceLastSpawn = 0f;
		_networking = GameObject.Find ("NetworkManager").GetComponent<NetworkScript> ();
		_localPlayer = _networking.Player;
	}

	void Update ()
	{
		_timeSinceLastSpawn += Time.deltaTime;
		if (_timeSinceLastSpawn >= _spawnCD)
		{
			int id = spawnRandomPowerup ();
			_networking.HandlePowerupSpawn (_localPlayer.transform.position, id);
		}
	}

	private int spawnRandomPowerup ()
	{
		Debug.LogWarning ("Spawning a powerup");
		// This is pretty bad and should be changed.
		// Remember to change the value when adding new powerups. 
		int id = Random.RandomRange (0, 2);
		NetworkEventHandlers.SendEvent (new SpawnPowerupEvent (_localPlayer.transform.position, id));
		return id;
	}
}
