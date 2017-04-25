using UnityEngine;


public class MakeItFall : MonoBehaviour {


    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Ground")
        {
            other.gameObject.BroadcastMessage("CallDrop");

        }
    }
}
