using UnityEngine;

public class NetworkPlayerController : MonoBehaviour
{

	private Vector3 _targetPosition;
	// Need to find a way to smooth the rotation
	private Quaternion _targetRotation;

	private PhotonView _photonView;
	// Use this for initialization
	void Start ()
	{
		_targetPosition = transform.position;
		_targetRotation = Quaternion.Euler (0, 0, 0);
		_photonView = GetComponent<PhotonView> ();
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (!_photonView.isMine)
		{
			transform.position = Vector3.Lerp (transform.position, _targetPosition, 0.1f); // Maybe add velocity scaling
			transform.rotation = _targetRotation;
		}
	}

	void OnPhotonSerializeView (PhotonStream pStream, PhotonMessageInfo pMessageInfo)
	{
		if (pStream.isWriting)
		{
			pStream.SendNext (transform.position);
			pStream.SendNext (transform.rotation);
		}
		else if (pStream.isReading)
		{
			_targetPosition = (Vector3)pStream.ReceiveNext ();
			_targetRotation = (Quaternion)pStream.ReceiveNext ();
		}
	}
}
