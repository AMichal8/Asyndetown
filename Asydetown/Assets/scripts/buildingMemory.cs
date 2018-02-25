using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class buildingMemory : MonoBehaviour {

	public bool isGoal = false;

	float secsHighlighted;
	float lookAtCounter = 0f;

	bool isTargetLocation;

	public int importanceRanking;
	public int importanceThreshold = 5;

	bool isHit = false;
	bool wasHit = false;

	colorTimer myCT;
	changeColor mycC;

	// Use this for initialization
	void Start () 
	{
		myCT = GetComponent<colorTimer> ();
		mycC = GetComponent<changeColor> ();
	}
	
	// Update is called once per frame
	void Update () 
	{	
		if (isGoal)
			mycC.setGoalColor ();
		
		if (isHit) 
		{
			wasHit = true;
			myCT.addToTimer (5f);

		}

		if (!isHit && wasHit) 
		{
			StartCoroutine(myCT.subtractFromTimer (3f));	
			wasHit = false;
		}

	}
	public void contacted()
	{
		isHit = true;
		lookAtCounter += 1f;
		checkImportance ();

		GetComponent<colorTimer> ().addToTimer (2);
	}
	public bool checkContact()
	{
		return isHit;
	}
	public void uncontacted()
	{
		isHit = false;
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
