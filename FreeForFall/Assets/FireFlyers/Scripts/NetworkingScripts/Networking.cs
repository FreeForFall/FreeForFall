using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AssemblyCSharp;
using UnityEngine.UI;

public class Networking : MonoBehaviour
{

	private Button _roomCreationButton;
	private Button _refreshButton;
	private InputField _roomNameInput;
	private InputField _nicknameInput;
	private Text _roomList;
	private Button _startGameButton;
	private Dropdown _mapChooser;

	private Constants.MAPS_IDS _mapID;


	#if UNITY_EDITOR
	void OnGUI ()
	{
		GUILayout.Label (PhotonNetwork.connectionStateDetailed.ToString ());
	}
	#endif




	void Start ()
	{
		findAllUIElements ();
		registerEventHandlers ();

		PhotonNetwork.autoJoinLobby = true;
		connectToServer (GameObject.Find ("SettingsManager").GetComponent<Settings> ().OnlineMode);
	}


	void OnJoinedLobby ()
	{
		Debug.Log ("Joined a lobby.");
		var rooms = PhotonNetwork.GetRoomList ();
		Debug.Log (rooms.Length + " rooms available");
		refreshRooms ();
	}

	void OnJoinedRoom ()
	{
		Debug.Log ("Joined a room");
		GameObject.Find ("WaitForGameStartCanvas").GetComponent<Canvas> ().enabled = true;
		GameObject.Find ("MatchmakingCanvas").GetComponent<Canvas> ().enabled = false;
	}

	void OnPhotonPlayerConnected (PhotonPlayer other)
	{
		Debug.Log ("Another player joined the room...");
		if (PhotonNetwork.isMasterClient)
			_startGameButton.interactable = true;
	}

	private void connectToServer (bool online)
	{
		if (!online)
		{
			PhotonNetwork.offlineMode = true;
			PhotonNetwork.CreateRoom ("Offline Game");
		}
		else
		{
			PhotonNetwork.ConnectUsingSettings (Constants.GAME_VERSION);
		}
	}

	private void findAllUIElements ()
	{
		_roomCreationButton = GameObject.Find ("CreateRoomButton").GetComponent<Button> ();
		_refreshButton = GameObject.Find ("RefreshButton").GetComponent<Button> ();
		_roomList = GameObject.Find ("RoomListView").GetComponent<Text> ();
		_nicknameInput = GameObject.Find ("NicknameInput").GetComponent<InputField> ();
		_mapChooser = GameObject.Find ("MapChoosingDropdown").GetComponent<Dropdown> ();
		_startGameButton = GameObject.Find ("StartGameButton").GetComponent<Button> ();
		_roomNameInput = GameObject.Find ("RoomNameInput").GetComponent<InputField> ();
	}

	private void registerEventHandlers ()
	{
		_roomCreationButton.onClick.AddListener (createRoom);
		_refreshButton.onClick.AddListener (refreshRooms);
		_startGameButton.onClick.AddListener (startGame);
	}

	private void createRoom ()
	{
		Debug.Log ("Creating a room");
		_mapID = chooseMap ();
		PhotonNetwork.playerName = _nicknameInput.text;
		PhotonNetwork.JoinOrCreateRoom (_roomNameInput.text, getRoomOptions (), null);
	}

	private void refreshRooms ()
	{
		Debug.Log ("Refreshing the room list");
	}

	private void startGame ()
	{
		Debug.Log ("Starting the game");
	}

	private RoomOptions getRoomOptions ()
	{
		var opt = new RoomOptions ();
		opt.MaxPlayers = 255;
		return opt;
	}

	private Constants.MAPS_IDS chooseMap ()
	{
		switch (_mapChooser.GetComponentInChildren<Text> ().text)
		{
			case "Map1":
				return Constants.MAPS_IDS.SPACE_MAP;
			case "Map2":
				return Constants.MAPS_IDS.BASIC_MAP;
			default:
				return Constants.MAPS_IDS.SPACE_MAP;
		}
	}
}
