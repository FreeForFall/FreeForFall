using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

	private bool fps;
	private GameObject player;
	private Vector3 distance;

	void Start () {
		this.fps = true;
		this.player = GameObject.Find("Player");
		this.distance = new Vector3(0, -3, 15);
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
		}

	}
}
