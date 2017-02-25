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

	void Drop()
    {
		Rigidbody rb = GetComponent<Rigidbody>();
		rb.isKinematic = false;
		Invoke("Remove", _destroyDelay);
	}

    void Call()
    {
        Invoke("Drop", _dropDelay);
    }

	void Remove() {
		Destroy(gameObject);
	}
}
