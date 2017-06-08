﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AssemblyCSharp;
using UnityEngine.UI;

public class SpeedBoost : MonoBehaviour
{
	public GameObject LocalPlayer;
    public PowerUpsHud playerscript;
    void Start()
    {
        gameObject.GetComponent<ParticleSystem>().startColor = Color.red;
    }

	void OnTriggerEnter (Collider c)
	{
		Debug.LogWarning ("Something collided with the powerup");
        playerscript = c.GetComponent<PowerUpsHud>();
        if (c.gameObject.tag == "Player" && !playerscript.OnePowerUp)
		{
			GameObject g;
			if (c.name == "bottom")
			{
				g = c.transform.parent.gameObject;
			}
			else
			{
				g = c.gameObject;
			}
			if (g == LocalPlayer)
			{
				Debug.LogWarning ("Doing SpeedBoost on " + g);
                playerscript.DoUpdate("1");
                g.transform.Find("Canvas/PowerCanvas/Boost").gameObject.GetComponent<Image>().enabled = true;
            }
			else
			{
				Debug.LogWarning ("Someone else collided with the powerup, removing it");
			}
			Destroy (gameObject);
		}
	}
}
