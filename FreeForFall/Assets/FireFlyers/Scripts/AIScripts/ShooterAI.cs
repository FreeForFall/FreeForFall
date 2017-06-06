using System.Collections;
using System.Collections.Generic;
using AssemblyCSharp;
using UnityEngine;

public class ShooterAI : MonoBehaviour {

    public GameObject Launcher;
    public GameObject Cannon;
    public GameObject Projectile;
    private float TimeSinceLastExplosion;
    private GameObject Target;
    private Vector3 _Aim;
    private bool _target;
    // Use this for initialization
    void Start () {
        TimeSinceLastExplosion = 0f;
        _target = false;
    }
	
	// Update is called once per frame
	void Update () {
        TimeSinceLastExplosion += Time.deltaTime;
        Aim();
        _Aim = Target.transform.position - this.transform.position;
        CannonRotation();
        Shoot();

    }

    void Shoot()
    {
        if (Constants.BAZOOKA_CD - TimeSinceLastExplosion <= 0)
        {
            if (_target)
            {
                TimeSinceLastExplosion = 0;
                GameObject temp_projectile;
                temp_projectile = Instantiate(Projectile, Launcher.transform.position, Launcher.transform.rotation, this.transform) as GameObject;
                Rigidbody projectile_body;
                projectile_body = temp_projectile.GetComponent<Rigidbody>();
                projectile_body.AddForce(_Aim * Constants.PROJECTILE_FORCE);
                Destroy(temp_projectile, 10.0f);
                _target = false;
            }
        }
    }

    void Aim()
    {
        if (!_target)
        {
            Collider[] colliders = Physics.OverlapSphere(this.transform.position, Constants.AIM_RADIUS_AI);
            for (int i = 0; i < colliders.Length; i++)
            {
                if (colliders[i].tag == "Player")
                {
                    Target = colliders[i].gameObject;
                    _target = true;
                    return;
                }
            }
        }
    }

    void CannonRotation ()
    {
        Cannon.transform.rotation = Quaternion.Slerp(Cannon.transform.rotation, Quaternion.LookRotation(_Aim),0.1f);
    }
}
