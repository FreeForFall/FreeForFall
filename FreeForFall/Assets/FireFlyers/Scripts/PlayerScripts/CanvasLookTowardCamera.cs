using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasLookTowardCamera : MonoBehaviour
{

    // Use this for initialization
    private Transform _camera;
    void Start()
    {
        _camera = GameObject.Find("NetworkManager")
            .GetComponent<Networking>().Engine.FPSCamera.transform;
    }
	
    // Update is called once per frame
    void Update()
    {
        transform.LookAt(_camera.transform.position);
        transform.Rotate(0, 180, 0);
    }
}
