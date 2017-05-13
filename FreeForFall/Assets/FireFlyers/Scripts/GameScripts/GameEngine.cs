using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Threading;
using AssemblyCSharp;

public class GameEngine
{
	private int _loadedCount;
	private int _lostCount;
	private int _playerCount;
	private int _playerSpawned;
	private Constants.MAPS_IDS _mapID;
	private PhotonView _localPhotonView;

	private GameObject _localPlayer;
	private GameObject _camera;
	private ShooterB _shooterB;

	private Vector3 _cameraStartPosition;

	private Camera _flyingCamera;

	private GameObject _map;

	private Networking _network;

	public Constants.MAPS_IDS MapID
	{
		get
		{
			return _mapID;
		}
	}

	public GameObject Player
	{
		get
		{
			return _localPlayer;
		}
	}

	public Networking Network
	{
		get
		{
			return _network;
		}
	}

	/// <summary>
	/// Initializes a new instance of the <see cref="GameEngine"/> class.
	/// </summary>
	/// <param name="map">The map.</param>
	public GameEngine (Constants.MAPS_IDS map)
	{
		_loadedCount = 0;
		_lostCount = 0;
		_playerSpawned = 0;
		_mapID = map;
		_network = GameObject.Find ("NetworkManager").GetComponent<Networking> ();
		_flyingCamera = GameObject.Find ("FlyingCamera").GetComponent<Camera> ();
	}

	/// <summary>
	/// Starts the game.
	/// </summary>
	public void StartGame ()
	{
		
		if (!PhotonNetwork.isMasterClient)
			Debug.LogError ("NON MASTER CLIENT TRIED TO START A GAME WTF");
		_playerCount = PhotonNetwork.room.PlayerCount;
		_loadedCount++;
		LoadScene (_mapID);
	}

	/// <summary>
	/// To be called when a player loaded the map.
	/// </summary>
	/// <returns><c>true</c>, if the walls should be removed, <c>false</c> otherwise.</returns>
	public bool PlayerLoadedScene ()
	{
		Debug.Log ("A player loaded the scene");
		return PhotonNetwork.isMasterClient && ++_loadedCount >= _playerCount;
	}

	/// <summary>
	/// To be called when a player is done executing ReadyMap
	/// </summary>
	public void PlayerSpawned ()
	{
		Debug.Log ("A player spawned");
		if (++_playerSpawned < _playerCount)
			return;
		_network.RemoveWalls ();
	}

	/// <summary>
	/// Spawns the players.
	/// </summary>
	public void SpawnPlayers ()
	{
		if (!PhotonNetwork.isMasterClient)
			return;
		Debug.Log ("Spawning all the players");
		ReadyMap ();
		NetworkEventHandlers.Broadcast (Constants.EVENT_IDS.SPAWN_PLAYER);
	}

	/// <summary>
	/// Removes the walls.
	/// </summary>
	public void RemoveWalls ()
	{
		GameObject.Destroy (_map.transform.Find ("BoxPrefab").gameObject);
	}

	/// <summary>
	/// Loads the map and instantiates the player.
	/// </summary>
	public void LoadScene (Constants.MAPS_IDS id)
	{
		_mapID = id;
		string name = Constants.MAP_SCENES_NAMES [(int)id];
		SceneManager.LoadScene (name);
		//_map = (GameObject)GameObject.Instantiate (Resources.Load (name), Vector3.zero, Quaternion.identity);
	}

	/// <summary>
	/// Sets the map up
	/// </summary>
	public void ReadyMap ()
	{
		Debug.Log ("Spawning");
		_map = GameObject.Find (Constants.MAPS_NAMES [(int)_mapID]);
		createPlayer ();
		_flyingCamera = GameObject.Find ("FlyingCamera").GetComponent<Camera> ();
		_camera = _localPlayer.transform.Find ("TPScamera/firstCamera").gameObject;
		_flyingCamera.gameObject.SetActive (false);
		_cameraStartPosition = _flyingCamera.transform.position;
	}

	/// <summary>
	/// Creates the player.
	/// </summary>
	private void createPlayer ()
	{
		_playerSpawned++;
		Vector3 spawnPosition = _map.transform.Find ("BoxPrefab").transform.position + Vector3.up * 15;
		spawnPosition.x = Random.Range (-9f, 9f);
		spawnPosition.z = Random.Range (-9f, 9f);
		_localPlayer = PhotonNetwork.Instantiate ("Player", spawnPosition, Quaternion.identity, 0);
		_localPlayer.GetComponent<CrosshairUI> ().enabled = true;
		_localPhotonView = _localPlayer.GetComponent<PhotonView> ();
		//_localPlayer.GetComponent<UI> ().enabled = true;
		//_localPlayer.transform.Find ("Canvas").gameObject.SetActive (true);
		_localPlayer.GetComponentInChildren<PlayerController> ().enabled = true;
		//_localPlayer.GetComponent<ShooterB> ().enabled = true;
		//_shooterB = _localPlayer.GetComponent<ShooterB> ();
		_localPlayer.GetComponentInChildren<LookTowardCamera> ().enabled = true;
		_localPlayer.GetComponentInChildren<CameraControl> ().enabled = true;
		if (!PhotonNetwork.isMasterClient)
			NetworkEventHandlers.Broadcast (Constants.EVENT_IDS.PLAYER_SPAWNED);
	}

	/// <summary>
	/// Switches to spectator view.
	/// </summary>
	public void SwitchToSpecView ()
	{
		switchCamera ();
		_network.MoveObject (_flyingCamera.gameObject, _camera.transform.position, _cameraStartPosition, Constants.SPEC_CAMERA_TRAVEL_TIME);
	}

	/// <summary>
	/// Swicthes from the flying camera to the player camera and vice versa.
	/// </summary>
	private void switchCamera ()
	{
		Debug.LogWarning ("SWITCHING CAMERAS");
		if (_flyingCamera.gameObject.GetActive ())
		{
			_flyingCamera.gameObject.GetComponent<Camera> ().enabled = false;
			_flyingCamera.GetComponent<AudioListener> ().enabled = false;
			_flyingCamera.gameObject.SetActive (false);
			_camera.SetActive (true);
		}
		else
		{
			_flyingCamera.gameObject.GetComponent<Camera> ().enabled = true;
			_flyingCamera.gameObject.SetActive (true);
			_flyingCamera.GetComponent<AudioListener> ().enabled = true;
			_camera.SetActive (false); 
		}
	}
}
