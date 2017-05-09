using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReturnToPosition : MonoBehaviour
{

	private Vector3 _initialPosition;
	private Quaternion _initialRotation;


	// Use this for initialization
	void Start ()
	{
		_initialPosition = transform.position;
		_initialRotation = transform.rotation;
		GetComponent<ReturnToPosition> ().enabled = false;
	}

	void Update ()
	{
		Rigidbody rb = GetComponent<Rigidbody> ();
		float step = 20 * Time.deltaTime;
		float rotate = 360 * Time.deltaTime;
		transform.position = Vector3.MoveTowards (transform.position, _initialPosition, step);
		rb.rotation = Quaternion.RotateTowards (transform.rotation, _initialRotation, rotate);
		if (transform.position == _initialPosition)
		{
			GetComponent<ReturnToPosition> ().enabled = false;
		}
	}




}
