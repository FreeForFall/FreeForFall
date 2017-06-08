using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AssemblyCSharp;
using UnityEngine.UI;

public class PowerUpsHud : MonoBehaviour
{

    public bool OnePowerUp = false;
    public string action;
    public string todo = "";
    public GameObject self;
    public GameObject visionparticlelocal;
    public GameObject visionparticlenonlocal;

    // Use this for initialization
    void Start()
    {
		
    }

    // Update is called once per frame
    public void DoUpdate(string powerup)
    {
        todo = powerup;
        OnePowerUp = true;
        print(todo);
    }

    void Update()
    {
        if ((Input.GetKeyDown(KeyCode.Joystick1Button2) || Input.GetKeyDown(KeyCode.R)) && todo != "")
        {
            if (todo == "0")
                ImpairedVision(self);
            if (todo == "1")
                SpeedBoost(self);
            if (todo == "2")
                CooldownRefresh(self);
            if (todo == "3")
                Swap(self);
            todo = "";
            OnePowerUp = false;
        }
    }

    public void ImpairedVision(GameObject c)
    {
        NetworkEventHandlers.Broadcast(Constants.EVENT_IDS.IMPAIR_VISION_EFFECT);
        NetworkEventHandlers.Broadcast(Constants.EVENT_IDS.VISION_IMPAIRED_PARTICLES, transform.position);
        GameObject g = c.transform.parent.gameObject;
        g.transform.Find("Canvas/PowerCanvas/ImpairedVision").gameObject.GetComponent<Image>().enabled = false;
        ImpairedVisionParticles(transform.position, true);
    }

    public void ImpairedVisionParticles(Vector3 a, bool local = false)
    {
        Debug.Log("Impair vision particles");
        GameObject visionbuff;
        if (local)
            visionbuff = Instantiate(visionparticlelocal, a, Quaternion.identity, transform) as GameObject;
        else
            visionbuff = Instantiate(visionparticlenonlocal, a, Quaternion.identity) as GameObject;
        Destroy(visionbuff, 2f);
    }

    public void SpeedBoost(GameObject c)
    {
        Debug.LogWarning("Giving the player a speed boost");
        c.GetComponentInChildren<PlayerController>().SpeedBoost(Constants.SPEED_BOOST_POWERUP_MULT);
        GameObject g = c.transform.parent.gameObject;
        g.transform.Find("Canvas/PowerCanvas/Boost").gameObject.GetComponent<Image>().enabled = false;
    }

    public void CooldownRefresh(GameObject c)
    {
        c.GetComponentInParent<ShooterB>().RefreshCooldowns();
        GameObject g = c.transform.parent.gameObject;
        g.transform.Find("Canvas/PowerCanvas/Cooldown").gameObject.GetComponent<Image>().enabled = false;
    }

    public void Swap(GameObject c)
    {
        c.GetComponentInChildren<PlayerController>().Swap();
        GameObject g = c.transform.parent.gameObject;
        g.transform.Find("Canvas/PowerCanvas/Swap").gameObject.GetComponent<Image>().enabled = false;
    }
}
