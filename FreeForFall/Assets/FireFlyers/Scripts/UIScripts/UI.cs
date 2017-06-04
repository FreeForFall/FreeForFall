using UnityEngine;
using UnityEngine.UI;
using AssemblyCSharp;

public class UI : MonoBehaviour
{
    public Text timerLabel;
    private float time = 0f;
    private Image launcher;
    private Image jump;
    private Image hook;
    private float lastExplosion;
    private float lastGrip;
    private float lastJump;

    private GameObject _localPlayer;
    private PlayerController _pController;
    private ShooterB _shooterB;


    void Start()
    {
        _localPlayer = GameObject.Find("NetworkManager").GetComponent<Networking>().Player;
        _pController = _localPlayer.GetComponentInChildren<PlayerController>();
        _shooterB = _localPlayer.GetComponentInChildren<ShooterB>();

        GameObject launchLabel = _localPlayer.transform.Find("Canvas/Rockets/Filled").gameObject;
        GameObject jumpLabel = _localPlayer.transform.Find("Canvas/Jump/Filled").gameObject;
        GameObject hookLabel = _localPlayer.transform.Find("Canvas/GrapplingHook/Filled").gameObject;
        
        launcher = launchLabel.GetComponent<Image>();
        jump = jumpLabel.GetComponent<Image>();
        hook = hookLabel.GetComponent<Image>();
        launcher.fillAmount = 1f;
        jump.fillAmount = 1f;
        hook.fillAmount = 1f;
        lastGrip = 0f;
        lastJump = 0f;
    }

    
    void Update()
    {
        lastExplosion = _shooterB.TimeSinceLastExplosion;
        lastGrip = _shooterB.TimeSinceLastGrip;
        lastJump = _pController.TimeSincePreviousJump;
        launcher.fillAmount = lastExplosion / Constants.BAZOOKA_CD;
        hook.fillAmount = lastGrip / Constants.GRIP_CD;
        jump.fillAmount = lastJump / Constants.JUMP_CD;

        if (launcher.fillAmount == 1f)
            launcher.color = Color.green;
        else
            launcher.color = new Color32(0, 160, 255, 255);

        if (hook.fillAmount == 1f)
            hook.color = Color.green;
        else
            hook.color = new Color32(0, 160, 255, 255);
        
        if (jump.fillAmount == 1f)
            jump.color = Color.green;
        else
            jump.color = new Color32(0, 160, 255, 255);
        
        
    }


    void Timer()
    {
        time += Time.deltaTime;
        var minutes = time / 60;
        var seconds = time % 60;
        var fraction = (time * 100) % 100;
        timerLabel.text = string.Format("{0:00} : {1:00} : {2:00}", minutes, seconds, fraction);
    }
}
