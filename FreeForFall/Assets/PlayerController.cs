using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

	private float speed;
	private float sprintSpeedMultiplicator;
	private bool airbone;
	private Rigidbody rigidBody;


	// Use this for initialization
	void Start () {
		this.speed = 5.0f;
		this.sprintSpeedMultiplicator = 1;
		this.airbone = false;
		this.rigidBody = this.GetComponent<Rigidbody>();
	}

	void OnCollisionEnter(Collision c){
		this.airbone = !(c.gameObject.name == "Ground");
	}

	private void doMovement(){
		if(Input.GetKeyDown(KeyCode.Space) && !this.airbone){
			this.airbone = true;
			this.rigidBody.AddForce(new Vector3(0f, 200f, 0f));
		}
		if(Input.GetKey(KeyCode.LeftShift)){
			this.sprintSpeedMultiplicator = 1.5f;
		} else {
			this.sprintSpeedMultiplicator = 1f;
		}
		float h = Input.GetAxis("Horizontal");
		float v = Input.GetAxis("Vertical");
		transform.Translate(Vector3.right * h * Time.deltaTime * this.speed * this.sprintSpeedMultiplicator);
		transform.Translate(Vector3.forward * v * Time.deltaTime * this.speed * this.sprintSpeedMultiplicator);
	}



	
	
	// Update is called once per frame
	void Update () {
		doMovement();
	}
}
