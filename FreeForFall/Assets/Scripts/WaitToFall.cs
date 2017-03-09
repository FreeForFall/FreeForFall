using UnityEngine;

public class WaitToFall : MonoBehaviour {

	private float _dropDelay;
	private float _destroyDelay;
    private float _dropforce;
    private Vector3 _initialPosition;
    private float _rx;
    private float _ry;
    private float _rz;
    private bool _isFallen = false;
    private GameObject cell;

	void Start() {
		_dropDelay = 0.2f;
		_destroyDelay = 20000000000000;
        _dropforce = 300f;
        Rigidbody rb = GetComponent<Rigidbody>();
        _initialPosition = transform.position;
        _rx = rb.rotation.x;
        _ry = rb.rotation.y;
        _rz = rb.rotation.z;
    }

	void Drop()
    {
        if (!_isFallen)
        {
            Rigidbody rb = GetComponent<Rigidbody>();
            rb.isKinematic = false;
            rb.AddForce(new Vector3(0f, -_dropforce, 0f));
            _isFallen = true;
        }
	}

    void CallDrop()
    {
        Invoke("Drop", _dropDelay);
    }

    void Replace()
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        rb.transform.position = Vector3.Lerp(transform.position, _initialPosition, 1);
        if (rb.transform.position == _initialPosition)
        {
            rb.isKinematic = true;
        }
    }
}
