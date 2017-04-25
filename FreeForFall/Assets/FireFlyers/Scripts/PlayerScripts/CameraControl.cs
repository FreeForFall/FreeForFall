using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{

	public Transform player;
	//the full object (in this case robot 1)
	public Transform target;
	//the target is a children of the body, it is the point where is looking the camera in TPS view
	public float orbitDegreesPerSec = 0.5f;
	public float sensivity = 4f;
	//sensivity is only used for the upward and downward speed int TPS. (but i need to ad a variablefor all other movement)
	public Vector3 relativeDistance;
	// distance at witch the camera will orbit arround the object
	public Vector3 initial_distance;
	// initializing the distance of the camera from the object
	public Vector3 fpsvertical;
	public bool once = true;
	public float MaxCameraAngleUp = 12;
	//max integer hieght of the camera in TPS
	public float MaxCameraAngleDown = 2;
	// min integer hight of the camera in TPS
	public Camera Principale;
	// the principale camera the will be the onlyone used by the player
	public Camera Secondaire;
	// a secondary camera that will stay in orbit (can be used for the big screen and such)
	public Transform FPSAnchor;
	// the place where the camera goes whene in FPS mode
	private bool istps = true;
	// the bool to know if the player is in TPS or FPS
	private Vector3 current;
	// suposed to calculate the angle on the x axis for the FPS view (but ... mehhhh not really working that great...)
	private Vector3 FPSyaxisInput;
	private float FPS_updown;
	private Quaternion rotationFPS;
	public float rot;




	void Start ()
	{
		Secondaire.enabled = false;
		Principale.enabled = true;
		Cursor.visible = false;
		//initialisation of the Camera (remember the camera is an empty object that host 2 real camera, the primary and secondary cameras. 
		initial_distance = new Vector3 (2, 2, -15);
		if (target != null)
		{
			transform.position = player.position + initial_distance;
			relativeDistance = transform.position - player.position;
		}
	}

	//the main methode

	void Orbit ()
	{
		//check for any input

		float h = Input.GetAxis ("Mouse X");
		float v = Input.GetAxis ("Mouse Y");
		//position the camera depending of the camera mode
		if (istps)
		{

			Principale.transform.position = Vector3.MoveTowards (Principale.transform.position, transform.position, 50 * Time.deltaTime);
		}
		else
			Principale.transform.position = Vector3.MoveTowards (Principale.transform.position, FPSAnchor.position, 50 * Time.deltaTime);

		//no matter what mode you are in the rotation for the Horizontal axis is the same

		if (target != null)
		{

			transform.position = Vector3.Lerp (transform.position, target.position + relativeDistance, 10 * 0.125f);
			transform.RotateAround (target.position, Vector3.up, orbitDegreesPerSec * h * 0.125f);
			relativeDistance = transform.position - target.position;
		}

		//here is the rotation for the TPS camera (it just raise or decrease the height of the camera).

		if (v != 0 && istps)
		{
			if (transform.position.y < target.position.y + MaxCameraAngleUp && v < 0)
			{
				Vector3 mouvement_up = new Vector3 (0, v, 0);
				relativeDistance = relativeDistance - mouvement_up;
				FPSAnchor.position = FPSAnchor.position + mouvement_up;
			}
			if (transform.position.y > target.position.y - MaxCameraAngleDown && v > 0)
			{
				Vector3 mouvement_up = new Vector3 (0, v, 0);
				relativeDistance = relativeDistance - mouvement_up;
				FPSAnchor.position = FPSAnchor.position + mouvement_up;

			}
			rot += -v * 2;
			if (rot < -18) //Check for lower limit
                rot = -18;
			if (rot > 39) //Check for upper limit
                rot = 39;
		}

		//here is the rotation for the fps view 

		if (v != 0 && !istps)
		{
			rot += -v * 2;
			if (rot < -60) //Check for lower limit
                rot = -60;
			if (rot > 60) //Check for upper limit
                rot = 60;
			Principale.transform.localRotation = Quaternion.AngleAxis (rot, Vector3.right);
		}
	}


	void LateUpdate ()
	{
		//change between FPS and TPS view
#if UNITY_STANDALONE_WIN
        if ((Input.GetKeyDown(KeyCode.Joystick1Button9) || Input.GetKeyDown(KeyCode.E)))
#endif
#if UNITY_STANDALONE_LINUX
		if ((Input.GetKeyDown (KeyCode.JoystickButton10) || Input.GetKeyDown (KeyCode.E)))
#endif
        {
			istps = !istps;
			if (istps)
				Principale.fieldOfView = 65f;
			else
				Principale.fieldOfView = 80f;
		}
		Orbit ();
		if (istps && Principale.transform.position == transform.position)
		{
			Vector3 lookto = new Vector3 (target.position.x, target.position.y + 2, target.position.z);
			Principale.transform.rotation = Quaternion.RotateTowards (Principale.transform.rotation,
				Quaternion.LookRotation (lookto - Principale.transform.position), 200 * Time.deltaTime);
		}
	}

}



