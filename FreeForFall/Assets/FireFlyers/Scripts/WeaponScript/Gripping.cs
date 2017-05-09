using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AssemblyCSharp;

public class Gripping : MonoBehaviour
{
	public GameObject impactParticle;
	public Vector3 impactNormal;
	// Use this for initialization
	void Start ()
	{
		
	}
	
	// Update is called once per frame
	void Update ()
	{
		
	}

	void OnCollisionEnter (Collision col)
	{
		// impactParticle = Instantiate(impactParticle, transform.position, Quaternion.FromToRotation(Vector3.up, impactNormal)) as GameObject;
		this.transform.parent.transform.GetComponent<Rigidbody> ().AddForce ((this.transform.parent.transform.position - transform.position).normalized * -Constants.GRIP_PULL_FORCE);
		Destroy (this.gameObject);
	}
}
