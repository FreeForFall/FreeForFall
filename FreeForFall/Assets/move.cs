using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class move : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		var dir = Input.GetAxis("Horizontal");
		if(Mathf.Abs(dir) > 0.4){
			Debug.Log("Trying to move it");
			Vector3 newpos = GameObject.Find("OtherPlayer").transform.position;
			newpos += Vector3.left * dir;
			GameObject.Find("NetworkManager").GetComponent<NetworkScript>().FutureActions.Enqueue(new AssemblyCSharp.MovementAction(1, newpos)); 
			Debug.Log("Added a movement action");
		}
	}
}
