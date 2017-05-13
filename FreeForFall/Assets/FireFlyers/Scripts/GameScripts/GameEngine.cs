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
	private Constants.MAPS_IDS _mapID;
	private PhotonView _localPhotonView;

	private GameObject _localPlayer;
	private GameObject _camera;
	private ShooterB _shooterB;

	private Vector3 _cameraStartPosition;

	private Camera _flyingCamera;

	private GameObject _map;

	private Networking _network;

	/// <summary>
	/// Initializes a new instance of the <see cref="GameEngine"/> class.
	/// </summary>
	/// <param name="map">The map.</param>
	public GameEngine (Constants.MAPS_IDS map)
	{
		_loadedCount = 0;
		_lostCount = 0;
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
		NetworkEventHandlers.Broadcast (Constants.EVENT_IDS.LOAD_MAP, _mapID);
		LoadMap (_mapID);
		if (PlayerJoined ())
		{
			_network.RemoveWalls ();
		}
	}

	/// <summary>
	/// To be called when a player loaded the map.
	/// </summary>
	/// <returns><c>true</c>, if the walls should be removed, <c>false</c> otherwise.</returns>
	public bool PlayerJoined ()
	{
		return ++_loadedCount >= _playerCount;
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
	public void LoadMap (Constants.MAPS_IDS id)
	{
		string name = Constants.MAPS_NAMES [(int)id];
		_map = (GameObject)GameObject.Instantiate (Resources.Load (name), Vector3.zero, Quaternion.identity);
		createPlayer ();
		_camera = _localPlayer.transform.Find ("TPScamera/firstCamera").gameObject;
		_flyingCamera.gameObject.SetActive (false);
		GameObject.Find ("WaitForGameStartCanvas").GetComponent<Canvas> ().enabled = false;
		if (!PhotonNetwork.isMasterClient)
			NetworkEventHandlers.Broadcast (Constants.EVENT_IDS.MAP_LOADED);
	}

	/// <summary>
	/// Creates the player.
	/// </summary>
	private void createPlayer ()
	{
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
	}
}
