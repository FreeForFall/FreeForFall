using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundPowerup : MonoBehaviour {

    public float powerradius = 1;
    SphereCollider groundpowerCollider;
    public GameObject beam;

    // Use this for initialization
    void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (Input.GetKeyDown(KeyCode.F) || Input.GetKeyDown(KeyCode.Joystick1Button1))
        {
            print("input");
            Grounpowerup();
        }
    }

    public void Grounpowerup()
    {
        List<GameObject> fallenCell = beam.GetComponent<DestroyCell>().FallenCells;
        foreach(GameObject cell in fallenCell)
        {
            cell.GetComponent<WaitToFall>().Goback();
            print("cells");
        }
    }
}
