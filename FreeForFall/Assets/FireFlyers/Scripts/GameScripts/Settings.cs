using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Settings : MonoBehaviour
{

	public bool OnlineMode;

	public float Sensitivity;

	public bool FpsMode;
	
	// Use this for initialization
	void Start ()
	{
		if (GameObject.FindGameObjectsWithTag ("Settings").Length > 1)
		{
			Destroy (this);
			return;
		}
		OnlineMode = false;
		Sensitivity = 1f;
		FpsMode = false;
		Debug.Log (OnlineMode);
		DontDestroyOnLoad (transform.gameObject);
	}

	public void PlayOnline ()
	{
		Debug.Log ("GONNA PLAY ONLINE");
		OnlineMode = true;
	}
}
