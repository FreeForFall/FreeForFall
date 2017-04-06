using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class wheelAnimation : MonoBehaviour {

    public bool left = false;
    public float sensivity;
	// Use this for initialization
	void Start ()
    {
        if (left)
            sensivity = 100;
        else
            sensivity = -100;

	}
	
	// Update is called once per frame

	void Update ()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        if (h != 0 || v != 0)
        {
            Quaternion rotate = new Quaternion(0, 0, 1 * sensivity, 1 * sensivity);
            Quaternion cam = new Quaternion(transform.position.x, transform.position.y,transform.position.z , transform.rotation.w);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, cam * rotate, 15);
        }
    }
}
