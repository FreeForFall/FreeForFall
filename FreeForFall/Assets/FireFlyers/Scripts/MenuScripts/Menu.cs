using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{

	public GameObject mainMenuHolder;
	public GameObject optionsMenuHolder;
	public GameObject pauseMenuHolder;
	public GameObject hud;
	public Text timerLabel;
	public Text timerLabel2;
	public Toggle[] resolutionToggles;
	public Toggle fullscreenToggle;
	public int[] screenWidths;
	int activeScreenResIndex;
	public bool fps;

    
	void Start ()
	{
		Time.timeScale = 1;
		activeScreenResIndex = PlayerPrefs.GetInt ("screen res index");
		bool isFullscreen = (PlayerPrefs.GetInt ("fullscreen") == 1) ? true : false;

		for (int i = 0; i < resolutionToggles.Length; i++)
		{
			resolutionToggles [i].isOn = i == activeScreenResIndex;
		}

		fullscreenToggle.isOn = isFullscreen;
        
	}

	void pause ()
	{
		while (true)
			timerLabel.text = timerLabel2.text;
	}

	public void Play ()
	{
		SceneManager.LoadScene ("BaseScence");
	}

	public void Quit ()
	{
		Application.Quit ();
	}

	public void OptionsMenu ()
	{
		mainMenuHolder.SetActive (false);
		optionsMenuHolder.SetActive (true);
	}

	public void MainMenu ()
	{
		optionsMenuHolder.SetActive (false);
		mainMenuHolder.SetActive (true);
	}

	public void PauseMenu ()
	{
		optionsMenuHolder.SetActive (false);
		pauseMenuHolder.SetActive (true);
	}

	public void OptionsPause ()
	{
		pauseMenuHolder.SetActive (false);
		optionsMenuHolder.SetActive (true);
	}

	public void SetResolution ()
	{
		if (resolutionToggles [3].isOn)
		{
			activeScreenResIndex = 3;
			Screen.SetResolution (960, 540, false);
			PlayerPrefs.SetInt ("screen res index", activeScreenResIndex);
			PlayerPrefs.Save ();
		}
		if (resolutionToggles [2].isOn)
		{
			activeScreenResIndex = 2;
			Screen.SetResolution (1280, 720, false);
			PlayerPrefs.SetInt ("screen res index", activeScreenResIndex);
			PlayerPrefs.Save ();
		}
		if (resolutionToggles [1].isOn)
		{
			activeScreenResIndex = 1;
			Screen.SetResolution (1920, 1080, false);
			PlayerPrefs.SetInt ("screen res index", activeScreenResIndex);
			PlayerPrefs.Save ();
		}
		if (resolutionToggles [0].isOn)
		{
			activeScreenResIndex = 0;
			Resolution[] allResolutions = Screen.resolutions;
			Resolution maxResolution = allResolutions [allResolutions.Length - 1];
			Screen.SetResolution (maxResolution.width, maxResolution.height, false);
			PlayerPrefs.SetInt ("screen res index", activeScreenResIndex);
			PlayerPrefs.Save ();
		}
      
	}

	public void SetSensitvity (Slider slider)
	{
		GameObject.Find ("SettingsManager").GetComponent<Settings> ().Sensitivity = slider.value;
	}

	public void SetCamera (bool fps)
	{
		GameObject.Find ("SettingsManager").GetComponent<Settings> ().FpsMode = true;
	}


	public void SetFullscreen (bool isFullscreen)
	{
		for (int i = 0; i < resolutionToggles.Length; i++)
		{
			resolutionToggles [i].interactable = !isFullscreen;
		}

		if (isFullscreen)
		{
			Resolution[] allResolutions = Screen.resolutions;
			Resolution maxResolution = allResolutions [allResolutions.Length - 1];
			Screen.SetResolution (maxResolution.width, maxResolution.height, true);
		}
		else
		{
			SetResolution ();
		}

		PlayerPrefs.SetInt ("fullscreen", ((isFullscreen) ? 1 : 0));
		PlayerPrefs.Save ();
	}

	public void QuitPause ()
	{
		SceneManager.LoadScene ("Menu");
	}


	public void Pause ()
	{
        
		if (Time.timeScale == 1)
		{
			Time.timeScale = 0;
            
			pauseMenuHolder.SetActive (true);

		}
		else if (Time.timeScale == 0)
		{
			Time.timeScale = 1;
			hud.SetActive (true);
            
			pauseMenuHolder.SetActive (false);

		}
	}


}
