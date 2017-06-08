﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Swap : MonoBehaviour {

    public GameObject LocalPlayer;
    public PowerUpsHud playerscript;

    void Start()
    {
        gameObject.GetComponent<ParticleSystem>().startColor = Color.yellow;
    }

    void OnTriggerEnter(Collider c)
    {
        Debug.LogWarning("Something collided with the powerup");
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
                Debug.LogWarning("Doing Swap on " + g);
                playerscript.DoUpdate("3");
            }
            else
            {
                Debug.LogWarning("Someone else collided with the powerup, removing it");
            }
            Destroy(gameObject);
        }
    }
}
