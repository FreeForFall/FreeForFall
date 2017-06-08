using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
        if (c.gameObject.tag == "Player" )
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
                Debug.LogWarning("Doing Swap on " + g);
                playerscript.DoUpdate("3");
                g.transform.Find("Canvas/PowerCanvas/Swap").gameObject.GetComponent<Image>().enabled = true;
            }
            else
            {
                Debug.LogWarning("Someone else collided with the powerup, removing it");
            }
            Destroy(gameObject);
        }
    }
}
