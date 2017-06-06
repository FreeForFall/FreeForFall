using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
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

    private bool _hasAlreadySpawned;

    private Networking _network;

    private Stack<string> _lostList;

    private Constants.ROBOT_IDS _robotID;

    private Text _chatText;

    private List<string> _chatMessages;

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

    public GameObject FPSCamera
    {
        get
        {
            return _camera;
        }
    }
    
    /// <summary>
    /// Initializes a new instance of the <see cref="GameEngine"/> class.
    /// </summary>
    /// <param name="map">The map.</param>
    public GameEngine(Constants.MAPS_IDS map, Constants.ROBOT_IDS robot)
    {
        _chatMessages = new List<string>();
        _robotID = robot;
        _hasAlreadySpawned = false;
        _loadedCount = 0;
        _lostCount = 0;
        _playerSpawned = 0;
        _mapID = map;
        _lostList = new Stack<string>();
        _network = GameObject.Find("NetworkManager").GetComponent<Networking>();
    }

    private void updateReferences()
    {    
        _network = GameObject.Find("NetworkManager").GetComponent<Networking>();
        if (GameObject.Find("FlyingCamera") != null)
            _flyingCamera = GameObject.Find("FlyingCamera").GetComponent<Camera>();
        _map = GameObject.Find(Constants.MAPS_NAMES[(int)_mapID]);
    }


    /// <summary>
    /// Starts the game.
    /// </summary>
    public void StartGame()
    {
        if (!PhotonNetwork.isMasterClient)
            Debug.LogError("NON MASTER CLIENT TRIED TO START A GAME WTF");
        _playerCount = PhotonNetwork.room.PlayerCount;
        _loadedCount++;
        LoadScene(_mapID);
    }

    /// <summary>
    /// To be called when a player loaded the map.
    /// </summary>
    public void PlayerLoadedScene()
    {
        _loadedCount++;
        if (_loadedCount != _playerCount)
            return;
        // We can tell everybody to spawn
        SpawnPlayers();
    }

    /// <summary>
    /// To be called when a player is done executing ReadyMap
    /// </summary>
    public void PlayerSpawned()
    {
        Debug.Log("A player spawned");
        _playerSpawned++;
        if (_playerSpawned != _playerCount)
            return;
        // We can remove the walls
        _network.RemoveWalls();
    }

    /// <summary>
    /// Spawns the players.
    /// </summary>
    public void SpawnPlayers()
    {
        Debug.Log("Spawning all the players");
        _playerSpawned++;
        CreatePlayer();
        NetworkEventHandlers.Broadcast(Constants.EVENT_IDS.SPAWN_PLAYER);
    }

    /// <summary>
    /// Sets the nicknames.
    /// </summary>
    private void setNicknames()
    {
        foreach (GameObject o in GameObject.FindGameObjectsWithTag ("Player"))
        {
            PhotonView p = o.GetComponent<PhotonView>();
            if (p == null)
                continue;
            o.transform
				.Find("bottom")
				.Find("Canvas").gameObject.SetActive(true);
            o.transform
				.Find("bottom")
				.Find("Canvas")
				.Find("Text")
				.GetComponent<Text>().text = p.owner.NickName;
        }
    }

    /// <summary>
    /// Removes the walls.
    /// </summary>
    public void RemoveWalls()
    {
        GameObject.Destroy(GameObject.Find("BoxPrefab"));
        setNametags();
        _localPlayer.GetComponent<Chat>().enabled = true;
    }

    /// <summary>
    /// Loads the map.
    /// </summary>
    public void LoadScene(Constants.MAPS_IDS id)
    {
        _mapID = id;
        string name = Constants.MAP_SCENES_NAMES[(int)id];
        Debug.Log(_playerCount + " - " + _playerSpawned);
        SceneManager.LoadScene(name);
    }

    /// <summary>
    /// Sets the map up
    /// </summary>
    public void ReadyMap()
    {
        Debug.Log("Readying the map");
        Debug.Log(GameObject.Find("FlyingCamera"));
        updateReferences();
        _cameraStartPosition = _flyingCamera.transform.position;

    }

    /// <summary>
    /// Creates the player.
    /// </summary>
    public void CreatePlayer()
    {
        if (_hasAlreadySpawned)
        {
            Debug.Log("Trying to create a player I already had");
            return;
        }
        _hasAlreadySpawned = true;
        updateReferences();
        Vector3 spawnPosition = _map.transform.Find("BoxPrefab").transform.position + Vector3.up * 15;
        spawnPosition.x = Random.Range(-9f, 9f);
        spawnPosition.z = Random.Range(-9f, 9f);
        _localPlayer = PhotonNetwork.Instantiate(Constants.ROBOT_NAMES[(int)_robotID], spawnPosition, Quaternion.identity, 0);
        _localPlayer.transform.Find("Canvas").gameObject.SetActive(true);
        _localPlayer.GetComponent<CrosshairUI>().enabled = true;
        _localPhotonView = _localPlayer.GetComponent<PhotonView>();
        //_localPlayer.transform.Find("bottom").Find("Canvas").Find("Text").GetComponent<Text>().text = PhotonNetwork.playerName;
        _localPlayer.GetComponentInChildren<PlayerController>().enabled = true;
        _chatText = _localPlayer.transform.Find("Canvas").Find("ChatText").GetComponent<Text>();
        _localPlayer.GetComponent<ShooterB>().enabled = true;
        _shooterB = _localPlayer.GetComponent<ShooterB>();
        _localPlayer.GetComponent<UI>().enabled = true;
        _localPlayer.GetComponentInChildren<LookTowardCamera>().enabled = true;
        _localPlayer.GetComponentInChildren<CameraControl>().enabled = true;
        _flyingCamera.gameObject.SetActive(false);
        _camera = _localPlayer.transform.Find("TPScamera/firstCamera").gameObject;
        _camera.GetComponent<AudioListener>().enabled = true;

        if (!PhotonNetwork.isMasterClient)
            NetworkEventHandlers.Broadcast(Constants.EVENT_IDS.PLAYER_SPAWNED);
        else
        {
            GameObject.Instantiate(Resources.Load("PowerupManager"));
        }

        if (!GameObject.Find("SettingsManager").GetComponent<Settings>().OnlineMode)
            spawnAI(Constants.NUMBER_OF_AI);
    }

    private void setNametags()
    {
        foreach (var player in GameObject.FindGameObjectsWithTag("Player"))
        {
            if (player == _localPlayer)
                Debug.Log("LOCAL PLAYER");
            var bottom = player.transform.Find("bottom");
            if (bottom == null)
                continue;
            bottom.Find("Canvas").Find("Text").GetComponent<Text>().text 
                = player.GetComponent<PhotonView>().owner.NickName;
        } 
    }

    /// <summary>
    /// To be called when a player has collided with the bottom of the map.
    /// </summary>
    /// <param name="p">The player that collided</param>
    public void PlayerLost(GameObject p)
    {
        Debug.Log("A player lost");
        _lostList.Push(p.GetComponent<PhotonView>().owner.NickName);
        if (_lostList.Count >= _playerCount)
        {
            string[] arr = _lostList.ToArray();
            NetworkEventHandlers.Broadcast(Constants.EVENT_IDS.END_GAME, arr);
            _network.EndGame(arr);
        }
    }

    /// <summary>
    /// Switches to spectator view.
    /// </summary>
    public void SwitchToSpecView()
    {
        switchCamera();
        _network.MoveObject(_flyingCamera.gameObject, 
            _camera.transform.position, _cameraStartPosition, Constants.SPEC_CAMERA_TRAVEL_TIME);
    }

    /// <summary>
    /// Swicthes from the flying camera to the player camera and vice versa.
    /// </summary>
    private void switchCamera()
    {
        Debug.LogWarning("SWITCHING CAMERAS");
        if (_flyingCamera.gameObject.GetActive())
        {
            _flyingCamera.gameObject.GetComponent<Camera>().enabled = false;
            _flyingCamera.GetComponent<AudioListener>().enabled = false;
            _flyingCamera.gameObject.SetActive(false);
            _camera.SetActive(true);
        }
        else
        {
            _flyingCamera.gameObject.GetComponent<Camera>().enabled = true;
            _flyingCamera.gameObject.SetActive(true);
            _flyingCamera.GetComponent<AudioListener>().enabled = true;
            _camera.SetActive(false); 
        }
    }

    public void SpawnPowerup(Vector3 position, Constants.POWERUP_IDS id)
    {
        var p = (GameObject)Object.Instantiate(
                    Resources.Load("Powerup"), 
                    position, 
                    Quaternion.identity);
         
        switch (id)
        {
            case Constants.POWERUP_IDS.SPEED_BOOST_POWERUP:
                var sp = p.AddComponent<SpeedBoost>();
                sp.LocalPlayer = _localPlayer;
                break;
            case Constants.POWERUP_IDS.VISION_IMPAIRED_POWERUP:
                var im = p.AddComponent<ImpairVision>();
                im.LocalPlayer = _localPlayer;
                break;
            case Constants.POWERUP_IDS.COOLDOWN_REFRESH_POWERUP:
                var cr = p.AddComponent<CooldownRefresh>();
                cr.LocalPlayer = _localPlayer;
                break;
            default:
                var s = p.AddComponent<SpeedBoost>();
                s.LocalPlayer = _localPlayer;
                break;
        }
    }

    public void BazookaShoot(Vector3 start, Quaternion angle, Vector3 force)
    {
        GameObject shell = GameObject.Instantiate(_shooterB.projectile, start, angle);
        shell.GetComponent<Rigidbody>().AddForce(force);
        GameObject.Destroy(shell, 10f);
    }


    private void spawnAI(int x)
    {
        Vector3 spawnPosition = _map.transform.Find("BoxPrefab").transform.position + Vector3.up * 10;
        spawnPosition.x = Random.Range(-9f, 9f);
        spawnPosition.z = Random.Range(-9f, 9f);
        for (int i = 0; i < x; i++)
        {
            GameObject.Instantiate(Resources.Load("IA"), spawnPosition, Quaternion.identity);
        }
    }

    public void ReceiveChatMessage(string name, string content)
    {
        Debug.Log("Received message from " + name + " : " + content);
        _chatMessages.Add(name + " - " + content + "\n");
        updateChatDisplay();
    }

    private void updateChatDisplay()
    {
        if (_chatMessages.Count < 9)
        {
            _chatText.text += _chatMessages.Last();
            return;
        }

        while (_chatMessages.Count > 9)
            _chatMessages.RemoveAt(0);
       
        _chatText.text = "";
        foreach (var msg in _chatMessages)
        {
            _chatText.text += msg;
        }
    }
}
