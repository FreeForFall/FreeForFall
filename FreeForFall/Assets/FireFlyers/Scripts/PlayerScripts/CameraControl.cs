using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour {

    public Transform player;
    public Transform target;
    public float orbitDegreesPerSec = 0.5f;
    public float sensivity = 4f;
    public Vector3 relativeDistance;
    public Vector3 initial_distance;
    public bool once = true;
    public float MaxCameraAngleUp = 12;
    public float MaxCameraAngleDown = 2;
    public Camera Principale;
    public Camera Secondaire;
    public Transform FPSAnchor;
    public Transform FPSlookto;
    private bool istps = true;
    private Vector3 eulerAngle;
    private float verticalOrientation;
    private float horizontalOrientation;
    private Quaternion rotation;


    void Start()
    {
        verticalOrientation = 0f;
        horizontalOrientation = 0f;
        eulerAngle = new Vector3(0f, 0f, 0f);
        Secondaire.enabled = false;
        Principale.enabled = true;
        Cursor.visible = false;
        initial_distance = new Vector3(2,2,-15);
        if (target != null)
        {
            transform.position = player.position + initial_distance;
            relativeDistance = transform.position - player.position;
        }
    }

    void OrbitTPS()
    {
        float h = Input.GetAxis("Mouse X");
        float v = Input.GetAxis("Mouse Y");
        if (istps)
        {
            Principale.transform.position = Vector3.MoveTowards(Principale.transform.position, transform.position, 60 * 0.125f);
            Principale.transform.rotation = transform.rotation;
        }
        else
            Principale.transform.position = Vector3.MoveTowards(Principale.transform.position, FPSAnchor.position, 60 * 0.125f);
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
        if (v !=0 && !istps)
        {
            horizontalOrientation = h * 5;
            verticalOrientation = v * 5;
            this.eulerAngle.x -= verticalOrientation;
            this.eulerAngle.y += horizontalOrientation;
            Principale.transform.eulerAngles = this.eulerAngle;
        }
    }

    void FPSview()
    {
        Principale.transform.position = Vector3.MoveTowards(Principale.transform.position, FPSAnchor.position, 40 * 0.125f);
        horizontalOrientation = Input.GetAxis("Mouse X") * 5;
        verticalOrientation = Input.GetAxis("Mouse Y") * 5;
        this.eulerAngle.x -= verticalOrientation;
        this.eulerAngle.y += horizontalOrientation;
        Principale.transform.eulerAngles = this.eulerAngle;
    }

    void Update()
    {
        if ((Input.GetKeyDown(KeyCode.Joystick1Button9) || Input.GetKeyDown(KeyCode.E)))
        {
            istps = !istps;
        }
        OrbitTPS();
        /*
        if (istps)
        {
            OrbitTPS();
        }
        else
        {
            FPSview();   
        }
        */
    }

    void LateUpdate()
    {
        if (istps)
        {
            lookto();
        }
    }

    void lookto()
    {
        Vector3 lookto = new Vector3(target.position.x, target.position.y + 2, target.position.z);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(lookto - transform.position), 40 * 0.125f);
    }
}



