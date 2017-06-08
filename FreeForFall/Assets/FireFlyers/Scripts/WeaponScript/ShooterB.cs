using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AssemblyCSharp;


public class ShooterB : MonoBehaviour
{
    public GameObject Launcher;
    public GameObject projectile;
    public GameObject grip;
    public GameObject Camera;
    public GameObject PlayerBody;
    public GameObject thing;
    public GameObject RefreshParticleNonLocal;
    public GameObject RefreshParticleLocal;
    public float TimeSinceLastExplosion;
    public float TimeSinceLastGrip;
    private PhotonView _pView;
    private Networking _network;

    private 
	// Use this for initialization
	void Start()
    {
        TimeSinceLastExplosion = 10f;
        TimeSinceLastGrip = 10f;
        _network = GameObject.Find("NetworkManager").GetComponent<Networking>();
    }
	
    // Update is called once per frame
    void Update()
    {
        TimeSinceLastExplosion += Time.deltaTime;
        TimeSinceLastGrip += Time.deltaTime;
        if (Constants.BAZOOKA_CD - TimeSinceLastExplosion <= 0)
        {
            if (Input.GetMouseButtonDown(0) || Input.GetKey(KeyCode.Joystick1Button5))
            {
                TimeSinceLastExplosion = 0;
                //NetworkEventHandlers.SendEvent(new BazookaEvent(Launcher.transform.position, Launcher.transform.rotation, Camera.transform.forward * Constants.PROJECTILE_FORCE));
                NetworkEventHandlers.Broadcast(Constants.EVENT_IDS.BAZOOKA_SHOT, new object[]
                    {
                        (object)Launcher.transform.position,
                        (object)Launcher.transform.rotation,
                        (object)(Camera.transform.forward * Constants.PROJECTILE_FORCE)
                    });
                _network.Engine.BazookaShoot(Launcher.transform.position, Launcher.transform.rotation, Camera.transform.forward * Constants.PROJECTILE_FORCE);
            }
        }

        if (Constants.GRIP_CD - TimeSinceLastGrip <= 0)
        {
            if (Input.GetMouseButtonDown(1) || Input.GetKey(KeyCode.Joystick1Button4))
            {
                TimeSinceLastGrip = 0;
                GameObject temp_projectile;
                temp_projectile = Instantiate(grip, Launcher.transform.position, Launcher.transform.rotation, PlayerBody.transform) as GameObject;
                Rigidbody projectile_body;
                projectile_body = temp_projectile.GetComponent<Rigidbody>();
                projectile_body.AddForce(Camera.transform.forward * Constants.PROJECTILE_FORCE);
                Destroy(temp_projectile, 10.0f);
            }
        }
    }

    public void RefreshCooldowns()
    {
        NetworkEventHandlers.Broadcast(Constants.EVENT_IDS.COOLDOWN_REFRESH_PARTICLES, transform.position);
        Debug.LogWarning("REFRESHING");
        RefreshCooldownsParticles(transform.FindChild("bottom").transform.position, true);
        TimeSinceLastExplosion = Constants.BAZOOKA_CD;
        TimeSinceLastGrip = Constants.GRIP_CD;
    }

    public void RefreshCooldownsParticles(Vector3 position, bool local = false)
    {
        GameObject particle;
        if (local)
        {
            particle = Instantiate(RefreshParticleLocal, position, Quaternion.identity, transform.FindChild("bottom")) as GameObject;
            Debug.LogWarning("Particle Rfresh");
        }
        else
        {
            particle = Instantiate(RefreshParticleNonLocal, position, Quaternion.identity) as GameObject;
            Debug.LogWarning("Particle Rfresh");
        }

        Destroy(particle, 1f);
    }
}
