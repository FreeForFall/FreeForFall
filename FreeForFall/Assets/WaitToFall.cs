using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaitToFall : MonoBehaviour {
    private bool fall;
    private bool done;

    // Use this for initialization
    void Start ()
    {
        fall = false;
        done = false;
    }
    IEnumerator Wait()
    {
        if (done)
        {
            Rigidbody rb = GetComponent<Rigidbody>();
            yield return new WaitForSeconds(0.4f);
            rb.isKinematic = false;
        }
    }
    private void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.tag == "Player")
        {
            print("touched");
            done = true;
            StartCoroutine(Wait());          
        }
    }
}
