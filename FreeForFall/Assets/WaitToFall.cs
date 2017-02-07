using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaitToFall : MonoBehaviour {

	public float Delay = 0.4f;


	void Drop(){
		Rigidbody rb = GetComponent<Rigidbody>();
		rb.isKinematic = false;
		Invoke("Remove", 3f);
	}

	void Remove(){
		Destroy(gameObject);
	}

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
			Invoke("Drop", 0.4f);
    }
}
