using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestImperedvision : MonoBehaviour
{
    public GameObject playecam;

    void OnTriggerEnter(Collider c)
    {

        if (c.gameObject.tag == "Player")
        {
            playecam.GetComponent<CameraFilterPack_FX_Glitch1>().enabled = true;
            Invoke("Stop", 5f);
        }        
    }

    void Stop()
    {
        playecam.GetComponent<CameraFilterPack_FX_Glitch1>().enabled = false;
    }
}


