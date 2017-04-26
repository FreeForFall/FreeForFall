using UnityEngine;

public class WaitToFall : MonoBehaviour
{

	private float _dropDelay;
	private float _destroyDelay;
	private float _dropforce;
	public bool _isFallen = false;
	private GameObject cell;
	public float speed = 5;
	public float rotateSpeed = 360;
    public GameObject impactParticle;
    public Vector3 impactNormal;

    void Start ()
	{
		_dropDelay = 0.2f;
		_destroyDelay = 20000000000000;
		_dropforce = 300f;
	}


	public void Goback ()
	{
		Renderer Ren = GetComponent<Renderer> ();
		MeshCollider Mcol = GetComponent<MeshCollider> ();
		Mcol.enabled = true;
        Mcol.enabled = true;
        GetComponent<ReturnToPosition> ().enabled = true;
 
	}

	void IsFallen ()
	{
		Rigidbody rb = GetComponent<Rigidbody> ();
		Renderer Ren = GetComponent<Renderer> ();
		MeshCollider Mcol = GetComponent<MeshCollider> ();
		Mcol.enabled = false;
		Ren.enabled = false;
		rb.isKinematic = true;
		_isFallen = true;
        impactParticle = Instantiate(impactParticle, transform.position, Quaternion.FromToRotation(Vector3.up, impactNormal)) as GameObject;

    }

	void Drop ()
	{
		if (!_isFallen)
		{
			Rigidbody rb = GetComponent<Rigidbody> ();
			rb.isKinematic = false;
			rb.AddForce (new Vector3 (0f, -_dropforce, 0f));
			_isFallen = true;
		}
	}

	void CallDrop ()
	{
		Invoke ("Drop", _dropDelay);
	}
}
