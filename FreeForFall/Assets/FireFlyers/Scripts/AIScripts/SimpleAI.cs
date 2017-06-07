using System.Collections;
using System.Collections.Generic;
using AssemblyCSharp;
using UnityEngine;

public class SimpleAI : MonoBehaviour
{

    public AudioSource jumpsound;
    public AudioSource Fallsound;
    public GameObject jumpeffect;
    public Transform jumpflamelocation;
    public float sensorLength = 2f;
    public float speed = 30f;
    public float turnspeed = 1000f;
    private int turnValue = 0;
    private System.Random rng;
    private float Timesincelastjump;
    private bool airbone;
    private bool jumping;
    private bool playfrogs;
    private string _name;

    public string Name
    {
        get
        {
            return _name;
        }
        set
        {
            _name = value;
        }
    }

    // Use this for initialization
    void Start()
    {
        Timesincelastjump = 3;
        turnValue = 1;
        this.playfrogs = false;
        Invoke("autisticfrogs", 10f);
        this.airbone = false;
        this.jumping = false;
    }
	
    // Update is called once per frame
    void Update()
    {
        Timesincelastjump += Time.deltaTime;
        air();
        if (!airbone)
        {
            Mouvement();
        }
        if (jumping)
        {
            transform.position += transform.forward * speed * Time.deltaTime;
        }

    }

    void OnDrawGizmos()
    {
        Gizmos.DrawRay(transform.position + 0.5f * transform.forward + transform.up, (transform.forward + -transform.up) * (sensorLength + transform.localScale.z));
        Gizmos.DrawRay(transform.position + 0.5f * transform.forward + transform.up, (-transform.up + transform.right) * (sensorLength + transform.localScale.z));
        Gizmos.DrawRay(transform.position + 0.5f * transform.forward + transform.up, (-transform.up + -transform.right) * (sensorLength + transform.localScale.z));
        Gizmos.DrawRay(transform.position - transform.up * 1f + -transform.right + transform.forward, (-transform.up) * (1));
        Gizmos.DrawRay(transform.position - transform.up * 1f + transform.right + transform.forward, (-transform.up) * (1));
        Gizmos.DrawRay(transform.position - transform.up * 1f + -transform.right + -transform.forward, (-transform.up) * (1));
        Gizmos.DrawRay(transform.position - transform.up * 1f + transform.right + -transform.forward, (-transform.up) * (1));
        Gizmos.DrawRay(transform.position - transform.up * 1f + transform.forward * 8, (-transform.up) * (1));
    }

    void Mouvement()
    {
        RaycastHit hit;
        int flag = 0;
        if (!Physics.Raycast(transform.position + 0.5f * transform.forward + transform.up, transform.forward + -transform.up, out hit, (sensorLength + transform.localScale.z)))
        {
            if (Physics.Raycast(transform.position + 1 * transform.forward * 8, -transform.up, out hit, 3) && Constants.JUMP_CD_AI < Timesincelastjump)
            {
                jump();
            }
            else
            {
                if (!Physics.Raycast(transform.position + 0.5f * transform.forward + transform.up, -transform.up + transform.right, out hit, (sensorLength + transform.localScale.z)))
                {
                    turnValue -= 1;
                    flag++;
                }
                else if (!Physics.Raycast(transform.position + 1.5f * transform.forward + transform.up, transform.forward + -transform.up + -transform.right, out hit, (sensorLength + transform.localScale.z)))
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
            
        }
        if (flag == 0)
            turnValue = 0;
        transform.Rotate(Vector3.up * (turnspeed * turnValue) * Time.deltaTime);
        transform.position += transform.forward * speed * Time.deltaTime;
    }

    private void jump()
    {
        jumping = true;
        GameObject JumpeffectDone = Instantiate(jumpeffect, jumpflamelocation.transform.position, jumpflamelocation.transform.rotation) as GameObject;
        jumpsound.Play();
        Timesincelastjump = 0;
        GetComponent<Rigidbody>().AddForce(new Vector3(0f, Constants.JUMP_FORCE, 0f));
        Destroy(JumpeffectDone, 0.5f);
        Invoke("jumpDown", 0.5f);

    }

    private void jumpDown()
    {
        GetComponent<Rigidbody>().AddForce(new Vector3(0f, -Constants.JUMP_FORCE * 1.5f, 0f)); 
        Invoke("canceljump", 0.3f);
    }

    private void air()
    {
        RaycastHit hit;
        if (!Physics.Raycast(transform.position - transform.up * 1f + transform.right + transform.forward, -transform.up, out hit, (0.5f))
            && !Physics.Raycast(transform.position - transform.up * 1f + -transform.right + transform.forward, -transform.up, out hit, (0.5f))
            && !Physics.Raycast(transform.position - transform.up * 1f + transform.right - transform.forward, -transform.up, out hit, (0.5f))
            && !Physics.Raycast(transform.position - transform.up * 1f + -transform.right - transform.forward, -transform.up, out hit, (0.5f)))
        {
            airbone = true;
            if (playfrogs)
            {
                Fallsound.Play();
            }

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

    private void autisticfrogs()
    {
        playfrogs = false;
    }
}
