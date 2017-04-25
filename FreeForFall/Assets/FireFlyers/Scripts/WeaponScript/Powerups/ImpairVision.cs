using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AssemblyCSharp;

public class ImpairVision : MonoBehaviour
{
	public GameObject LocalPlayer;

	void OnTriggerEnter (Collider c)
	{
		Debug.LogWarning ("Something collided with the powerup");
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
			if (g == LocalPlayer)
			{
				Debug.LogWarning ("Doing ImpairVision on " + g);
				doPowerup (g);
			}
			else
			{
				Debug.LogWarning ("Someone else collided with the powerup, removing it");
			}
			Destroy (gameObject);
		}
	}

	private void doPowerup (GameObject c)
	{
		//Debug.LogWarning ("NOT IMPLEMENTED : Impairing the vision");
        NetworkEventHandlers.SendEvent(new VisionImpairedEvent());
    }
}
