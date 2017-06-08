using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AssemblyCSharp;
using UnityEngine.UI;


public class ImpairVision : MonoBehaviour
{
    public GameObject LocalPlayer;
    public PowerUpsHud playerscript;

    void OnTriggerEnter(Collider c)
    {
        Debug.LogWarning("Something collided with the powerup");
        playerscript = c.GetComponent<PowerUpsHud>();
        if (c.gameObject.tag == "Player")
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
            if (g == LocalPlayer && !playerscript.OnePowerUp)
            {
                Debug.LogWarning("Doing ImpairVision on " + g);
                playerscript.DoUpdate("0");
                g.transform.Find("Canvas/PowerCanvas/ImpairedVision").gameObject.GetComponent<Image>().enabled = true;
            }
            else
            {
                Debug.LogWarning("Someone else collided with the powerup, removing it");
            }
            Destroy(gameObject);
        }
    }
}
