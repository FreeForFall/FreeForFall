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
			spawn ();
		}
	}

	private void spawn ()
	{
		Debug.LogWarning ("Spawning a powerup");
		// This is pretty bad and should be changed.
		// Remember to change the value when adding new powerups. 
		switch (Random.RandomRange (0, 2))
		{
			case 0:
				// SpeedBoost
				break;
			case 1:
				// CooldownRefresh
				break;
			case 2:
				// ImpairVision
				break;
		}
	}
}
