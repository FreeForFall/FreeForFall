using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StayOverBottom : MonoBehaviour
{
	public Transform bottom;

	void Update ()
	{
		transform.position = bottom.position + new Vector3 (-0.02f, 1.45f, 0);
	}
}
