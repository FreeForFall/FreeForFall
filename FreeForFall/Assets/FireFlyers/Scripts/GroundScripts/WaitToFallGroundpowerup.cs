using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaitToFallGroundpowerup : MonoBehaviour
{

    private float _dropDelay = 0.3f;
    private float _destroyDelay;
    private float _dropforce;
    public bool _isFallen = false;
    private GameObject cell;


    public void Goback()
    {
        print("Goback");
        Renderer Ren = GetComponent<Renderer>();
        MeshCollider Mcol = GetComponent<MeshCollider>();
        Ren.enabled = true;
        GetComponent<ReturnToPosition>().enabled = true;
        Mcol.enabled = true;

    }

    void IsFallen()
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        MeshCollider Mcol = GetComponent<MeshCollider>();
        Mcol.enabled = false;
        rb.isKinematic = true;
        _isFallen = true;
        Invoke("Goback", 0.1f);
    }

    void Drop()
    {
        if (!_isFallen)
        {
            Rigidbody rb = GetComponent<Rigidbody>();
            rb.isKinematic = false;
            rb.AddForce(new Vector3(0f, -_dropforce, 0f));
            _isFallen = true;
            Invoke("IsFallen", 4f);
        }
    }

    void CallDrop()
    {
        Invoke("Drop", _dropDelay);
    }
}


