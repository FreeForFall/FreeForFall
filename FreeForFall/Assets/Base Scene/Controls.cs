﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Controls : MonoBehaviour {

    public GameObject pauseMenuHolder;
    public GameObject hud;
    // Use this for initialization
    void Start () {

    }

    
// Update is called once per frame
void Update () {

        if ((Input.GetKeyDown(KeyCode.Joystick1Button6) || Input.GetKeyDown(KeyCode.R)))
        {
            SceneManager.LoadScene("BaseScence");
        }

        if ((Input.GetKeyDown(KeyCode.Joystick1Button8) || Input.GetKeyDown(KeyCode.Escape)))
        {
            Pause();
        }

    }

    public void Pause()
    {
        if (Time.timeScale == 1)
        {
            Time.timeScale = 0;
            hud.SetActive(false);
            pauseMenuHolder.SetActive(true);

        }
        else if (Time.timeScale == 0)
        {
            Time.timeScale = 1;
            hud.SetActive(false);
            pauseMenuHolder.SetActive(false);

        }
    }
}
