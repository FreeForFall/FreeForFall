﻿using System.Collections;
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
	public float TimeSinceLastExplosion;
	public float TimeSinceLastGrip;
	private PhotonView _pView;
	private NetworkScript _network;

	private 
	// Use this for initialization
	void Start ()
	{
		TimeSinceLastExplosion = 10f;
		TimeSinceLastGrip = 10f;
		_network = GameObject.Find ("NetworkManager").GetComponent<NetworkScript> ();
	}
	
	// Update is called once per frame
	void Update ()
	{
		TimeSinceLastExplosion += Time.deltaTime;
		TimeSinceLastGrip += Time.deltaTime;
		if (Constants.BAZOOKA_CD - TimeSinceLastExplosion <= 0)
		{
			if (Input.GetMouseButtonDown (0) || Input.GetKey (KeyCode.Joystick1Button5))
			{
				TimeSinceLastExplosion = 0;
				NetworkEventHandlers.SendEvent (new BazookaEvent (Launcher.transform.position, Launcher.transform.rotation, Camera.transform.forward * Constants.PROJECTILE_FORCE));
				_network.HandleBazooka (Launcher.transform.position, Launcher.transform.rotation, Camera.transform.forward * Constants.PROJECTILE_FORCE);
			}
		}

		if (Constants.GRIP_CD - TimeSinceLastGrip <= 0)
		{
			if (Input.GetMouseButtonDown (1) || Input.GetKey (KeyCode.Joystick1Button4))
			{
				TimeSinceLastGrip = 0;
				GameObject temp_projectile;
				temp_projectile = Instantiate (grip, Launcher.transform.position, Launcher.transform.rotation, PlayerBody.transform) as GameObject;
				Rigidbody projectile_body;
				projectile_body = temp_projectile.GetComponent<Rigidbody> ();
				projectile_body.AddForce (Camera.transform.forward * Constants.PROJECTILE_FORCE);
				Destroy (temp_projectile, 10.0f);
			}
		}
	}

	public void RefreshCooldowns ()
	{
		Debug.LogWarning ("REFRESHING");
		TimeSinceLastExplosion = Constants.BAZOOKA_CD;
		TimeSinceLastGrip = Constants.GRIP_CD;
	}
}
