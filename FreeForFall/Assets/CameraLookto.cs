using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraLookto : MonoBehaviour {

    public Transform target;
    public float looktospeed = 100f;
    public bool istps = true;
    // Use this for initialization

    private void Update()
    {
        if ((Input.GetKeyDown(KeyCode.Joystick1Button9) || Input.GetKeyDown(KeyCode.E)))
        {
            istps = !istps;
        }
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (istps)
        {
            lookto();
        }
    }
    void lookto()
    {
        Vector3 lookto = new Vector3(target.position.x, target.position.y + 2, target.position.z);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(lookto - transform.position), looktospeed * 0.125f);
    }
}
