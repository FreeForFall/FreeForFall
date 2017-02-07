using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{

    public GameObject mainMenuHolder;


    public void Play()
    {
        SceneManager.LoadScene("BaseScence");
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void SetFullscreen(bool isFullscreen)
    {

    }
}
