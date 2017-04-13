using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gripping : MonoBehaviour {

    public float PullForce;
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnCollisionEnter(Collision col)
    {
        this.transform.parent.transform.GetComponent<Rigidbody>().AddForce((this.transform.parent.transform.position - transform.position).normalized * -PullForce);
        Destroy(this.gameObject);
    }
 }
