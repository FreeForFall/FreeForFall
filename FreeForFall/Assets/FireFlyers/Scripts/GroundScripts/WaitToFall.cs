using UnityEngine;
using AssemblyCSharp;

public class WaitToFall : MonoBehaviour
{
	public bool _isFallen = false;
	private GameObject cell;
	public GameObject impactParticle;

	public void Goback ()
	{
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
		impactParticle = Instantiate (impactParticle, transform.position, Quaternion.identity);

	}

	void Drop ()
	{
		if (!_isFallen)
		{
			Rigidbody rb = GetComponent<Rigidbody> ();
			rb.isKinematic = false;
			rb.AddForce (new Vector3 (0f, -Constants.DROP_FORCE, 0f));
			_isFallen = true;
		}
	}

	void CallDrop ()
	{
		Invoke ("Drop", Constants.DROP_DELAY);
	}
}
