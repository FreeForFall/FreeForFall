using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyCell : MonoBehaviour {

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Ground")
        {
            Destroy(other.gameObject);
        }
        else
            return;

    }
}
