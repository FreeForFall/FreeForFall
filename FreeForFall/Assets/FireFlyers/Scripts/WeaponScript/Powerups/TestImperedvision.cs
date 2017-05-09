using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AssemblyCSharp;

public class TestImperedvision : MonoBehaviour
{
	public GameObject playecam;

	void OnTriggerEnter (Collider c)
	{

		if (c.gameObject.tag == "Player")
		{
			playecam.GetComponent<CameraFilterPack_FX_Glitch1> ().enabled = true;
			Invoke ("Stop", Constants.VISION_IMPAIRED_POWERUP_DURATION);
		}        
	}

	void Stop ()
	{
		playecam.GetComponent<CameraFilterPack_FX_Glitch1> ().enabled = false;
	}
}


