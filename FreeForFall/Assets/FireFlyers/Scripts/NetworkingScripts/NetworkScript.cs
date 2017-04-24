using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


namespace AssemblyCSharp
{
	public class NetworkScript : MonoBehaviour
	{
		/*

		TODO : Rewrite this, at the moment I need have 2 find*Objects functions which is not pretty.
		Instantiating the two menus from the beginning and only rendering one of them could be cool.

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
		private Text _roomList;
		private Text _playerCountText;
		private Button _startGameButton;

		private GameObject _player;
		private GameObject _camera;

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
		}

		// Need to introduce safe casting of the objects sent over the network, this is really dangerous.
		private void handleNetworkEvents (byte eventCode, object content, int senderId)
		{
			switch (eventCode)
			{
				case 0x0:
					handleMapLoadNetworkEvent (((byte[])content) [0]);
					break;
				case 0x1:
					removeWalls ();
					break;
				case 0x3:
				// remove walls
					Invoke ("switchCamera", 3);
					Invoke ("destroyBox", 5);
					break;
				case 0x19:
					// Player lost
					handlePlayerLost ();
					break;
				case 0x99:
					// End of the game
					endGame ();
					break;
				default:
					Debug.LogWarning ("Received unknown event with code " + eventCode);
					return;
			}
		}

		private void endGame ()
		{
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
			if (playerLost () < PhotonNetwork.room.PlayerCount)
			{
				Debug.Log ("a player lost, but he wasn't the last one");
				return;
			}
			Debug.Log ("A player lost and was the last man standing.");
			NetworkEventHandlers.SendEvent (new EndGameEvent ());
			endGame ();
		}

		private void removeWalls ()
		{
			if (!PhotonNetwork.isMasterClient)
				return;
			if (handlePlayerLoaded () >= PhotonNetwork.room.PlayerCount)
			{
				Debug.Log ("REMOVING WALLS");
				NetworkEventHandlers.SendEvent (new RemoveWallsEvent ());
				Invoke ("switchCamera", 3);
				Invoke ("destroyBox", 5);
			}
		}

		private void switchCamera ()
		{
			GameObject.Find ("Camera").SetActive (false);
			_camera.GetComponent<Camera> ().enabled = true;
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
			Debug.Log ("Loading map " + name);
			_map = (GameObject)Instantiate (Resources.Load (name), Vector3.zero, Quaternion.identity);
			Destroy (_waitForGameStartCanvas);
			Vector3 spawnPosition = _map.transform.Find ("BoxPrefab").transform.position + Vector3.up * 10;
			spawnPosition.x = Random.Range (-9f, 9f);
			spawnPosition.z = Random.Range (-9f, 9f);
			_player = PhotonNetwork.Instantiate ("Player", spawnPosition, Quaternion.identity, 0);
			_player.GetComponent<CrosshairUI> ().enabled = true;
			_player.GetComponentInChildren<PlayerController> ().enabled = true;
            _player.GetComponent<ShooterB>().enabled = true;
			//_player.GetComponentInChildren<LookTowardCamera> ().enabled = true;
			_player.GetComponentInChildren<CameraControl> ().enabled = true;
			_player.transform.Find ("TPScamera/firstCamera").gameObject.GetComponent<Camera> ().enabled = true;
			// Don't forget to remove the camera of the other players
			removeWalls ();
			if (!PhotonNetwork.isMasterClient)
			{
				NetworkEventHandlers.SendEvent (new MapLoadedEvent ());
			}
            if (!GameObject.Find("SettingsManager").GetComponent<Settings>().OnlineMode)
                spawnAI(2);
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
				GUILayout.Label (PhotonNetwork.connectionStateDetailed.ToString () + " Count in room : " + PhotonNetwork.room.PlayerCount + " Count Loaded : " + _loadedCount);
			else
				GUILayout.Label (PhotonNetwork.connectionStateDetailed.ToString ());
		}

		private void findMatchmakingObjects ()
		{
			_roomCreationButton = GameObject.Find ("CreateRoomButton").GetComponent<Button> ();
			_refreshButton = GameObject.Find ("RefreshButton").GetComponent<Button> ();
			_nameInput = GameObject.Find ("RoomNameInput").GetComponent<InputField> ();
			_roomList = GameObject.Find ("RoomListView").GetComponent<Text> ();
		}

		private void roomCreationClick ()
		{
			Debug.Log ("Creating a room with text : " + _nameInput.text);
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
            Vector3 spawnPosition = _map.transform.Find("BoxPrefab").transform.position + Vector3.up * 10;
            spawnPosition.x = Random.Range(-9f, 9f);
            spawnPosition.z = Random.Range(-9f, 9f);
            for(int i=0; i<x;i++)
            {
                Instantiate(Resources.Load("IA"), spawnPosition, Quaternion.identity);
            }
        }
	}

}
