using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GoalManager : MonoBehaviour {

	[SerializeField]
	GameObject[] buildings;

	GameObject goal;

	// Use this for initialization
	void Start () 
	{
		if (buildings == null || buildings.Length ==0)
			buildings = GameObject.FindGameObjectsWithTag ("building");

		randomlySelectGoal ();
		Debug.Log ("The goal is " + goal);
		
	}
	
	// Update is called once per frame
	void Update () 
	{
		checkGoalFound();
		
	}
	void checkGoalFound()
	{
		if (goal.GetComponent<buildingMemory> ().checkContact ()) 
		{
			SceneManager.LoadScene ("Prototype1");
		}
	}

	void randomlySelectGoal()
	{
		if (buildings.Length != 0) 
		{
			float rand = Random.Range (0, buildings.Length - 1);
			Debug.Log ((int)rand + " is the randomly choosen index in array.");
			goal = buildings[(int)rand];

			goal.GetComponent<buildingMemory> ().isGoal = true;
		}
	}
}
