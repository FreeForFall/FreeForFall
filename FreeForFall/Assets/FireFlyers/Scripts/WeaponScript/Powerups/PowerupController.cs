using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AssemblyCSharp;

public class PowerupController : MonoBehaviour
{
	private NetworkScript _networking;
	private GameObject _localPlayer;
	private Transform _playerTrs;

	private float _timeSinceLastSpawn;
	private const float _spawnCD = 5f;

	void Start ()
	{
		_timeSinceLastSpawn = 0f;
		_networking = GameObject.Find ("NetworkManager").GetComponent<NetworkScript> ();
		_localPlayer = _networking.Player;
		_playerTrs = _localPlayer.transform.Find ("bottom").transform;
	}

	void Update ()
	{
		_timeSinceLastSpawn += Time.deltaTime;
		if (_timeSinceLastSpawn >= _spawnCD)
		{
			int id = spawnRandomPowerup ();
			_networking.HandlePowerupSpawn (_playerTrs.position, id);
			_timeSinceLastSpawn = 0f;
		}
	}

	private int spawnRandomPowerup ()
	{
		Debug.LogWarning ("Spawning a powerup");
		// This is pretty bad and should be changed.
		// Remember to change the value when adding new powerups. 
		int id = Random.RandomRange (0, 3);
		NetworkEventHandlers.SendEvent (new SpawnPowerupEvent (_playerTrs.position, id));
		return id;
	}
}
