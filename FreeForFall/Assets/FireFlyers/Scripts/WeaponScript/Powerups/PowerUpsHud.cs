using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AssemblyCSharp;

public class PowerUpsHud : MonoBehaviour {

    public bool OnePowerUp = false;
    public string action;
    public string todo = "";
    public GameObject self;

	// Use this for initialization
	void Start ()
    {
		
	}

	// Update is called once per frame
	public void DoUpdate (string powerup)
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
    }

    public void SpeedBoost(GameObject c)
    {
        Debug.LogWarning("Giving the player a speed boost");
        c.GetComponentInChildren<PlayerController>().SpeedBoost(Constants.SPEED_BOOST_POWERUP_MULT);
    }

    public void CooldownRefresh(GameObject c)
    {
        c.GetComponentInParent<ShooterB>().RefreshCooldowns();
    }

    public void Swap (GameObject c)
    {
        c.GetComponentInChildren<PlayerController>().Swap();
    }
}
