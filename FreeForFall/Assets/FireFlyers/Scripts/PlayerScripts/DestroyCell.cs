using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DestroyCell : MonoBehaviour {

    public List<GameObject> FallenCells;
    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Ground")
        {
            other.gameObject.BroadcastMessage("IsFallen");
            FallenCells.Add(other.gameObject);
        }
    }
}
