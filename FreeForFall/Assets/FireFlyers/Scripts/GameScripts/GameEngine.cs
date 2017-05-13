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
	private Constants.MAPS_IDS _mapID;

	private GameObject _localPlayer;
	private GameObject _camera;
	private ShooterB _shooterB;

	private Vector3 _cameraStartPosition;

	private Camera _flyingCamera;

	private GameObject _map;

	public GameEngine (Constants.MAPS_IDS map)
	{
		_loadedCount = 0;
		_lostCount = 0;
		_mapID = map;
		_flyingCamera = GameObject.Find ("FlyingCamera").GetComponent<Camera> ();
	}

	public void StartGame ()
	{
		if (!PhotonNetwork.isMasterClient)
			Debug.LogError ("NON MASTER CLIENT TRIED TO START A GAME WTF");
		NetworkEventHandlers.Broadcast (Constants.EVENT_IDS.LOAD_MAP);
		LoadMap ();
	}

	/*

	Loading the map has to be done with an event because we don't have a photonview
	at this point

	*/

	public void LoadMap ()
	{
		
		string name = Constants.MAPS_NAMES [(int)_mapID];
		Debug.Log ("Loading map " + name);
		_map = (GameObject)GameObject.Instantiate (Resources.Load (name), Vector3.zero, Quaternion.identity);
		Vector3 spawnPosition = _map.transform.Find ("BoxPrefab").transform.position + Vector3.up * 15;
		spawnPosition.x = Random.Range (-9f, 9f);
		spawnPosition.z = Random.Range (-9f, 9f);
		_localPlayer = PhotonNetwork.Instantiate ("Player", spawnPosition, Quaternion.identity, 0);
		//_localPlayer.transform.Find ("bottom").GetComponentInChildren<Canvas> ().GetComponentInChildren<Text> ().text = _nickname;
		_localPlayer.GetComponent<CrosshairUI> ().enabled = true;
		_localPlayer.GetComponent<UI> ().enabled = true;
		_localPlayer.transform.Find ("Canvas").gameObject.SetActive (true);
		_localPlayer.GetComponentInChildren<PlayerController> ().enabled = true;
		_localPlayer.GetComponent<ShooterB> ().enabled = true;
		_shooterB = _localPlayer.GetComponent<ShooterB> ();
		_localPlayer.GetComponentInChildren<LookTowardCamera> ().enabled = true;
		_localPlayer.GetComponentInChildren<CameraControl> ().enabled = true;
		_camera = _localPlayer.transform.Find ("TPScamera/firstCamera").gameObject;
		_flyingCamera.gameObject.SetActive (false);
		GameObject.Find ("WaitForGameStartCanvas").GetComponent<Canvas> ().enabled = false;
	}
}
