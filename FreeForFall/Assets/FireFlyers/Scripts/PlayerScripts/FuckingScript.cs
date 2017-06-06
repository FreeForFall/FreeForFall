using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FuckingScript : MonoBehaviour {

    public GameObject impacteffect;
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
            GameObject ImpacteffectDone = Instantiate(impacteffect, CollisionInfo.contacts[0].point, transform.rotation) as GameObject;
            this.GetComponent<Rigidbody>().velocity = Vector3.zero;
            this.GetComponent<Rigidbody>().AddForce(-transform.position * 250, ForceMode.Impulse);
            Destroy(ImpacteffectDone, 0.5f);
        }
    }
}
