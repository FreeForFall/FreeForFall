using UnityEngine;

public class NetworkPlayerController : MonoBehaviour
{
	private Vector3 _targetPosition;
	// Need to find a way to smooth the rotation
	private Quaternion _targetBottomRotation;

	private Quaternion _targetTorsoRotation;

	private PhotonView _photonView;

	private Transform _bottom;

	private Transform _torso;
	// Use this for initialization
	void Start ()
	{
		Debug.LogError (gameObject);
		
		_photonView = GetComponent<PhotonView> ();
		_bottom = gameObject.transform.Find ("bottom");
		_torso = gameObject.transform.Find ("Torso");
		_targetPosition = _bottom.position;
		_targetBottomRotation = Quaternion.Euler (0, 0, 0);
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (!_photonView.isMine)
		{
			_bottom.position = Vector3.Lerp (_bottom.position, _targetPosition, 0.1f); // Maybe add velocity scaling
			_bottom.rotation = Quaternion.Lerp (_bottom.rotation, _targetBottomRotation, 0.1f);
			_torso.rotation = Quaternion.Lerp (_torso.rotation, _targetTorsoRotation, 0.1f);
		}
	}

	void OnPhotonSerializeView (PhotonStream pStream, PhotonMessageInfo pMessageInfo)
	{
		if (pStream.isWriting)
		{
			pStream.SendNext (_bottom.position);
			pStream.SendNext (_bottom.rotation);
			pStream.SendNext (_torso.rotation);
		}
		else
		if (pStream.isReading)
		{
			_targetPosition = (Vector3)pStream.ReceiveNext ();
			_targetBottomRotation = (Quaternion)pStream.ReceiveNext ();
			_targetTorsoRotation = (Quaternion)pStream.ReceiveNext ();
		}
	}
}
