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
        if (timerOn == true) { 
			fraction += Time.deltaTime;
			if(fraction >= 1){
				fraction %= 1;
				seconds++;
				if(seconds > 60){
					seconds %= 60;
					minutes++;
				}
			}
			var disp = (fraction * 100) % 100;
			timerLabel.text = string.Format("{0}:{1}:{2:0##}", minutes, seconds, disp);
        }
    }
}