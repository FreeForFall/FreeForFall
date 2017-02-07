using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundManager : MonoBehaviour {

	public float DropDelay;
	public float DestroyDelay;

	void Start () {
		if(DropDelay == 0f)
			DropDelay = 0.4f;
		if(DestroyDelay == 0f)
			DestroyDelay = 3f;
		foreach(Transform t in transform){
			t.gameObject.AddComponent<WaitToFall>();
		}
	}
}
