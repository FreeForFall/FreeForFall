using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReturnToPosition : MonoBehaviour {

    private Vector3 _initialPosition;
    private Quaternion _initialRotation;


    // Use this for initialization
    void Start ()
    {
        _initialPosition = transform.position;
        _initialRotation = transform.rotation;
    }
	
	// Update is called once per frame


 

}
