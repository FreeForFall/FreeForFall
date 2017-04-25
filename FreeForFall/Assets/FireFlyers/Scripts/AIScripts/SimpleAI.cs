using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleAI : MonoBehaviour {

    public float sensorLength = 2f;
    public float speed = 30f;
    public float turnspeed = 1000f;
    private int turnValue = 0;
    private System.Random rng;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        RaycastHit hit;
        int flag = 0;
        if (!Physics.Raycast(transform.position + 2 * transform.forward + transform.up, transform.forward + -transform.up, out hit, (sensorLength + transform.localScale.z)))
        {
            if (!Physics.Raycast(transform.position + 2 * transform.forward + transform.up, transform.forward + -transform.up + transform.right, out hit, (sensorLength + transform.localScale.z)))
            {
                turnValue -= 1;
                flag++;
            }
            else if (!Physics.Raycast(transform.position + 2 * transform.forward + transform.up, transform.forward + -transform.up + -transform.right, out hit, (sensorLength + transform.localScale.z)))
            {
                turnValue += 1;
                flag++;
            }
            else
            {
                var x = Random.Range(-1, 2);
                turnValue += x;
                flag++;
            }
        }
        if (flag == 0)
            turnValue = 0;
        transform.Rotate(Vector3.up * (turnspeed * turnValue) * Time.deltaTime);
        transform.position += transform.forward * speed * Time.deltaTime;
    }

    void OnDrawGizmos ()
    {
        Gizmos.DrawRay(transform.position + 2 * transform.forward + transform.up, (transform.forward + -transform.up) *(sensorLength + transform.localScale.z));
        Gizmos.DrawRay(transform.position + 2 * transform.forward + transform.up, (-transform.up + transform.right + transform.forward) *(sensorLength + transform.localScale.z));
        Gizmos.DrawRay(transform.position + 2 * transform.forward + transform.up, (-transform.up + -transform.right + transform.forward) *(sensorLength + transform.localScale.z));
    }
}
