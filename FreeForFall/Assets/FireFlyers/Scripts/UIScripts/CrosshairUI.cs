using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrosshairUI : MonoBehaviour
{

	public Texture2D crosshairImage;
	// Use this for initialization
	void OnGUI ()
	{
		crosshairImage.texelSize.Scale (new Vector2 (0.2f, 0.2f));
		float xMin = (Screen.width / 2) - (crosshairImage.width / 2);
		float yMin = (Screen.height / 2) - (crosshairImage.height / 2);
		GUI.DrawTexture (new Rect (xMin, yMin, crosshairImage.width, crosshairImage.height), crosshairImage);
	}
}
