using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPSBounds : MonoBehaviour {

    public Transform cam;
	// Use this for initialization
	void Start ()
    {
       		
	}
	
	// Update is called once per frame
	void Update ()
    {
        transform.position = cam.position + new Vector3(0, 0, 10);
	}
}
