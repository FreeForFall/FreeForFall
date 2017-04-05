using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour {


    public Transform target;
    public float orbitDegreesPerSec = 180.0f;
    public float sensivity = 1f;
    public Vector3 relativeDistance;
    public Vector3 initial_distance;
    public bool once = true;
   

    void Start()
    {
        initial_distance = new Vector3(0,2,-10);
        if (target != null)
        {
            transform.position = target.position + initial_distance;
            relativeDistance = transform.position - target.position;
        }
    }

    void Orbit()
    {
        float h = Input.GetAxis("Mouse X");
        float v = Input.GetAxis("Mouse Y");
        Vector3 lookto = new Vector3(target.position.x, target.position.y + 2, target.position.z);
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(lookto - transform.position), Time.deltaTime * 10f);
        if (target != null)
        {
            
            transform.position = (target.position + relativeDistance);
            transform.RotateAround(target.position, Vector3.up, orbitDegreesPerSec* h * Time.deltaTime);
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
                Vector3 mouvement_up = new Vector3(0, v * sensivity , 0);
                relativeDistance = Vector3.Lerp(relativeDistance, relativeDistance + mouvement_up,Time.deltaTime* sensivity * 10f);
            }
            if(transform.position.y > target.position.y - 0.5 && v <0)
            {
                Vector3 mouvement_up = new Vector3(0, v * sensivity , 0);
                relativeDistance = Vector3.Lerp(relativeDistance, relativeDistance + mouvement_up,Time.deltaTime * sensivity * 10f);

            }
        }

    }

    void LateUpdate()
    {
        Orbit();
    }

    private Vector3 getPositionBehind(Transform o, float behind, float above)
    {
        return o.transform.position - (target.transform.forward * behind) + (target.transform.up * above);
    }
}



