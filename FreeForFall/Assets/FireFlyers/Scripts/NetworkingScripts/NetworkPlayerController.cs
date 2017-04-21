using UnityEngine;

public class NetworkPlayerController : MonoBehaviour
{
	private Vector3 _targetPosition;
	// Need to find a way to smooth the rotation
	private Quaternion _targetRotation;

	private PhotonView _photonView;

	private Transform _bottom;
	// Use this for initialization
	void Start ()
	{
		Debug.LogError (gameObject);
		
		_photonView = GetComponent<PhotonView> ();
		Debug.LogError ("photon view");
		Debug.Log (_photonView.isMine);
		_bottom = gameObject.transform.Find ("bottom");
		_targetPosition = _bottom.position;
		_targetRotation = Quaternion.Euler (0, 0, 0);
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (!_photonView.isMine)
		{
			_bottom.position = Vector3.Lerp (_bottom.position, _targetPosition, 0.1f); // Maybe add velocity scaling
			_bottom.rotation = _targetRotation;
		}
	}

	void OnPhotonSerializeView (PhotonStream pStream, PhotonMessageInfo pMessageInfo)
	{
		if (pStream.isWriting)
		{
			pStream.SendNext (_bottom.position);
			pStream.SendNext (_bottom.rotation);
		}
		else
		if (pStream.isReading)
		{
			_targetPosition = (Vector3)pStream.ReceiveNext ();
			_targetRotation = (Quaternion)pStream.ReceiveNext ();
		}
	}
}
