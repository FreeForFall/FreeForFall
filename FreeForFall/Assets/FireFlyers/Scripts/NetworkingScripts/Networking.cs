﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AssemblyCSharp;
using System.Linq;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;


/*

This time, I'll be using RPCs as often as possible.


There are a few race conditions that should be fixed if we go large scale : createRoom and joinRoom rely on the fact
that no room is created with the name they're looking for while they're processing the room list.

After a quick glance at the source of PUN it appears that calling GetRoomList() is actually not an issue as the 
result is cached. 

*/

public class Networking : MonoBehaviour
{

    private Button _roomCreationButton;
    private Button _refreshButton;
    private InputField _roomNameInput;
    private InputField _nicknameInput;
    private Text _roomList;
    private Text _waitingForGameStartText;
    private Button _startGameButton;
    private Button _joinRoomButton;
    private Dropdown _mapChooser;
    private Dropdown _robotChooser;
    private Canvas _leaderboard;
    private Text _leaderboardText;
    private Chat _chat;
    private int _round;
    private Constants.MAPS_IDS _mapID;
    private float _countdown;

    private Toggle _volcanotoggle;
    private Toggle _spacetoggle;
    private ToggleGroup _mapSelect;
    private ToggleGroup _robotSelect;
    private Toggle _testtoggle;
    private Toggle _basicetoggle;
    private Toggle _talltoggle;
    private Toggle _shorttoggle;


    private GameEngine _engine;

    public GameObject Player
    {
        get
        {
            return _engine.Player;
        }
    }

    public GameEngine Engine
    {
        get
        {
            return _engine;
        }
    }

    #if UNITY_EDITOR
	void OnGUI ()
	{
		GUILayout.Label (PhotonNetwork.connectionStateDetailed.ToString () + " ROUND : " + _round);
	}
	#endif

    void Start()
    {
        _round = 0;
        Debug.Log("NETWORKING START");
        DontDestroyOnLoad(this);
        findAllUIElements();
        registerEventHandlers();
        PhotonNetwork.OnEventCall += handler;
        Debug.Log("Handler count : ");
        Debug.Log(PhotonNetwork.OnEventCall.GetInvocationList().Length);
        PhotonNetwork.autoJoinLobby = true;
        connectToServer(GameObject.Find("SettingsManager").GetComponent<Settings>().OnlineMode);
        _chat = GameObject.Find("ChatManager").GetComponent<Chat>();
        DontDestroyOnLoad(_chat);
    }

    /// <summary>
    /// Handles the events received by the client.
    /// </summary>
    /// <param name="eventCode">Event code.</param>
    /// <param name="content">Content.</param>
    /// <param name="senderId">Sender identifier.</param>
    private void handler(byte eventCode, object content, int senderId)
    {
        object[] c = (object[])content;
        Constants.EVENT_IDS eventID = (Constants.EVENT_IDS)eventCode;
        Debug.Log("RECEIVED : " + eventID + " FROM " + senderId);

        Debug.Log("Handler count : ");
        Debug.Log(PhotonNetwork.OnEventCall.GetInvocationList().Length);
        switch (eventID)
        {
            case Constants.EVENT_IDS.LOAD_SCENE:
                _round++;
                _engine.Reset();
                _engine.LoadScene((Constants.MAPS_IDS)c[0]);
                return;
            case Constants.EVENT_IDS.SCENE_LOADED:
                if (!PhotonNetwork.isMasterClient)
                    return;
                _engine.PlayerLoadedScene();
                return;
			
            case Constants.EVENT_IDS.SPAWN_PLAYER:
                if (PhotonNetwork.isMasterClient)
                {
                    Debug.LogError("Asked to spawn a player, but we are the master client");
                    return;
                }
                _engine.CreatePlayer();					
                return;	
			
            case Constants.EVENT_IDS.PLAYER_SPAWNED:
                if (!PhotonNetwork.isMasterClient)
                    return;
                _engine.PlayerSpawned();
                return;
            case Constants.EVENT_IDS.REMOVE_WALLS:
                if (PhotonNetwork.isMasterClient)
                    return;
                _engine.RemoveWalls();
                return;

            case Constants.EVENT_IDS.SPAWN_POWERUP:
                _engine.SpawnPowerup((Vector3)c[0], (Constants.POWERUP_IDS)c[1]);
                return;     

            case Constants.EVENT_IDS.IMPAIR_VISION_EFFECT:
                _engine.FPSCamera.GetComponent<CameraFilterPack_FX_Glitch1>().enabled = true;
                Invoke("removeVisionImpaired", Constants.VISION_IMPAIRED_POWERUP_DURATION); 
                return;   

            case Constants.EVENT_IDS.BAZOOKA_SHOT:
                _engine.BazookaShoot((Vector3)c[0], (Quaternion)c[1], (Vector3)c[2]);
                return;
            case Constants.EVENT_IDS.END_GAME:
                EndGame((string[])c);
                return;

            case Constants.EVENT_IDS.CHAT_MESSAGE:
                _chat.ReceiveMessage((string)c[0], (string)c[1]);
                return;

            case Constants.EVENT_IDS.SWAP_PARTICLES:
                _engine.SwapParticles((Vector3)c[0]);
                return;

            default:
                Debug.LogError("UNKNOWN EVENT");
                return;
        }
    }

    /// <summary>
    /// A wrapper around a wrapper, because Unity sucks. Removes the walls in Constants.START_GAME_DELAY seconds.
    /// </summary>
    public void RemoveWalls()
    {
        if (PhotonNetwork.isMasterClient)
        {
            _countdown = Constants.START_GAME_DELAY;
            sendCountdown();
        }
        Invoke("doRemove", Constants.START_GAME_DELAY);
    }

    private void sendCountdown()
    {
        if (_countdown == -1)
            return;
        _chat.SendChatMessage("GAME", _countdown--.ToString());    
        Invoke("sendCountdown", 0.9f);
    }

    /// <summary>
    /// Removes the walls and sends the event to all the players.
    /// </summary>
    private void doRemove()
    {
        NetworkEventHandlers.Broadcast(Constants.EVENT_IDS.REMOVE_WALLS);
        _engine.RemoveWalls();
    }

    /// <summary>
    /// Raised when the player joins the lobby.
    /// </summary>
    void OnJoinedLobby()
    {
        GameObject.Find("MatchmakingCanvas/TitleText").GetComponent<Text>().text = "CREATE ROOM";
        Debug.Log("Joined a lobby.");
        var rooms = PhotonNetwork.GetRoomList();
        Debug.Log(rooms.Length + " rooms available");
        refreshRooms();
        _joinRoomButton.interactable = true;
        _roomCreationButton.interactable = true;
    }

    /// <summary>
    /// Raised when the player joins a room.
    /// </summary>
    void OnJoinedRoom()
    {
        _chat.enabled = true;
        _chat.InMenu = true;
        Debug.Log("Joined a room");
        GameObject.Find("WaitForGameStartCanvas").GetComponent<Canvas>().enabled = true;
        if (!GameObject.Find("SettingsManager").GetComponent<Settings>().OnlineMode)
        {
            GameObject.Find("WaitForGameStartCanvas/Text").GetComponent<Text>().text = "START THE GAME";
        }    
        GameObject.Find("MatchmakingCanvas").GetComponent<Canvas>().enabled = false;
        if (!PhotonNetwork.isMasterClient)
            _waitingForGameStartText.text = PhotonNetwork.room.PlayerCount + " players are in the room.";
        Constants.ROBOT_IDS robotID;

        if (_robotSelect.ActiveToggles().FirstOrDefault() == _talltoggle)
            robotID = Constants.ROBOT_IDS.ROBOT_1;
        if (_robotSelect.ActiveToggles().FirstOrDefault() == _shorttoggle)
            robotID = Constants.ROBOT_IDS.ROBOT_2;
        else
            robotID = Constants.ROBOT_IDS.ROBOT_1;
         
        _engine = new GameEngine(_mapID, robotID);
    }

    /// <summary>
    /// Raised when another player joins your room.
    /// </summary>
    /// <param name="other">Other player</param>
    void OnPhotonPlayerConnected(PhotonPlayer other)
    {
        _waitingForGameStartText.text = PhotonNetwork.room.PlayerCount + " players are in the room.";
        if (PhotonNetwork.isMasterClient)
            _startGameButton.interactable = true;
    }

    /// <summary>
    /// Connects to the server.
    /// </summary>
    /// <param name="online">If set to <c>true</c> online.</param>
    private void connectToServer(bool online)
    {
        if (!online)
        {
            PhotonNetwork.offlineMode = true;
            _startGameButton.interactable = true;
            _roomNameInput.gameObject.SetActive(false);
            _refreshButton.gameObject.SetActive(false);
            _joinRoomButton.gameObject.SetActive(false);

            // this is a hack and won't work if the mapChooser changes too much
            _roomList.enabled = false;
            GameObject.Find("MatchmakingCanvas/AvailableText").GetComponent<Text>().text = "";
            
            GameObject.Find("MatchmakingCanvas/TitleText").GetComponent<Text>().text = "CREATE ROOM";
            _roomCreationButton.interactable = true;
        }
        else
        {
            PhotonNetwork.ConnectUsingSettings(Constants.GAME_VERSION);
        }
    }

    /// <summary>
    /// Finds all the UI elements needed to make the pre-game menus work.
    /// </summary>
    private void findAllUIElements()
    {
        _roomCreationButton = GameObject.Find("CreateRoomButton").GetComponent<Button>();
        _joinRoomButton = GameObject.Find("JoinRoomButton").GetComponent<Button>();
        _refreshButton = GameObject.Find("RefreshButton").GetComponent<Button>();
        _roomList = GameObject.Find("RoomListView").GetComponent<Text>();
        _nicknameInput = GameObject.Find("NicknameInput").GetComponent<InputField>();
//        _mapChooser = GameObject.Find("MapChoosingDropdown").GetComponent<Dropdown>();
        _startGameButton = GameObject.Find("StartGameButton").GetComponent<Button>();
        _roomNameInput = GameObject.Find("RoomNameInput").GetComponent<InputField>();
        _waitingForGameStartText = GameObject.Find("WaitForGameStartCanvas").transform.FindChild("Text").GetComponent<Text>();
//        _robotChooser = GameObject.Find("RobotChoosingDropdown").GetComponent<Dropdown>();
        _spacetoggle = GameObject.Find("Space").GetComponent<Toggle>();
        _volcanotoggle = GameObject.Find("Volcano").GetComponent<Toggle>();
        _testtoggle = GameObject.Find("Test").GetComponent<Toggle>();
        _basicetoggle = GameObject.Find("Basic").GetComponent<Toggle>();
        _talltoggle = GameObject.Find("Tallrobot").GetComponent<Toggle>();
        _shorttoggle = GameObject.Find("Shortrobot").GetComponent<Toggle>();
        _mapSelect = _volcanotoggle.group;
        _robotSelect = _talltoggle.group;

    }

    /// <summary>
    /// Registers the event handlers.
    /// </summary>
    private void registerEventHandlers()
    {
        _roomCreationButton.onClick.AddListener(createRoom);
        _refreshButton.onClick.AddListener(refreshRooms);
        _startGameButton.onClick.AddListener(startGame);
        _joinRoomButton.onClick.AddListener(joinRoom);
    }

    /// <summary>
    /// Tries to join the room.
    /// </summary>
    private void joinRoom()
    {
        string name = _roomNameInput.text;
        RoomInfo[] roomList = PhotonNetwork.GetRoomList();

        bool found = false;
        for (int i = 0; i < roomList.Length && !found; i++)
            found = name == roomList[i].Name;
		
        if (!found)
        {
            Debug.LogError("Room doesn't exist!");
            return;
        }

        PhotonNetwork.playerName = _nicknameInput.text;
        PhotonNetwork.JoinRoom(name);
    }

    /// <summary>
    /// Tries to create a room.
    /// </summary>
    private void createRoom()
    {
        Debug.Log("Creating a room");
        string name = _roomNameInput.text;
        RoomInfo[] roomList = PhotonNetwork.GetRoomList();
        int i = 0;
        for (; i < roomList.Length && roomList[i].Name != name; i++)
            continue;
        if (i != roomList.Length)
        {
            Debug.LogError("Room Already exists!");
            return;
        }
        _mapID = chooseMap();
        PhotonNetwork.playerName = _nicknameInput.text;
        PhotonNetwork.CreateRoom(_roomNameInput.text, getRoomOptions(), null);
    }

    /// <summary>
    /// Refreshs the room list.
    /// </summary>
    private void refreshRooms()
    {
        Debug.Log("Refreshing the room list");
        RoomInfo[] rooms = PhotonNetwork.GetRoomList();
        string str = "";
        foreach (var r in rooms)
        {
            str += r.Name + " - " + r.PlayerCount + "/" + r.MaxPlayers + " players.\n";
        }
        _roomList.text = str;
    }

    /// <summary>
    /// Starts the game.
    /// </summary>
    private void startGame()
    {
        if (!PhotonNetwork.isMasterClient)
        {
            Debug.LogError("A normal client clicked the start game button");
            return;
        }

        Debug.Log("Starting the game");
        _round = _round == 0 ? 1 : _round;
        _engine.StartGame();
    }





    #region utils

    /// <summary>
    /// Moves an object from a position to another over a time period.
    /// </summary>
    /// <param name="o">The object to move.</param>
    /// <param name="from">Start position.</param>
    /// <param name="to">End position.</param>
    /// <param name="time">Movement duration.</param>
    public void MoveObject(GameObject o, Vector3 from, Vector3 to, float time)
    {
        StartCoroutine(_moveObject(o.transform, from, to, time));
    }

    private IEnumerator _moveObject(Transform obj, Vector3 source, Vector3 target, float overTime)
    {
        float startTime = Time.time;
        while (Time.time < startTime + overTime)
        {
            obj.position = Vector3.Lerp(source, target, (Time.time - startTime) / overTime);
            yield return null;
        }
        obj.position = target;
    }


    #endregion


    /// <summary>
    /// Gets the room options.
    /// </summary>
    /// <returns>The room options.</returns>
    private RoomOptions getRoomOptions()
    {
        RoomOptions opt = new RoomOptions();
        opt.MaxPlayers = 255;
        return opt;
    }

    /// <summary>
    /// Returns the map selected by the player.
    /// </summary>
    /// <returns>The map.</returns>
    private Constants.MAPS_IDS chooseMap()
    {
        if (_mapSelect.ActiveToggles().FirstOrDefault() == _volcanotoggle)
            return Constants.MAPS_IDS.VOLCANO_MAP; 
        if (_mapSelect.ActiveToggles().FirstOrDefault() == _spacetoggle)
            return Constants.MAPS_IDS.SPACE_MAP;
        if (_mapSelect.ActiveToggles().FirstOrDefault() == _basicetoggle)
            return Constants.MAPS_IDS.BASIC_MAP;
        if (_mapSelect.ActiveToggles().FirstOrDefault() == _testtoggle)
            return Constants.MAPS_IDS.TEST_MAP;
        else
            return Constants.MAPS_IDS.SPACE_MAP;
    }

    public void menu()
    {
        PhotonNetwork.OnEventCall -= handler;
        Destroy(GameObject.Find("SettingsManager"));
        PhotonNetwork.LeaveRoom();
        PhotonNetwork.Disconnect();
        SceneManager.LoadScene("Menu");
    }

    /// <summary>
    /// Ends the game.
    /// </summary>
    private void doEndGame()
    {
        Debug.Log("IN DOENDGAME");
        if (_round == Constants.ROUND_COUNT)
        {
            Debug.Log("End of the game");
            menu();
            return;
        }
        Debug.Log("Playing the next round");
        _round++;
        if (!PhotonNetwork.isMasterClient)
            return;
        _engine.Reset();
        //NetworkEventHandlers.Broadcast(Constants.EVENT_IDS.LOAD_SCENE, (int)_mapID);
        
        startGame();
    }

    /// <summary>
    /// Wrapper around doEndGame. This is what should display and leaderboard.
    /// </summary>
    public void EndGame(string[] leaderboard)
    {
        Cursor.visible = true;
        _leaderboard = GameObject.Find("WinCanvas").GetComponent<Canvas>();
        _leaderboard.enabled = true;
        _chat.InMenu = true;
        _leaderboardText = GameObject.Find("WinCanvas/leaderboard").GetComponent<Text>();
        foreach (var v in leaderboard)
        {
            string[] spl = v.Split('-');
            _leaderboardText.text += "<color=#ffa500ff>" + spl[0] + "</color>";
            for (int i = 1; i < spl.Length; i++)
                _leaderboardText.text += spl[i];
            _leaderboardText.text += Environment.NewLine;
        }
        if (_round == Constants.ROUND_COUNT)
        {
            GameObject.Find("WinCanvas/ButtonToMenu/Text").GetComponent<Text>().text = "RETURN TO MENU";
            GameObject.Find("WinCanvas/ButtonToMenu").GetComponent<Button>().interactable = true;
            GameObject.Find("WinCanvas/ButtonToMenu").GetComponent<Button>().onClick.AddListener(doEndGame);
            return;
        }
        GameObject.Find("WinCanvas/ButtonToMenu/Text").GetComponent<Text>().text = "NEXT ROUND";
        Debug.Log("Changed the text");
        GameObject.Find("WinCanvas/ButtonToMenu").GetComponent<Button>().interactable = PhotonNetwork.isMasterClient;
        Debug.Log("Adding a listener");
        GameObject.Find("WinCanvas/ButtonToMenu").GetComponent<Button>().onClick.AddListener(doEndGame);
        Debug.Log("At the end of EndGame");
    }

    private void removeVisionImpaired()
    {
        _engine.FPSCamera.GetComponent<CameraFilterPack_FX_Glitch1>().enabled = false;
    }


}
