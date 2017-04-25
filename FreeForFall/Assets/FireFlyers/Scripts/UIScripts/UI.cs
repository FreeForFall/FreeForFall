using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using AssemblyCSharp;

public class UI : MonoBehaviour
{
	public Text timerLabel;
	private float time = 0f;
	private bool timerOn = true;
	private Image launcher;
	private Image jump;
	private Image hook;
	private float gripCD;
	private float launcherCD;
	private float lastExplosion;
	private float lastGrip;
	private float JumpCD;
	private float lastJump;

	private GameObject _localPlayer;
	private PlayerController _pController;
	private ShooterB _shooterB;


	void Start ()
	{
		_localPlayer = GameObject.Find ("NetworkManager").GetComponent<NetworkScript> ().Player;
		_pController = _localPlayer.GetComponentInChildren<PlayerController> ();
		_shooterB = _localPlayer.GetComponentInChildren<ShooterB> ();

		GameObject launchLabel = GameObject.Find ("Canvas/Rockets/Filled");
		GameObject jumpLabel = GameObject.Find ("Canvas/Jump/Filled");
		GameObject hookLabel = GameObject.Find ("Canvas/GrapplingHook/Filled");
        
		launcher = launchLabel.GetComponent<Image> ();
		jump = jumpLabel.GetComponent<Image> ();
		hook = hookLabel.GetComponent<Image> ();
		launcherCD = _shooterB.CDExplosion;
		gripCD = _shooterB.CDGrip;
		JumpCD = _pController.JumpCooldown;
		launcher.fillAmount = 1f;
		jump.fillAmount = 1f;
		hook.fillAmount = 1f;
		lastGrip = 0f;
		lastJump = 0f;
	}

    
	void Update ()
	{
		lastExplosion = _shooterB.TimeSinceLastExplosion;
		lastGrip = _shooterB.TimeSinceLastGrip;
		lastJump = _pController.TimeSincePreviousJump;
		launcher.fillAmount = lastExplosion / launcherCD;
		hook.fillAmount = lastGrip / gripCD;
		jump.fillAmount = lastJump / JumpCD;

		if (launcher.fillAmount == 1f)
			launcher.color = Color.green;
		else
			launcher.color = new Color32 (0, 160, 255, 255);

		if (hook.fillAmount == 1f)
			hook.color = Color.green;
		else
			hook.color = new Color32 (0, 160, 255, 255);
        
		if (jump.fillAmount == 1f)
			jump.color = Color.green;
		else
			jump.color = new Color32 (0, 160, 255, 255);
        
        
	}


	void Timer ()
	{
		time += Time.deltaTime;
		var minutes = time / 60;
		var seconds = time % 60;
		var fraction = (time * 100) % 100;
		timerLabel.text = string.Format ("{0:00} : {1:00} : {2:00}", minutes, seconds, fraction);
	}
}


