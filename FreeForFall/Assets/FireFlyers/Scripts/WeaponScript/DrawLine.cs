using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawLine : MonoBehaviour
{


	private LineRenderer lineRenderer;
	private float counter;
	private float dist;

	public Transform player;
	public Transform grip;

	// Use this for initialization
	void Start ()
	{
		lineRenderer = GetComponent<LineRenderer> ();
		lineRenderer.SetPosition (0, player.transform.position);
		lineRenderer.SetPosition (1, grip.transform.position);
	}
	
	// Update is called once per frame
	void Update ()
	{
		lineRenderer.SetPosition (0, player.transform.position);
		lineRenderer.SetPosition (1, grip.transform.position);
	}
}
