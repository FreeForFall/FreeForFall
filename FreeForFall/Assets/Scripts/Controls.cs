using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Controls : MonoBehaviour {

	// Use this for initialization
	void Start () {

    }

    
// Update is called once per frame
void Update () {

        if ((Input.GetKeyDown(KeyCode.Joystick1Button6) || Input.GetKeyDown(KeyCode.R)))
        {
            SceneManager.LoadScene("BaseScence");
        }

    }
}
