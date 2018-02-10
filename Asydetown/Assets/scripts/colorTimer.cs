using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class colorTimer : MonoBehaviour {

	changeColor colorManager;
	public float secondsTimer = 0f;
	[SerializeField]
	float maxSeconds = 0f;

	// Use this for initialization
	void Start () 
	{
		colorManager = GetComponent<changeColor> ();
		colorManager.resetObjColor ();
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (maxSeconds != 0) 
		{
			Debug.Log (secondsTimer / maxSeconds + " is fraction of time.");
			colorManager.changeObjColor (1, secondsTimer / maxSeconds);
		}
		else
			colorManager.resetObjColor ();
	}

	void OnTriggerEnter (Collider other)
	{
		addToTimer (10f);
	}
	void OnTriggerStay(Collider other)
	{
		addToTimer (4f);
	}
	void OnTriggerExit(Collider other)
	{
		StartCoroutine(subtractFromTimer (1f));	
	}

	void addToTimer(float seconds)
	{
		secondsTimer += seconds * Time.deltaTime;

		if (secondsTimer > maxSeconds)
			maxSeconds = secondsTimer;
		//Debug.Log ("Adding time... " + seconds + " seconds.");
		//Debug.Log ("Seconds Timer = " + secondsTimer);
		//Debug.Log ("MaxSeconds = " + maxSeconds);
	}

	IEnumerator subtractFromTimer(float seconds)
	{
		while (secondsTimer > 0) 
		{
			secondsTimer -= seconds * Time.deltaTime;

			if (secondsTimer <= 0) 
			{
				secondsTimer = 0;

				StartCoroutine(reduceMemoryTime(seconds));

			}

			yield return null;

		}
	}

	IEnumerator reduceMemoryTime(float reduceAmount)
	{
		while (maxSeconds > 0) 
		{
			maxSeconds -= (reduceAmount/2) * Time.deltaTime;

			if (maxSeconds < 0)
				maxSeconds = 0;

			yield return null;
		}
	}
}
