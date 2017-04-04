using UnityEngine;

public class WaitToFall : MonoBehaviour {

	private float _dropDelay;
	private float _destroyDelay;
    private float _dropforce;
    private Vector3 _initialPosition;
    private Quaternion _initialRotation;
    public bool _isFallen = false;
    private GameObject cell;
    public float speed = 5;
    public float rotateSpeed = 360;

    void Start() {
		_dropDelay = 0.2f;
		_destroyDelay = 20000000000000;
        _dropforce = 300f;
        _initialPosition = transform.position;
        _initialRotation = transform.rotation;
    }

    void Update()
    {
        if (_isFallen)
        {
            Rigidbody rb = GetComponent<Rigidbody>();
            float step = speed * Time.deltaTime;
            float rotate = rotateSpeed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, _initialPosition, step);
            rb.rotation = Quaternion.RotateTowards(transform.rotation, _initialRotation, rotate);
            if (transform.position == _initialPosition)
            {
                _isFallen = false;
            }
        }

    }

    void IsFallen()
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        rb.isKinematic = true;
        _isFallen = true;
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
}
