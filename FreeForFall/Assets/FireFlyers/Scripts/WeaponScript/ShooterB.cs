using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShooterB : MonoBehaviour {
    public GameObject Launcher;
    public GameObject projectile;
    public GameObject grip;
    public GameObject Camera;
    public GameObject PlayerBody;
    public float projectile_force;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButtonDown(0)||Input.GetKey(KeyCode.Joystick1Button5))
        {
            GameObject temp_projectile;
            temp_projectile = Instantiate(projectile, Launcher.transform.position, Launcher.transform.rotation) as GameObject;
            Rigidbody projectile_body;
            projectile_body = temp_projectile.GetComponent<Rigidbody>();
            projectile_body.AddForce( Camera.transform.forward * projectile_force);
            Destroy(temp_projectile, 10.0f);
        }

        if (Input.GetMouseButtonDown(1)|| Input.GetKey(KeyCode.Joystick1Button4))
        {
            GameObject temp_projectile;
            temp_projectile = Instantiate(grip, Launcher.transform.position, Launcher.transform.rotation, PlayerBody.transform) as GameObject;
            Rigidbody projectile_body;
            projectile_body = temp_projectile.GetComponent<Rigidbody>();
            projectile_body.AddForce(Camera.transform.forward * projectile_force);
            Destroy(temp_projectile, 10.0f);
        }
	}
}
