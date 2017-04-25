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

	private float _timeSinceLastSerialize;
	// Use this for initialization
	void Start ()
	{
		Debug.LogError (gameObject);
		_timeSinceLastSerialize = 0.05f;
		_photonView = GetComponent<PhotonView> ();
		_bottom = gameObject.transform.Find ("bottom");
		_torso = gameObject.transform.Find ("Torso");
		Debug.LogWarning (_torso);
		_targetPosition = _bottom.position;
		_targetBottomRotation = Quaternion.Euler (0, 0, 0);
		_targetTorsoRotation = Quaternion.Euler (0, 0, 0);
	}
	
	// Update is called once per frame
	void Update ()
	{
		_timeSinceLastSerialize += Time.deltaTime;
		if (!_photonView.isMine)
		{
			_bottom.position = Vector3.Lerp (_bottom.position, _targetPosition, _timeSinceLastSerialize); // Maybe add velocity scaling
			_bottom.rotation = Quaternion.Lerp (_bottom.rotation, _targetBottomRotation, _timeSinceLastSerialize);
			_torso.rotation = Quaternion.Lerp (_torso.rotation, _targetTorsoRotation, _timeSinceLastSerialize);
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
			_timeSinceLastSerialize = 0f;
			_bottom.position = _targetPosition;
			_bottom.rotation = _targetBottomRotation;
			_torso.rotation = _targetTorsoRotation;
			_targetPosition = (Vector3)pStream.ReceiveNext ();
			_targetBottomRotation = (Quaternion)pStream.ReceiveNext ();
			_targetTorsoRotation = (Quaternion)pStream.ReceiveNext ();
			Debug.LogWarning (_targetTorsoRotation);
		}
	}
}
