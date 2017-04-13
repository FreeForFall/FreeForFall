using UnityEngine;

public class PlayerController : MonoBehaviour
{

    public float speed;
    public float maxSprintSpeed;
    private float sprintMultiplier;
    public float jumpforce;
    private bool airbone;
    private Rigidbody rigidBody;
    private Quaternion rotation;
    private Quaternion camera_rotation;
    public float rotateSpeed = 1f;
    private Rigidbody rb;
    public Transform playerCamera;  
    private Quaternion startrotation;
    private Vector3 initial_Camera;


    // Use this for initialization
    void Start()
    {
        this.sprintMultiplier = 1f;
        this.airbone = false;
        this.rigidBody = this.GetComponent<Rigidbody>();
        rb = GetComponent<Rigidbody>();


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


    private void doMovement()
    {
        if ((Input.GetKeyDown(KeyCode.Joystick1Button0) || Input.GetKeyDown(KeyCode.Space)) && !this.airbone)
        {
            this.rigidBody.AddForce(new Vector3(0f, jumpforce, 0f));
        }
        if ((Input.GetKeyDown(KeyCode.Joystick1Button3) || Input.GetKeyDown(KeyCode.F)))
        {
            BroadcastMessage("IsFallen", 100f);
        }
        if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.Joystick1Button8))
        {
            this.sprintMultiplier = this.maxSprintSpeed;
        }
        else
        {
            this.sprintMultiplier = 1f;
        }
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        if (h > 0 || h < 0 || v > 0 || v < 0)
        {
            float rotate = rotateSpeed;
            Vector3 direction = new Vector3(h, 0, v);
            rotation = Quaternion.LookRotation(direction, Vector3.up);
            Quaternion cam = new Quaternion(0, playerCamera.rotation.y, 0, playerCamera.rotation.w);
            rb.rotation = Quaternion.RotateTowards(transform.rotation, cam * rotation, rotate * 0.125f);
            Vector3 Moveto = new Vector3(0, transform.position.y, 1);
            transform.position = Vector3.MoveTowards(transform.position, transform.position + transform.forward,speed * maxSprintSpeed * 0.125f);
        }
    }

    // Update is called once per frame
    void Update()
    {

        doMovement();
    }
}
