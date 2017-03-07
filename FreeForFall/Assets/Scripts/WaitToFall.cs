using UnityEngine;

public class WaitToFall : MonoBehaviour {

	private float _dropDelay;
	private float _destroyDelay;
    private float _dropforce;
    private float _x;
    private float _y;
    private float _z;
    private float _rx;
    private float _ry;
    private float _rz;
    private GameObject cell;

	void Start() {
		_dropDelay = 0.2f;
		_destroyDelay = 20000000000000;
        _dropforce = 300f;
        Rigidbody rb = GetComponent<Rigidbody>();
        _x = rb.position.x;
        _y = rb.position.y;
        _z = rb.position.z;
        _rx = rb.rotation.x;
        _ry = rb.rotation.y;
        _rz = rb.rotation.z;
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

	void Replace()
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        rb.position.x = _x;
        _y = rb.position.y;
        _z = rb.position.z;
        _rx = rb.rotation.x;
        _ry = rb.rotation.y;
        _rz = rb.rotation.z;
    }
}
