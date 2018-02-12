using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class buildingMemory : MonoBehaviour {


	float secsHighlighted;
	float lookAtCounter = 0f;



	public int importanceRanking;
	public int importanceThreshold = 5;


	// Use this for initialization
	void Start () 
	{
		
	}
	
	// Update is called once per frame
	void Update () 
	{
		
	}
	public void contacted()
	{
		lookAtCounter += 1f;
		checkImportance ();

		GetComponent<colorTimer> ().addToTimer (2);
	}
	void checkImportance()
	{
		if (lookAtCounter >= importanceThreshold)
			importanceRanking++;
	}
	void increaseImportanceRanking(int increaseValue)
	{
		importanceRanking += increaseValue;
	}
	void decreaseImportanceRanking(int decreaseValue)
	{
		importanceRanking -= decreaseValue;
	}
}
