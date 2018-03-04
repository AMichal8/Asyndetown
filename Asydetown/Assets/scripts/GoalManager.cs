using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GoalManager : MonoBehaviour {

	[SerializeField]
	GameObject[] buildings;

	public GameManager gm;

	GameObject player;

	GameObject goal;
	GameObject previousGoal;
	GameObject spawnGoal;

	bool targetIsBuildingGoal = false;
	public Image fade;

	// Use this for initialization
	void Start () 
	{
		player = GameObject.FindGameObjectWithTag ("Player");

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
		if (targetIsBuildingGoal && goal !=null) 
		{
			if(goal.GetComponent<buildingMemory> ().checkContact ())
			{
				StartCoroutine ("goalChange");

			}
		}
		if (!targetIsBuildingGoal && spawnGoal!=null) 
		{
			if(spawnGoal.GetComponent<spawnBehavior> ().contactPlayer == true)
			{
				Debug.Log ("SpawnGoal Reached! Huzzah!");
				StartCoroutine ("goalChange");


			}
		}
	}

	IEnumerator goalChange()
	{
		player.GetComponent<playerMovement> ().inTransition = true;

		StartCoroutine ("FadeOut");
		if (targetIsBuildingGoal) 
		{
			deSelectGoal ();
			yield return new WaitForSeconds (2f);
			StartCoroutine ("FadeIn");
			setSpawnGoal ();
			player.GetComponent<playerMovement> ().inTransition = false;
		} 
		else 
		{
			deSelectSpawnGoal ();
			yield return new WaitForSeconds (2f);
			StartCoroutine ("FadeIn");
			setPreviousGoal ();
			player.GetComponent<playerMovement> ().inTransition = false;
		}
		gm.roundCounter (1);
	
			
	}
	IEnumerator FadeOut()
	{
		for (float f = 0f; f <= 1.5f; f += .05f) 
		{
			Color tempColor = fade.color;
			tempColor.a = f;
			fade.color = tempColor;
			yield return null;
		}
	}
	IEnumerator FadeIn()
	{
		for (float f = 1f; f >= -.5f; f -= .025f) 
		{
			Color tempColor = fade.color;
			tempColor.a = f;
			fade.color = tempColor;
			yield return null;
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
			targetIsBuildingGoal = true;
		}
	}
	void deSelectGoal()
	{
		if (goal != null) 
		{
			
			goal.GetComponent<buildingMemory> ().isGoal = false;
			previousGoal = goal;
			goal = null;
			targetIsBuildingGoal = false;

		}
	}

	void setPreviousGoal()
	{		
		targetIsBuildingGoal = true;
		goal = previousGoal;
		goal.GetComponent<buildingMemory> ().isGoal = true;
		previousGoal = null;

	}

	void setSpawnGoal()
	{
		Color tempColor = gm.playerSpawn.GetComponent<SpriteRenderer> ().color;
		tempColor.a = 1f;
		gm.playerSpawn.GetComponent<SpriteRenderer> ().color = tempColor;
		gm.playerSpawn.GetComponent<spawnBehavior> ().isTarget = true;
		spawnGoal = gm.playerSpawn;

	}
	void deSelectSpawnGoal()
	{
		Color tempColor = gm.playerSpawn.GetComponent<SpriteRenderer> ().color;
		tempColor.a = 0f;
		spawnGoal.GetComponent<SpriteRenderer> ().color = tempColor;
		spawnGoal.GetComponent<spawnBehavior> ().isTarget = false;
		spawnGoal = null;


	}
}
