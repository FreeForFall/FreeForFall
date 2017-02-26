using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaitToFall : MonoBehaviour {

	private float _dropDelay;
	private float _destroyDelay;
    private float _dropforce;
    private GameObject cell;

	void Start() {
		_dropDelay = 0.2f;
		_destroyDelay = 20000000000000;
        _dropforce = 300f;

    }

	void Drop()
    {
		Rigidbody rb = GetComponent<Rigidbody>();
		rb.isKinematic = false;
        rb.AddForce(new Vector3(0f, -_dropforce, 0f));
        Invoke("Remove", _destroyDelay);
	}

    void Call()
    {
        Invoke("Drop", _dropDelay);
    }

	void Remove()
    {
        Destroy(gameObject);
	}
}
