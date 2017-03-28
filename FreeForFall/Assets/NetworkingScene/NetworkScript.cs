using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace AssemblyCSharp
{
	public class NetworkScript : MonoBehaviour
	{
		/*

	TODO : Rewrite this, at the moment I need have 2 find*Objects functions which is not pretty.
	Instantiating the two menus from the beginning and only rendering one of them could be cool.

	*/
		private GameObject _matchMakingCanvas;
		private GameObject _waitForGameStartCanvas;
		private Button _roomCreationButton;
		private Button _refreshButton;
		private InputField _nameInput;
		private Text _roomList;
		private Text _playerCountText;
		private Button _startGameButton;

		void Start ()
		{
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
			default:
				Debug.LogWarning ("Received unknown event with code " + eventCode);
				return;
			}
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
			Instantiate (Resources.Load (name));
			Vector3 spawnPosition = new Vector3 (0, 33, 0);
			spawnPosition.x = Random.Range (-9f, 9f);
			spawnPosition.z = Random.Range (-9f, 9f);
			var player = PhotonNetwork.Instantiate ("Player", spawnPosition, Quaternion.identity, 0);
			player.GetComponent<PlayerController> ().enabled = true;
			player.GetComponent<Controls> ().enabled = true;
			player.transform.Find ("PlayerView").GetComponent<Camera> ().enabled = true;
			player.transform.Find ("PlayerView").GetComponent<CameraController> ().enabled = true;
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
			options.MaxPlayers = 10;
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
			if (PhotonNetwork.room.MaxPlayers < PhotonNetwork.room.PlayerCount)
				return;
			Debug.Log ("The room is now full");
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
			Destroy (_waitForGameStartCanvas);
			NetworkEventHandlers.SendEvent (new LoadMapEvent ());
			loadMap ("Map");
		}
	}

}
