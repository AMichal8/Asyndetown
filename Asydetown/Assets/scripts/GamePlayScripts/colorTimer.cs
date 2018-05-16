using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class colorTimer : MonoBehaviour {

	changeColor colorManager;
	public float secondsTimer = 0f;
	[SerializeField]
	float maxSeconds = 0f;

	public bool fadeTransparency = false;
	public float transparency = 1;

	// Use this for initialization
	void Start () 
	{
		colorManager = GetComponent<changeColor> ();

		//colorManager.resetObjColor ();
	}
	
	// Update is called once per frame
	void Update () 
	{

		if (maxSeconds != 0) //if has been looked at
		{
			//Debug.Log (secondsTimer / maxSeconds + " is fraction of time.");
			colorManager.changeObjColor (transparency, secondsTimer / maxSeconds);

			
		} 
		else //if hasn't been looked at
		{
			if(!fadeTransparency)
				colorManager.resetObjColor ();
			else 
			{
				transparency -= 1f * Time.deltaTime;
				colorManager.setTransparency(transparency);

				if (transparency <= 0)
					this.gameObject.SetActive (false);
			}
		}
	}
	public void FadeTransparency()
	{
		fadeTransparency = true;
	}
	public void addToTimer(float seconds)
	{
		
		secondsTimer += seconds * Time.deltaTime;

		if (secondsTimer > maxSeconds)
			maxSeconds = secondsTimer;
		
		//Debug.Log ("Adding time... " + seconds + " seconds.");
		//Debug.Log ("Seconds Timer = " + secondsTimer);
		//Debug.Log ("MaxSeconds = " + maxSeconds);
	}

	public IEnumerator subtractFromTimer(float seconds)
	{
		while (secondsTimer > 0) 
		{
			secondsTimer -= seconds * Time.deltaTime;

			if (secondsTimer <= 0) 
			{
				secondsTimer = 0;

				StartCoroutine(reduceMemoryTime(seconds/2));

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
