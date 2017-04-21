using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WheelAnimation : MonoBehaviour
{

	public bool left = false;
	public float sensivity;
	// Use this for initialization
	void Start ()
	{
		sensivity = -200;
	}
	
	// Update is called once per frame

	void Update ()
	{
		float h = Input.GetAxis ("Horizontal");
		float v = Input.GetAxis ("Vertical");
		if (h != 0 || v != 0)
		{
			transform.Rotate (Vector3.left * Time.deltaTime * sensivity, Space.Self);
		}
	}
}
