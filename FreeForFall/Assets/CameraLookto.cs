using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraLookto : MonoBehaviour {

    public Transform target;
    public float looktospeed = 40f;
	// Use this for initialization
	void Start ()
    {
		
	}

    // Update is called once per frame
    void LateUpdate()
    {
        Vector3 lookto = new Vector3(target.position.x, target.position.y + 2, target.position.z);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(lookto - transform.position), looktospeed * 0.125f);
    }
}
