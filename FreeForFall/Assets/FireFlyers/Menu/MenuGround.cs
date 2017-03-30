using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuGround : MonoBehaviour{

    public float dropDelay;
    private float _destroyDelay;

    void Start() {

        Invoke("Drop", dropDelay);
        

        
    }

    void Update()
    {
         
    }



    void Drop()
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        rb.useGravity = true;
       }

    void Remove()
    {
        Destroy(gameObject);
    }

   
}