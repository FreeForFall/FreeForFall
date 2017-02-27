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
			t.gameObject.AddComponent<Rigidbody>();
			var r = t.GetComponent<Rigidbody>();
			r.useGravity = true;
			r.isKinematic = true;
			var m1 = t.gameObject.AddComponent<MeshCollider>();
			var m2 = t.gameObject.AddComponent<MeshCollider>();
			m1.convex = true;
			m2.convex = true;
			m2.inflateMesh = true;
			m2.isTrigger = true;
		}
	}
}
