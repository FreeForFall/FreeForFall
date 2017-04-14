using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour {

    public Transform player; //the full object (in this case robot 1)
    public Transform target; //the target is a children of the body, it is the point where is looking the camera in TPS view
    public float orbitDegreesPerSec = 0.5f;
    public float sensivity = 4f; //sensivity is only used for the upward and downward speed int TPS. (but i need to ad a variablefor all other movement)
    public Vector3 relativeDistance; // distance at witch the camera will orbit arround the object
    public Vector3 initial_distance; // initializing the distance of the camera from the object
    public bool once = true;
    public float MaxCameraAngleUp = 12; //max integer hieght of the camera in TPS
    public float MaxCameraAngleDown = 2; // min integer hight of the camera in TPS
    public Camera Principale; // the principale camera the will be the onlyone used by the player 
    public Camera Secondaire; // a secondary camera that will stay in orbit (can be used for the big screen and such)
    public Transform FPSAnchor; // the place where the camera goes whene in FPS mode
    private bool istps = true; // the bool to know if the player is in TPS or FPS
    private Vector3 eulerAngle; // suposed to calculate the angle on the x axis for the FPS view (but ... mehhhh not really working that great...)
    private float verticalOrientation; 


    void Start()
    {
        verticalOrientation = 0f;
        eulerAngle = new Vector3(0f, 0f, 0f);
        Secondaire.enabled = false;
        Principale.enabled = true;
        Cursor.visible = false;
        //initialisation of the Camera (remember the camera is an empty object that host 2 real camera, the primary and secondary cameras. 
        initial_distance = new Vector3(2,2,-15);
        if (target != null)
        {
            transform.position = player.position + initial_distance;
            relativeDistance = transform.position - player.position;
        }
    }

    //the main methode

    void Orbit()
    {
        //check for any input

        float h = Input.GetAxis("Mouse X");
        float v = Input.GetAxis("Mouse Y");
        //position the camera depending of the camera mode
        if (istps)
        {
            Principale.transform.position = Vector3.MoveTowards(Principale.transform.position, transform.position, 20 * 0.125f);
            Principale.transform.rotation = transform.rotation;
        }
        else
            Principale.transform.position = Vector3.MoveTowards(Principale.transform.position, FPSAnchor.position, 20 * 0.125f);
        //no matter what mode you are in the rotation for the Horizontal axis is the same
        if (target != null)
        {
            
            transform.position = (target.position + relativeDistance);
            transform.RotateAround(target.position, Vector3.up, orbitDegreesPerSec* h * 0.125f);
            if (once)
            {

                once = false;
            }
            relativeDistance = transform.position - target.position;
        }
        //here is the rotation for the TPS camera (it just raise or decrease the height of the camera.
        if (v != 0 && istps)
        {
            if (transform.position.y < target.position.y + MaxCameraAngleUp && v > 0)
            {
                Vector3 mouvement_up = new Vector3(0, v + sensivity , 0);
                relativeDistance = Vector3.Slerp(relativeDistance, relativeDistance + mouvement_up, 2 * 0.125f);
            }
            if(transform.position.y > target.position.y - MaxCameraAngleDown && v <0)
            {
                Vector3 mouvement_up = new Vector3(0, v - sensivity , 0);
                relativeDistance = Vector3.Slerp(relativeDistance, relativeDistance + mouvement_up, 2 * 0.125f);

            }
        }
        //here is the rotation for the fps view (need to be fixed, right now if you look up or down the camera directly look forward (so if you look east and then look up you'll rotate toward north ...)
        if (v !=0 && !istps)
        {
            verticalOrientation += v * 5;
            eulerAngle = new Vector3(verticalOrientation, 0, 0);
            Principale.transform.eulerAngles = this.eulerAngle;
        }
    }


    void Update()
    {
        //change between FPS and TPS view
        if ((Input.GetKeyDown(KeyCode.Joystick1Button9) || Input.GetKeyDown(KeyCode.E)))
        {
            istps = !istps;
        }
        Orbit();
    }

    void LateUpdate()
    {
        // say the the camera to look at the target if in TPS mode, otherwise it should look forward.
        if (istps)
        {
            Vector3 lookto = new Vector3(target.position.x, target.position.y + 2, target.position.z);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(lookto - transform.position), 40 * 0.125f);
        }
    }
}



