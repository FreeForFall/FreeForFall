using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AssemblyCSharp;

public class Explosives : MonoBehaviour
{
	public float radius;
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

	// Called when the object collides
	void OnCollisionEnter (Collision collision)
	{
		impactParticle = Instantiate (impactParticle, transform.position, Quaternion.FromToRotation (Vector3.up, impactNormal)) as GameObject;
		Vector3 explosion_epicenter = transform.position;
		Collider[] colliders = Physics.OverlapSphere (explosion_epicenter, radius);
		for (int i = 0; i < colliders.Length; i++)
		{
			if (colliders [i].GetComponent<Rigidbody> ())
			{
				colliders [i].GetComponent<Rigidbody> ().AddExplosionForce (Constants.BAZOOKA_EXPLOSION_FORCE, explosion_epicenter, radius, 1f);
			}
		}
	}
}
