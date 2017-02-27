using UnityEngine;

public class Movement : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		var x = Input.GetAxis("Horizontal");
        var z = Input.GetAxis("Vertical");

        transform.Translate(x, 0, z);
	}
}
