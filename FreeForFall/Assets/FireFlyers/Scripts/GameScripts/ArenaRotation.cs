using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArenaRotation : MonoBehaviour {

    public float minimum = -5.0F;
    public float maximum = 5.0F;
    static float t = 0.0f;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        transform.Rotate(0, 0, Mathf.Lerp(minimum, maximum, t) * Time.deltaTime);
        t += 0.1f * Time.deltaTime;
        if (t > 3.0f)
        {
            float temp = maximum;
            maximum = minimum;
            minimum = temp;
            t = 0.0f;
        }

       
    }
}

