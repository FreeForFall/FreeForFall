using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosives : MonoBehaviour
{
	public float radius;
	public float power;
	public GameObject impactParticle;
	public Vector3 impactNormal;
	private float explosionPhysics = 15f;

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
				colliders [i].GetComponent<Rigidbody> ().AddExplosionForce (power, explosion_epicenter, radius, explosionPhysics);
			}
		}
	}
}
