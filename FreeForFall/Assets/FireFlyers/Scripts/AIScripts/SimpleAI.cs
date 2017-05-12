using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleAI : MonoBehaviour {

    public float sensorLength = 2f;
    public float speed = 30f;
    public float turnspeed = 1000f;
    private int turnValue = 0;
    private System.Random rng;
    public const float JUMP_FORCE = 150000f; // to remove
    private bool airbone;
    private bool jumping;

    // Use this for initialization
    void Start () {
        this.airbone = false;
        this.jumping = false;
	}
	
	// Update is called once per frame
	void Update () {
        air();
        if(!airbone)
        {
            Mouvement();
            if (Input.GetKeyDown(KeyCode.Space))
                jump();
        }
        if (jumping)
            transform.position += transform.forward * speed * Time.deltaTime;

    }

    void OnDrawGizmos ()
    {
        Gizmos.DrawRay(transform.position + 2 * transform.forward + transform.up, (transform.forward + -transform.up) *(sensorLength + transform.localScale.z));
        Gizmos.DrawRay(transform.position + 2 * transform.forward + transform.up, (-transform.up + transform.right + transform.forward) *(sensorLength + transform.localScale.z));
        Gizmos.DrawRay(transform.position + 2 * transform.forward + transform.up, (-transform.up + -transform.right + transform.forward) *(sensorLength + transform.localScale.z));
        Gizmos.DrawRay(transform.position - transform.up * 1f, (-transform.up) * (1));
        Gizmos.DrawRay(transform.position - transform.up * 1f + transform.right, (-transform.up) * (1));
        Gizmos.DrawRay(transform.position - transform.up * 1f + -transform.right, (-transform.up) * (1));
    }

    void Mouvement()
    {
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

    private void jump()
    {
        jumping = true;
        GetComponent<Rigidbody>().AddForce(new Vector3(0f,  JUMP_FORCE, 0f));
        Invoke("jumpDown", 0.3f);

    }

    private void jumpDown()
    {
        GetComponent<Rigidbody>().AddForce(new Vector3(0f, -JUMP_FORCE * 1.5f, 0f));
        Invoke("canceljump", 0.3f);
    }

    private void air()
    {
        RaycastHit hit;
        if (!Physics.Raycast(transform.position - transform.up * 1f, -transform.up, out hit, (0.5f))
            && !Physics.Raycast(transform.position - transform.up * 1f + -transform.right, -transform.up, out hit, (0.5f))
            && !Physics.Raycast(transform.position - transform.up * 1f + transform.right, -transform.up, out hit, (0.5f))) // add more raycasts
        {
            airbone = true;
        }
        else
        {
            airbone = false;
        }
    }

    private void canceljump()
    {
        jumping = false;
    }
}
