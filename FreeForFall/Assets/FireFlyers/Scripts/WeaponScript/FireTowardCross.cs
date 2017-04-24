using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireTowardCross : MonoBehaviour {

    public Transform fireray;
    Vector3 lookpoint;
    public Camera TheOne;
    Ray ray;
    RaycastHit hit;

	// Use this for initialization
	void Start ()
    {
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (Physics.Raycast(fireray.position, fireray.forward, 100000f, 1))
        {
            transform.LookAt(hit.point);
        }
	}
}
