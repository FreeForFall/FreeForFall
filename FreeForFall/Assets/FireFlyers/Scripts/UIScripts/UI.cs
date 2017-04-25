using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UI : MonoBehaviour
{
    public Text timerLabel;
    private float time = 0f;
    private bool timerOn = true;
    private Image launcher;
    private Image jump;
    private Image hook;
    private bool coolingDown;
    public float gripCD;
    public float launcherCD;

    void Start()
    {
        GameObject launchLabel = GameObject.Find("Canvas/Rockets/Filled");
        GameObject jumpLabel = GameObject.Find("Canvas/Jump/Filled");
        GameObject hookLabel = GameObject.Find("Canvas/GrapplingHook/Filled");
        launcher = launchLabel.GetComponent<Image>();
        jump = jumpLabel.GetComponent<Image>();
        hook = hookLabel.GetComponent<Image>();
        launcherCD = GetComponent<ShooterB>().CDExplosion;
        launcher.fillAmount = 1f;
        jump.fillAmount = 1f;
        hook.fillAmount = 1f;

    }

    
    void Update()
    {
        if (Input.GetMouseButtonDown(0) || Input.GetKey(KeyCode.Joystick1Button5))
        {
            launcher.fillAmount = 0f;
            while (launcher.fillAmount < 1f)
            {
                
                launcher.fillAmount += 1f / launcherCD * Time.deltaTime;
            }
        }
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


