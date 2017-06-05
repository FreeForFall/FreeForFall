using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FuckingScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnCollisionEnter(Collision CollisionInfo)
    {
        if (CollisionInfo.collider.tag == "forcefield")
        {
            this.GetComponent<Rigidbody>().AddForce(-transform.position * 50, ForceMode.Impulse);
        }
    }
}
