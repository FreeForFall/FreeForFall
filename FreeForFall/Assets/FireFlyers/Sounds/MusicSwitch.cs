using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicSwitch : MonoBehaviour {
    public AudioSource music_space;
	// Use this for initialization
	void Start ()
    {
        GameObject a = GameObject.Find("MusicMenu");
        Destroy(a);
        music_space.Play();
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}
}
