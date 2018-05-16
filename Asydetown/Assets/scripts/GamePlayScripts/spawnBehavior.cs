using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spawnBehavior : MonoBehaviour {

	public bool isTarget = false;
	public bool contactPlayer = false;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (!isTarget)
			contactPlayer = false;
	}
	void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag ("Player") && isTarget) 
		{
			contactPlayer = true;
		}
	}
}
