using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class ButtonScript : MonoBehaviour
{
	public GameObject Button;
	Text MyButtonText;
	string projectileParticleName;		// The variable to update the text component of the button

	FireProjectile effectScript;		// A variable used to access the list of projectiles
	ProjectileScript projectileScript;

	public float buttonsX;
	public float buttonsY;
	public float buttonsSizeX;
	public float buttonsSizeY;
	public float buttonsDistance;
	
	void Start ()
	{
		effectScript = GameObject.Find("FireProjectile").GetComponent<FireProjectile>();
		getProjectileNames();
		MyButtonText = Button.transform.FindChild("Text").GetComponent<Text>();
		MyButtonText.text = projectileParticleName;
	}

	void Update ()
	{
		MyButtonText.text = projectileParticleName;
//		print(projectileParticleName);
	}

	public void getProjectileNames()			// Find and diplay the name of the currently selected projectile
	{
		// Access the currently selected projectile's 'ProjectileScript'
		projectileScript = effectScript.projectiles[effectScript.currentProjectile].GetComponent<ProjectileScript>();
		projectileParticleName = projectileScript.projectileParticle.name;	// Assign the name of the currently selected projectile to projectileParticleName
	}

	public bool overButton()		// This function will return either true or false
	{
		Rect button1 = new Rect(buttonsX, buttonsY, buttonsSizeX, buttonsSizeY);
		Rect button2 = new Rect(buttonsX + buttonsDistance, buttonsY, buttonsSizeX, buttonsSizeY);
		
		if(button1.Contains(new Vector2(Input.mousePosition.x, Screen.height - Input.mousePosition.y)) ||
		   button2.Contains(new Vector2(Input.mousePosition.x, Screen.height - Input.mousePosition.y)))
		{
			return true;
		}
		else
			return false;
	}

	public void LoadStage1()  {
		SceneManager.LoadScene ("magic_projectiles");
	}
	public void LoadStage2()  {
        SceneManager.LoadScene ("magic_sprays");
	}
	public void LoadStage3()  {
        SceneManager.LoadScene ("magic_aura");
	}
	public void LoadStage4()  {
        SceneManager.LoadScene ("magic_modular");
	}
	public void LoadStage5()  {
        SceneManager.LoadScene("magic_domes");
	}
	public void LoadStage6()  {
        SceneManager.LoadScene("magic_shields");
	}
	public void LoadStage7()  {
        SceneManager.LoadScene("magic_sphereblast");
	}
}
