using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookTowardCamera : MonoBehaviour
{

	public Transform playerCamera;
	public Transform bottom;
	Vector3 offset;

	// Use this for initialization
	void Start ()
	{
		offset = transform.position - bottom.position;
	}

	// Update is called once per frame
	// Update is called once per frame
	void LateUpdate ()
	{
		float rotate = 0.125f;
		transform.position = bottom.position + new Vector3 (-0.02f, 1.45f, 0);
		Quaternion cam = new Quaternion (0, playerCamera.rotation.y, 0, playerCamera.rotation.w);
		transform.rotation = Quaternion.Slerp (transform.rotation, cam, rotate);
	}
}
