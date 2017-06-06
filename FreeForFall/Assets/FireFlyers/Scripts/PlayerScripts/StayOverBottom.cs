using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StayOverBottom : MonoBehaviour
{
	public Transform bottom;
    public float x;
    public float y;
    public float z;
	void LateUpdate ()
	{
		transform.position = bottom.position + new Vector3 (x, y, z);
	}
}
