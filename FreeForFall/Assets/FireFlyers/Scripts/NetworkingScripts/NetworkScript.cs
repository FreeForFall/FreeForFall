using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Threading;


namespace AssemblyCSharp
{
	public class NetworkScript : MonoBehaviour
	{
		/*

		TODO : Rewrite this, at the moment I need have 2 find*Objects functions which is not pretty.
		Instantiating the two menus from the beginning and only rendering one of them could be cool.

		TODO : Use RPCs FFS
		*/

		/*


		The event handling should be reworked too. Normal clients should ignore most events.
		Maybe reserve < 20 eventCodes for master client events.

		*/
		private GameObject _matchMakingCanvas;
		private GameObject _waitForGameStartCanvas;
		private GameObject _map;
		private Button _roomCreationButton;
		private Button _refreshButton;
		private InputField _nameInput;
		private InputField _nicknameInput;
		private Text _roomList;
		private Text _playerCountText;
		private Button _startGameButton;

		private GameObject _player;
		private GameObject _camera;

		private Vector3 _cameraStartPosition;

		private string _nickname;

		public GameObject Player
		{
			get
			{
				return _player;
			}
		}

		public Camera FlyingCamera;

		private int _loadedCount;
		private int _lostCount;

		public int getPlayerCount ()
		{
			return PhotonNetwork.room.PlayerCount;
		}

		void Start ()
		{
			_loadedCount = 0;
			_lostCount = 0;
			PhotonNetwork.autoJoinLobby = true;
			PhotonNetwork.OnEventCall += handleNetworkEvents;
			_matchMakingCanvas = (GameObject)Resources.Load ("MatchmakingCanvas");
			_waitForGameStartCanvas = (GameObject)Resources.Load ("WaitForGameStartCanvas");
			_matchMakingCanvas = Instantiate (_matchMakingCanvas);
			init ();
			connectToServer (GameObject.Find ("SettingsManager").GetComponent<Settings> ().OnlineMode);
			_cameraStartPosition = FlyingCamera.transform.position;
		}

		// Need to introduce safe casting of the objects sent over the network, this is really dangerous.
		private void handleNetworkEvents (byte eventCode, object content, int senderId)
		{
			/*

			0x0 : LoadMapEvent : c[0] : byte = mapid
			0x1 : MapLoadedEvent
			0x3 : RemoveWallsEvent
			
			0x19 : PlayerLostEvent

			0x30 : VisionImpairedEvent
			0x31 : SpeedBoostEvent // Not implemented, needed for particles
			0x32 : CooldownRefreshEvent // Not implemented, needed for particles

			0x40 : SpawnPowerupEvent : c[0] : Vector3 = position, c[1] : int = eventid

			0x50 : GrapplingHookEvent // Needed for particles
			0x51 : BazookaEvent // Needed for particles and forces

			0x99 : EndGameEvent

			*/
			object[] c = (object[])content;
			switch (eventCode)
			{
				case 0x0:
					int b = (int)c [0];
					handleMapLoadNetworkEvent ((byte)b);
					return;
				case 0x1:
					removeWalls ();
					return;
				case 0x3:
				// remove walls
					Invoke ("destroyBox", 5);
					return;

				case 0x19:
					// Player lost
					handlePlayerLost ();
					return;
				
				case 0x30:
					handleVisionImpaired ();
					return;
				case 0x31:
					handleSpeedBoost ();
					return;
				case 0x32:
					handleCooldownRefresh ();
					return;

				case 0x40:
					HandlePowerupSpawn ((Vector3)c [0], (int)c [1]);
					return;
				
				case 0x50:
					handleGrapplingHook ();
					return;
				case 0x51:
					handleBazooka ();
					return;
				
				
				case 0x99:
					// End of the game
					endGame ();
					return;
				default:
					Debug.LogWarning ("Received unknown event with code " + eventCode);
					return;
			}
		}

		// public because called from PowerupController
		/*

		Need an enum later on
		0 : SpeedBoost
		1 : ImpairVision
		2 : CooldownRefresh
		*/

		public void HandlePowerupSpawn (Vector3 position, int id)
		{
			Debug.LogWarning ("Spawning powerup with id " + id + " at position " + position);
			GameObject p = (GameObject)Instantiate (Resources.Load ("Powerup"), position, Quaternion.identity);
			switch (id)
			{
				case 0:
					var sp = p.AddComponent<SpeedBoost> ();
					sp.LocalPlayer = _player;
					break;
				case 1:
					var im = p.AddComponent<ImpairVision> ();
					im.LocalPlayer = _player;
					break;
				case 2:
					var cr = p.AddComponent<CooldownRefresh> ();
					cr.LocalPlayer = _player;
					break;
				default:
					var s = p.AddComponent<SpeedBoost> ();
					s.LocalPlayer = _player;
					break;
			}
			Debug.LogWarning ("Spawned");
		}

		private void handleGrapplingHook ()
		{
			Debug.LogWarning ("Not implemented");	
		}

		private void handleBazooka ()
		{
			Debug.LogWarning ("Not implemented");
		}

		private void handleSpeedBoost ()
		{
			Debug.LogWarning ("Not implemented");
		}

		private void handleCooldownRefresh ()
		{
			Debug.LogWarning ("Not implemented");
		}

		private void handleVisionImpaired ()
		{
			_camera.GetComponent<CameraFilterPack_FX_Glitch1> ().enabled = true;
			Invoke ("RemoveVisionImpaired", 5);


		}

		private void endGame ()
		{
			/*

			Replace this method by something that shows a prefab
			When it does, remove the Invoke("endGame") from handlePlayerLost, 
			because it won't be needed anymore

			*/
			PhotonNetwork.Disconnect ();
			SceneManager.LoadScene ("Menu");
		}

		private int playerLost ()
		{
			return ++_lostCount;
		}

		// public because has to be called from PlayerCollector.cs
		public void handlePlayerLost ()
		{
			Debug.Log ("A player lost");
			if (!PhotonNetwork.isMasterClient)
				return;
			int a = playerLost ();
			if (GameObject.Find ("SettingsManager").GetComponent<Settings> ().OnlineMode)
			{
				if (a < PhotonNetwork.room.PlayerCount)
				{
					Debug.Log ("a player lost, but he wasn't the last one");
					return;
				}
				Debug.Log ("A player lost and was the last man standing.");
				NetworkEventHandlers.SendEvent (new EndGameEvent ());
				Invoke ("endGame", 6);
			}
			else
			{
				Invoke ("endGame", 6);
			}

		}

		private void removeWalls ()
		{
			if (!PhotonNetwork.isMasterClient)
				return;
			if (handlePlayerLoaded () >= PhotonNetwork.room.PlayerCount)
			{
				Debug.Log ("REMOVING WALLS");
				NetworkEventHandlers.SendEvent (new RemoveWallsEvent ());
				//Invoke ("switchCamera", 3);
				Invoke ("destroyBox", 5);
			}
		}

		public void SwitchToSpecView ()
		{
			switchCamera ();
			StartCoroutine (MoveObject (FlyingCamera.transform, _camera.transform.position, _cameraStartPosition, 5f));
		}

		IEnumerator MoveObject (Transform obj, Vector3 source, Vector3 target, float overTime)
		{
			float startTime = Time.time;
			while (Time.time < startTime + overTime)
			{
				obj.position = Vector3.Lerp (source, target, (Time.time - startTime) / overTime);
				yield return null;
			}
			obj.position = target;
		}


		private void switchCamera ()
		{
			Debug.LogWarning ("SWITCHING CAMERAS");
			if (FlyingCamera.gameObject.GetActive ())
			{
				FlyingCamera.gameObject.GetComponent<Camera> ().enabled = false;
				FlyingCamera.gameObject.SetActive (false);
				_camera.SetActive (true);
			}
			else
			{
				FlyingCamera.gameObject.GetComponent<Camera> ().enabled = true;
				FlyingCamera.gameObject.SetActive (true);
				_camera.SetActive (false); 
			}

		}

		private void destroyBox ()
		{
			Debug.Log ("Destroying the box");
			Destroy (_map.transform.Find ("BoxPrefab").gameObject);
		}

		private int handlePlayerLoaded ()
		{
			return ++_loadedCount;
		}

		private void handleMapLoadNetworkEvent (byte map)
		{
			switch (map)
			{
				case 0x0:
					loadMap ("Map");
					return;
				default:
					Debug.LogWarning ("Tried to load a map that didn't exist with id : " + map);
					return;
			}
		}

		private void loadMap (string name)
		{
			FlyingCamera.gameObject.SetActive (false);
			Debug.Log ("Loading map " + name);
			_map = (GameObject)Instantiate (Resources.Load (name), Vector3.zero, Quaternion.identity);
			Destroy (_waitForGameStartCanvas);
			Vector3 spawnPosition = _map.transform.Find ("BoxPrefab").transform.position + Vector3.up * 10;
			spawnPosition.x = Random.Range (-9f, 9f);
			spawnPosition.z = Random.Range (-9f, 9f);
			_player = PhotonNetwork.Instantiate ("Player", spawnPosition, Quaternion.identity, 0);
			_player.transform.Find ("bottom").GetComponentInChildren<Canvas> ().GetComponent<Text> ().text = _nickname;
			_player.GetComponent<CrosshairUI> ().enabled = true;
			_player.GetComponentInChildren<PlayerController> ().enabled = true;
			_player.GetComponent<ShooterB> ().enabled = true;
			_player.GetComponentInChildren<LookTowardCamera> ().enabled = true;
			_player.GetComponentInChildren<CameraControl> ().enabled = true;
			_camera = _player.transform.Find ("TPScamera/firstCamera").gameObject;

			// SET THE NICKNAME CANVAS
			// Client only stuff
			if (!PhotonNetwork.isMasterClient)
			{
				NetworkEventHandlers.SendEvent (new MapLoadedEvent ());
			}
			// Master only stuff
			else
			{
				GameObject.Find ("PowerupManager").gameObject.GetComponent<PowerupController> ().enabled = true;
			}
			if (!GameObject.Find ("SettingsManager").GetComponent<Settings> ().OnlineMode)
				spawnAI (25);
			removeWalls ();
		}

		private void init ()
		{
			findMatchmakingObjects ();
			addMatchmakingListeners ();
		}

		private void addMatchmakingListeners ()
		{
			_roomCreationButton.onClick.AddListener (roomCreationClick);
			_refreshButton.onClick.AddListener (refreshClick);
		}

		private void addPlayMenuListeners ()
		{
			_startGameButton.onClick.AddListener (startButtonClick);
		}

		private void refreshClick ()
		{
			Debug.Log ("Clicked on refresh");
			var rooms = PhotonNetwork.GetRoomList ();
			var str = "";
			foreach (var r in rooms)
			{
				str += r.Name + "\n";
			}
			_roomList.text = str;
		}


		void OnGUI ()
		{
			if (PhotonNetwork.isMasterClient)
				GUILayout.Label (PhotonNetwork.connectionStateDetailed.ToString () + " Count in room : " + PhotonNetwork.room.PlayerCount + " Count Loaded : " + _loadedCount + " Count Lost : " + _lostCount);
			else
				GUILayout.Label (PhotonNetwork.connectionStateDetailed.ToString ());
		}

		private void findMatchmakingObjects ()
		{
			_roomCreationButton = GameObject.Find ("CreateRoomButton").GetComponent<Button> ();
			_refreshButton = GameObject.Find ("RefreshButton").GetComponent<Button> ();
			_nameInput = GameObject.Find ("RoomNameInput").GetComponent<InputField> ();
			_roomList = GameObject.Find ("RoomListView").GetComponent<Text> ();
			_nicknameInput = GameObject.Find ("NicknameInput").GetComponent<InputField> ();
		}

		private void roomCreationClick ()
		{
			Debug.Log ("Creating a room with text : " + _nameInput.text + " by nickname : " + _nicknameInput.text);
			_nickname = _nicknameInput.text;
			var options = new RoomOptions ();
			options.MaxPlayers = 255;
			PhotonNetwork.JoinOrCreateRoom (_nameInput.text, options, null);
		}

		private void connectToServer (bool online)
		{
			if (!online)
			{
				PhotonNetwork.offlineMode = true;
				PhotonNetwork.CreateRoom ("some name");
			}
			else
			{
				PhotonNetwork.ConnectUsingSettings ("v0.1");
			}
		}

		private void findPlayMenuObjects ()
		{
			_playerCountText = GameObject.Find ("NumberOfPlayersText").GetComponent<Text> ();
			_startGameButton = GameObject.Find ("StartGameButton").GetComponent<Button> ();
		}

		void OnJoinedLobby ()
		{
			Debug.Log ("Joined a lobby.");
			var rooms = PhotonNetwork.GetRoomList ();
			Debug.Log (rooms.Length + " rooms available");
			refreshClick ();
		}

		void OnJoinedRoom ()
		{
			Debug.Log ("Joined a room");
			Destroy (_matchMakingCanvas);
			_waitForGameStartCanvas = Instantiate (_waitForGameStartCanvas);
			findPlayMenuObjects ();
			addPlayMenuListeners ();
		}

		void OnPhotonPlayerConnected (PhotonPlayer other)
		{
			Debug.Log ("Another player joined the room...");
			/*
			 * I'm not sure of the way the game is handling disconnections...
			 */
			if (!PhotonNetwork.isMasterClient)
				_startGameButton.interactable = false;
		}

		private void startButtonClick ()
		{
			if (!PhotonNetwork.isMasterClient)
				return;
			//_startGameButton.interactable = false;
			NetworkEventHandlers.SendEvent (new LoadMapEvent ());
			loadMap ("Map");
		}

		private void spawnAI (int x)
		{
			Vector3 spawnPosition = _map.transform.Find ("BoxPrefab").transform.position + Vector3.up * 10;
			spawnPosition.x = Random.Range (-9f, 9f);
			spawnPosition.z = Random.Range (-9f, 9f);
			for (int i = 0; i < x; i++)
			{
				Instantiate (Resources.Load ("IA"), spawnPosition, Quaternion.identity);
				_loadedCount++;
			}
		}

		private void RemoveVisionImpaired ()
		{
			_camera.GetComponent<CameraFilterPack_FX_Glitch1> ().enabled = false;
		}
	}

}
