using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

	public bool fps;
	private GameObject player;
	private Vector3 distance;
	private float horizontalOrientation;
	private float verticalOrientation;
	private Vector3 eulerAngle;
	private float rotationAroundPlayer;
	public float sensibility;

	void Start () {
		this.player = transform.parent.gameObject;
		this.distance = new Vector3(0, -3, 15);
		this.horizontalOrientation = 0f;
		this.verticalOrientation = 0f;
		this.eulerAngle = new Vector3(0f, 0f, 0f);
		this.rotationAroundPlayer = 0f;
		if(sensibility == 0)
			sensibility = 1;

		// To start in fps, remove those two lines
		if(!fps){ 
			transform.position = getPositionBehind(this.player, 15, 3);
			transform.LookAt(this.player.transform);
		}
	}

	private void doOrientation(){
		horizontalOrientation += Input.GetAxis("Mouse X") * sensibility;
		verticalOrientation -= Input.GetAxis("Mouse Y") * sensibility;
		this.eulerAngle.x = verticalOrientation;
		this.eulerAngle.y = horizontalOrientation;
	}

	void Update(){
		doOrientation();
	}

	private Vector3 getPositionBehind(GameObject o, int behind, int above){
		return o.transform.position -  (this.player.transform.forward * behind) + (this.player.transform.up * above);
	}

	// Update is called once per frame
	void LateUpdate () {
		this.player.transform.eulerAngles = new Vector3(0f, horizontalOrientation, 0f);
		if(Input.GetKeyDown(KeyCode.E ) || Input.GetKeyDown(KeyCode.Joystick1Button9))
        {
			if(fps){
				// this updates the position to be behind the player
				transform.position = getPositionBehind(this.player, 15, 3);
				transform.LookAt(this.player.transform);
			}
			this.rotationAroundPlayer = 0f;
			this.fps = !this.fps;
		}
		if(!this.fps){
			this.rotationAroundPlayer += Input.GetAxis("Mouse ScrollWheel") * 1000;
			this.transform.RotateAround(this.player.transform.position, Vector3.up, rotationAroundPlayer * Time.deltaTime);
			transform.LookAt(this.player.transform);
		} else {
			transform.position = this.player.transform.position + Vector3.up;
			transform.eulerAngles = this.eulerAngle;
		}
	}
}
