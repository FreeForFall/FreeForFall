using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaitToFall : MonoBehaviour {

	private float _dropDelay;
	private float _destroyDelay;

	void Start() {
		_dropDelay = 0.2f;
		_destroyDelay = 200000000;
	}

	void Drop() {
		Rigidbody rb = GetComponent<Rigidbody>();
		rb.isKinematic = false;
		Invoke("Remove", _destroyDelay);
	}

	void Remove() {
		Destroy(gameObject);
	}

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.tag == "Player")
			Invoke("Drop", _dropDelay);
    }
}
