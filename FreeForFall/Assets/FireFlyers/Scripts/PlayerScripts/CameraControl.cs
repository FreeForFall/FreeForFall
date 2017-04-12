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
   

    void Start()
    {
        Cursor.visible = false;
        initial_distance = new Vector3(2,2,-15);
        if (target != null)
        {
            transform.position = player.position + initial_distance;
            relativeDistance = transform.position - player.position;
        }
    }

    void Orbit()
    {
        float h = Input.GetAxis("Mouse X");
        float v = Input.GetAxis("Mouse Y");
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
        if (v != 0)
        {
            if (transform.position.y < target.position.y + 9 && v > 0)
            {
                Vector3 mouvement_up = new Vector3(0, v + sensivity , 0);
                relativeDistance = Vector3.Slerp(relativeDistance, relativeDistance + mouvement_up,2* 0.125f);
            }
            if(transform.position.y > target.position.y - 0.5 && v <0)
            {
                Vector3 mouvement_up = new Vector3(0, v - sensivity , 0);
                relativeDistance = Vector3.Slerp(relativeDistance, relativeDistance + mouvement_up,2 * 0.125f);

            }
        }

    }

    void Update()
    {
        Orbit();
    }
}



