using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookTowardCamera : MonoBehaviour
{
	public Transform playerCamera;

	void LateUpdate ()
	{
		Quaternion cam = new Quaternion (0, playerCamera.rotation.y, 0, playerCamera.rotation.w);
		transform.rotation = cam;
	}
}
