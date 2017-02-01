using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

	private bool fps;
	private GameObject player;
	private Vector3 distance;
	private float horizontalOrientation;
	private float verticalOrientation;
	private Vector3 eulerAngle;

	void Start () {
		this.fps = true;
		this.player = GameObject.Find("Player");
		this.distance = new Vector3(0, -3, 15);
		this.horizontalOrientation = 0f;
		this.verticalOrientation = 0f;
		this.eulerAngle = new Vector3(0f, 0f, 0f);
	}

	private void doOrientation(){
		float rtx = Input.GetAxis("Mouse X");
		float rty = Input.GetAxis("Mouse Y");
		horizontalOrientation += Input.GetAxis("Mouse X");
		verticalOrientation -= Input.GetAxis("Mouse Y");
		this.eulerAngle.x = verticalOrientation;
		this.eulerAngle.y = horizontalOrientation;
	}

	void Update(){
		doOrientation();
		this.player.transform.eulerAngles = new Vector3(0f, horizontalOrientation, 0f);
	}

	// Update is called once per frame
	void LateUpdate () {
		if(Input.GetKeyDown(KeyCode.E)){
			this.fps = !this.fps;
		}
		if(!this.fps){
			transform.position = this.player.transform.position - this.distance;
			transform.LookAt(player.transform);
		} else {
			transform.position = this.player.transform.position;
			transform.eulerAngles = this.eulerAngle;

		}
	}
}
