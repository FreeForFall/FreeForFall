using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MakeItFall : MonoBehaviour {

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Ground")
        {
            other.gameObject.BroadcastMessage("Call");
        }
        else
            return;

    }
}
