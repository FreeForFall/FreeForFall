using UnityEngine;

public class WaitToFall : MonoBehaviour {

	private float _dropDelay;
	private float _destroyDelay;
    private float _dropforce;
    private Vector3 _initialPosition;
    private Quaternion _initialRotation;
    private bool _isFallen = false;
    private GameObject cell;

	void Start() {
		_dropDelay = 0.2f;
		_destroyDelay = 20000000000000;
        _dropforce = 300f;
        Rigidbody rb = GetComponent<Rigidbody>();
        _initialPosition = transform.position;
        _initialRotation = transform.rotation;
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
        transform.position = Vector3.Lerp(transform.position, _initialPosition, 100f );
        transform.rotation = Quaternion.Lerp(transform.rotation, _initialRotation, 100f);
        if (rb.transform.position == _initialPosition)
        {
            rb.isKinematic = true;
            _isFallen = false;
        }
    }
}
