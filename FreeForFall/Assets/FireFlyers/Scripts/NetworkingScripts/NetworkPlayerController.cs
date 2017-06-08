using UnityEngine;
using AssemblyCSharp;

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
    void Start()
    {		
        _photonView = GetComponent<PhotonView>();
        _bottom = gameObject.transform.Find("bottom");
        _torso = gameObject.transform.Find("Torso");
        _targetPosition = _bottom.position;
        _targetBottomRotation = Quaternion.Euler(0, 0, 0);
        _targetTorsoRotation = Quaternion.Euler(0, 0, 0);
    }
	
    // Update is called once per frame
    void Update()
    {
        if (!_photonView.isMine)
        {
            if (Vector3.Distance(_bottom.position, _targetPosition) > 6f)
            {
                _bottom.position = _targetPosition;
            }
            else
            {
                _bottom.position = Vector3.Lerp(_bottom.position, _targetPosition, Constants.NETWORK_SMOOTHING);
            }
            _bottom.rotation = Quaternion.Lerp(_bottom.rotation, _targetBottomRotation, Constants.NETWORK_SMOOTHING);
            _torso.rotation = Quaternion.Lerp(_torso.rotation, _targetTorsoRotation, Constants.NETWORK_SMOOTHING);
        }
    }

    void OnPhotonSerializeView(PhotonStream pStream, PhotonMessageInfo pMessageInfo)
    {
        if (pStream.isWriting)
        {
            pStream.SendNext(_bottom.position);
            pStream.SendNext(_bottom.rotation);
            pStream.SendNext(_torso.rotation);
        }
        else if (pStream.isReading)
        {
            _targetPosition = (Vector3)pStream.ReceiveNext();
            _targetBottomRotation = (Quaternion)pStream.ReceiveNext();
            _targetTorsoRotation = (Quaternion)pStream.ReceiveNext();
        }
    }
}
