using UnityEngine;
using System.Collections;

public class FireProjectile : MonoBehaviour 
{
    RaycastHit hit;
    public GameObject[] projectiles;
    public Transform spawnPosition;
    [HideInInspector]
    public int currentProjectile = 0;
	public float speed = 1000;

//    MyGUI _GUI;
	ButtonScript selectedProjectileButton;

	void Start () 
	{
		selectedProjectileButton = GameObject.Find("Button").GetComponent<ButtonScript>();
	}

	void Update () 
	{
		if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            nextEffect();
			selectedProjectileButton.getProjectileNames(); // Run the getProjectileNames() function in ButtonScript
        }

		if (Input.GetKeyDown(KeyCode.D))
		{
			nextEffect();
			selectedProjectileButton.getProjectileNames();
		}

		if (Input.GetKeyDown(KeyCode.A))
		{
			previousEffect();
			selectedProjectileButton.getProjectileNames();
		}
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            previousEffect();
			selectedProjectileButton.getProjectileNames();
        }

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {

			if (!selectedProjectileButton.overButton())
            {
                if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 100f))
                {
                    GameObject projectile = Instantiate(projectiles[currentProjectile], spawnPosition.position, Quaternion.identity) as GameObject;
                    projectile.transform.LookAt(hit.point);
                    projectile.GetComponent<Rigidbody>().AddForce(projectile.transform.forward * speed);
                    projectile.GetComponent<ProjectileScript>().impactNormal = hit.normal;
                }  
            }

        }
        Debug.DrawRay(Camera.main.ScreenPointToRay(Input.mousePosition).origin, Camera.main.ScreenPointToRay(Input.mousePosition).direction*100, Color.yellow);
	}

    public void nextEffect()
    {
        if (currentProjectile < projectiles.Length - 1)
            currentProjectile++;
        else
            currentProjectile = 0;
    }

    public void previousEffect()
    {
        if (currentProjectile > 0)
            currentProjectile--;
        else
            currentProjectile = projectiles.Length-1;
    }

	public void AdjustSpeed(float newSpeed)
	{
		speed = newSpeed;
	}
}
