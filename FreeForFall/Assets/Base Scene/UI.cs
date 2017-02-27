using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UI : MonoBehaviour
{
    public Text timerLabel;

	private float fraction = 0f;
	private float seconds = 0f;
	private float minutes = 0f;
    private bool timerOn = true;

    void Update()
    {
            time += Time.deltaTime;

            var minutes = time / 60; 
            var seconds = time % 60;
            var fraction = (time * 100) % 100;

            timerLabel.text = string.Format("{0:00} : {1:00} : {2:00}", minutes, seconds, fraction);
        }
    }


