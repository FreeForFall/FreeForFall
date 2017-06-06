using System.Collections;
using System.Collections.Generic;
using AssemblyCSharp;
using UnityEngine;

public class AIShooter : MonoBehaviour {

    private float TimeSinceLastExplosion;
    public GameObject projectile ;
    public GameObject Launcher;
    public GameObject PlayerBody;
    public GameObject Canon;
    private Vector3 Direction;
    public float radius;
    private bool HasTarget;
    private GameObject Target;
    public float projectileforce;
    // Use this for initialization
    void Start () {
        TimeSinceLastExplosion = -20f;
        HasTarget = false;
	}
	
	// Update is called once per frame
	void Update ()
    {
        TimeSinceLastExplosion += Time.deltaTime;
        if (HasTarget)
        {
            Debug.LogWarning("Has a target");
            Direction = Target.transform.position - PlayerBody.transform.position;
            Canon.transform.rotation = Quaternion.Slerp(Canon.transform.rotation, Quaternion.LookRotation(Direction), 0.1f);
            Shoot();
            HasTarget = false;
        }
        else
            LockOnTarget();
    }

    private void Shoot()
    {
        if (Constants.BAZOOKA_CD - TimeSinceLastExplosion <= 0)
        {
                TimeSinceLastExplosion = 0;
                GameObject temp_projectile;
                temp_projectile = Instantiate(projectile, Launcher.transform.position, PlayerBody.transform.rotation, PlayerBody.transform) as GameObject;
                Rigidbody projectile_body;
                projectile_body = temp_projectile.GetComponent<Rigidbody>();
                projectile_body.AddForce(Direction * projectileforce);
                Destroy(temp_projectile, 10.0f);
        }
    }

    private void LockOnTarget()
    {
        Collider[] InRange = Physics.OverlapSphere(PlayerBody.transform.position, radius);
        for (int i = 0; i < InRange.Length; i++)
        {
            if (InRange[i].tag == "Player" && InRange[i].gameObject != PlayerBody.gameObject)
            {
                Debug.LogWarning("Oh boy");
                Target = InRange[i].gameObject;
                HasTarget = true;
                return;
            }
        }
    }
}
