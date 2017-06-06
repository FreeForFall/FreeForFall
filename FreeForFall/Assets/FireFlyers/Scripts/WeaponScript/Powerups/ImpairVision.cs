using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AssemblyCSharp;

public class ImpairVision : MonoBehaviour
{
    public GameObject LocalPlayer;
    public PowerUpsHud playerscript;

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
            if (g == LocalPlayer )
            {
                Debug.LogWarning("Doing ImpairVision on " + g);
                playerscript.DoUpdate("0");
            }
            else
            {
                Debug.LogWarning("Someone else collided with the powerup, removing it");
            }
            Destroy(gameObject);
        }
    }
}
