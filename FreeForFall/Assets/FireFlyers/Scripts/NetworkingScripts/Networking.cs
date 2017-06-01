using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AssemblyCSharp;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


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

    private Constants.MAPS_IDS _mapID;

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
		GUILayout.Label (PhotonNetwork.connectionStateDetailed.ToString ());
	}
	#endif

    void Start()
    {
        Debug.Log("NETWORKING START");
        DontDestroyOnLoad(this);
        findAllUIElements();
        registerEventHandlers();
        PhotonNetwork.OnEventCall += handler;
        Debug.Log(PhotonNetwork.OnEventCall.GetInvocationList().Length);
        PhotonNetwork.autoJoinLobby = true;
        connectToServer(GameObject.Find("SettingsManager").GetComponent<Settings>().OnlineMode);
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
        switch (eventID)
        {
            case Constants.EVENT_IDS.LOAD_SCENE:
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

            case Constants.EVENT_IDS.END_GAME:
                EndGame((string[])c);
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
        Invoke("doRemove", Constants.START_GAME_DELAY);
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
        Debug.Log("Joined a lobby.");
        var rooms = PhotonNetwork.GetRoomList();
        Debug.Log(rooms.Length + " rooms available");
        refreshRooms();
    }

    /// <summary>
    /// Raised when the player joins a room.
    /// </summary>
    void OnJoinedRoom()
    {
        Debug.Log("Joined a room");
        GameObject.Find("WaitForGameStartCanvas").GetComponent<Canvas>().enabled = true;
        GameObject.Find("MatchmakingCanvas").GetComponent<Canvas>().enabled = false;
        if (!PhotonNetwork.isMasterClient)
            _waitingForGameStartText.text = PhotonNetwork.room.PlayerCount + " players are in the room.";
        _engine = new GameEngine(_mapID);
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
            Vector3 pos = _roomCreationButton.transform.position;
            _roomCreationButton.transform.position = new Vector3(_mapChooser.transform.position.x, pos.y, pos.z);
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
        _mapChooser = GameObject.Find("MapChoosingDropdown").GetComponent<Dropdown>();
        _startGameButton = GameObject.Find("StartGameButton").GetComponent<Button>();
        _roomNameInput = GameObject.Find("RoomNameInput").GetComponent<InputField>();
        _waitingForGameStartText = GameObject.Find("WaitForGameStartCanvas").transform.FindChild("Text").GetComponent<Text>();
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
        _engine.StartGame();
    }


    /// <summary>
    /// Ends the game.
    /// </summary>
    private void doEndGame()
    {
        PhotonNetwork.OnEventCall -= handler;
        Destroy(GameObject.Find("SettingsManager"));
        PhotonNetwork.LeaveRoom();
        PhotonNetwork.Disconnect();
        SceneManager.LoadScene("Menu");
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
        switch (_mapChooser.GetComponentInChildren<Text>().text)
        {
            case "Map1":
                return Constants.MAPS_IDS.SPACE_MAP;
            case "Map2":
                return Constants.MAPS_IDS.BASIC_MAP;
            default:
                return Constants.MAPS_IDS.SPACE_MAP;
        }
    }

    /// <summary>
    /// Wrapper around doEndGame. This is what should display and leaderboard.
    /// </summary>
    public void EndGame(string[] leaderboard)
    {
        foreach (var v in leaderboard)
        {
            Debug.Log(v);
        }
        Invoke("doEndGame", 5f);
    }

    private void removeVisionImpaired()
    {
        _engine.FPSCamera.GetComponent<CameraFilterPack_FX_Glitch1>().enabled = false;
    }
}
