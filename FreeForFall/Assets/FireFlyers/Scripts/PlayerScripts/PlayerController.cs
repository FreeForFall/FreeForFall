using UnityEngine;
using UnityEngine.SceneManagement;
using AssemblyCSharp;

public class PlayerController : MonoBehaviour
{
    public GameObject jumpeffect;
    public Transform jumpflamelocation;
    public GameObject portal;
    public GameObject speedparticle;
    public float TimeSincePreviousJump;
    private float sprintMultiplier;
    private bool airbone;
    private Rigidbody rigidBody;
    private Quaternion rotation = Quaternion.identity;
    private Quaternion cam = Quaternion.identity;
    private Rigidbody rb;
    public Transform playerCamera;
    private Vector3 initial_Camera;
    public Transform spawn;
    public bool ground_powerup = true;
    private float _speedBoost;
    public AudioSource motorsound;
    public AudioSource jumpsound;
    private GameObject _spawn;

    // Use this for initialization
    void Start()
    {
        TimeSincePreviousJump = 0f;
        this.sprintMultiplier = 1f;
        this.airbone = false;
        this.rigidBody = this.GetComponent<Rigidbody>();
        rb = GetComponent<Rigidbody>();
        _speedBoost = 1f;
        _spawn = GameObject.Find("DevSpawn");
    }

    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.name == "ground")
        {
            this.airbone = false;
        }
    }

    void OnCollisionExit(Collision other)
    {
        if (other.gameObject.name == "ground")
        {
            this.airbone = true;
        }
    }

    private void jump()
    {

        this.rigidBody.AddForce(new Vector3(0f, Constants.JUMP_FORCE, 0f));

    }


    private void doMovement()
    {
        if ((Input.GetKeyDown(KeyCode.Joystick1Button0)
            || Input.GetKeyDown(KeyCode.Space))
            && !this.airbone
            && Constants.JUMP_CD < TimeSincePreviousJump)
        {
            GameObject JumpeffectDone = Instantiate(jumpeffect, jumpflamelocation.transform.position, jumpflamelocation.transform.rotation) as GameObject;
            jumpsound.Play();
            TimeSincePreviousJump = 0f;
            jump();
            Destroy(JumpeffectDone, 0.5f);
        }

        if (Input.GetKey(KeyCode.F1) || Input.GetKey(KeyCode.Joystick1Button7))
        {
            GameObject.Find("NetworkManager").GetComponent<Networking>().menu();
        }
        #if UNITY_STANDALONE_WIN
        if (Input.GetKey (KeyCode.LeftShift) || Input.GetKey (KeyCode.Joystick1Button8))
        #endif
        #if UNITY_STANDALONE_LINUX
		if (Input.GetKey (KeyCode.LeftShift) || Input.GetKey (KeyCode.JoystickButton9))
        #endif
        {
            this.sprintMultiplier = Constants.SPRINT_SPEED;
        }
        else
        {
            this.sprintMultiplier = 1f;
        }
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        if (h > 0 || h < 0 || v > 0 || v < 0)
        {
            Vector3 direction = new Vector3(h, 0, v);
            rotation = Quaternion.LookRotation(direction, Vector3.up);
            cam = new Quaternion(0, playerCamera.rotation.y, 0, playerCamera.rotation.w);
            rb.rotation = Quaternion.Lerp(transform.rotation, cam * rotation, 800 * Time.deltaTime);
            transform.position = Vector3.Lerp(transform.position, 
                transform.position + transform.forward * Constants.MOVEMENT_SPEED * _speedBoost, 
                Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.R) || Input.GetKey(KeyCode.Joystick1Button6))
        {
            if (spawn != null)
                transform.position = spawn.position;
        }
    }

    // Update is called once per frame

    public void SpeedBoost(float mult)
    {
        _speedBoost = mult;
        GameObject speedbuff = Instantiate(speedparticle, transform.position, transform.rotation, transform) as GameObject;
        Invoke("removeSpeedBoost", 2);
        Destroy(speedbuff, 2);
    }

    public void Swap()
    {
        NetworkEventHandlers.Broadcast(Constants.EVENT_IDS.SWAP_PARTICLES, transform.position);
        SpawnSwapParticles(transform.position);
        transform.position = _spawn.transform.position;
    }
    
    public void SpawnSwapParticles(Vector3 a)
    {
        GameObject portalIn = Instantiate(portal, a, transform.rotation) as GameObject;
        GameObject portalOut = Instantiate(portal, _spawn.transform.position, transform.rotation) as GameObject;
        Destroy(portalIn, 2f);
        Destroy(portalOut, 2f);
    }

    private void removeSpeedBoost()
    {
        _speedBoost = 1f;
    }

    private void DevSpawn()
    {
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKey(KeyCode.Joystick1Button1))
        {
            transform.position = _spawn.transform.position;
        }
    }

    // Update is called once per frame
    void Update()
    {
        TimeSincePreviousJump += Time.deltaTime;
        doMovement();
        DevSpawn(); 
    }
}
