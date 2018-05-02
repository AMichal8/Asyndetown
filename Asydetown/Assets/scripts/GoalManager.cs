using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GoalManager : MonoBehaviour {


	public int maxDistance = 15;
	public int minDistance = 8;
	//For Transitional Fades
	public float FadeInOutSeconds = 2f;


	public float buildingFadeTimer = 180f;

	//[SerializeField]
	public List<GameObject> buildings;

	public GameManager gm;

	GameObject player;

	GameObject goal;
	GameObject previousGoal;
	GameObject spawnGoal;

	bool beganManaging = false;

	bool targetIsBuildingGoal = false;
	public Image fade;

	// Use this for initialization
	void Start () 
	{
		

	}

	// Update is called once per frame
	void Update () 
	{
		StartCoroutine ("buildingTransparencyTimer");

		if(beganManaging)
			checkGoalFound();

	}
	IEnumerator buildingTransparencyTimer()
	{
		Debug.Log ("Beginning Time for building's fade");
		yield return new WaitForSeconds (buildingFadeTimer);

		foreach (GameObject building in buildings) 
		{
			if (building.GetComponent<buildingMemory> ().importanceRanking ==0) 
			{
				//buildings.Remove (building);
				building.GetComponent<buildingMemory> ().FadeAway ();
			}
		}

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

	public void manageGoal()
	{
		player = GameObject.FindGameObjectWithTag ("Player");
		spawnGoal = gm.playerSpawn;
		int tagged = 0;

		if (buildings.Count == 0) 
		{
			foreach (GameObject build in GameObject.FindGameObjectsWithTag ("building")) 
			{
				Debug.Log ("tagged objects: " + ++tagged);
				buildings.Add (build);
			}
				
		}
			

		if(spawnGoal ==null)
			spawnGoal = gm.playerSpawn;
		
		if(player == null)
			player = GameObject.FindGameObjectWithTag ("Player");

		randomlySelectGoal ();

		//Debug.Log ("The goal is " + goal);

		beganManaging = true;
	}

	void randomlySelectGoal()
	{
		if (buildings.Count != 0)
		{
			Debug.Log ("Building's length does not equal zero!");

			bool checkPassed = false;
			float rand;

//			if (spawnGoal == null)
//				spawnGoal = gm.playerSpawn;

			while (!checkPassed) 
			{
				//Debug.Log ("Beginning while loop in goal selection");
				rand = Random.Range (0, buildings.Count - 1);
				//Debug.Log ((int)rand + " is the randomly choosen index in array.");

				if ((Mathf.Abs(spawnGoal.transform.position.x - buildings[(int)rand].transform.position.x) < maxDistance && Mathf.Abs(spawnGoal.transform.position.x - buildings[(int)rand].transform.position.x) > minDistance) &&
					(Mathf.Abs(spawnGoal.transform.position.z - buildings[(int)rand].transform.position.z) < maxDistance && Mathf.Abs(spawnGoal.transform.position.z - buildings[(int)rand].transform.position.z) > minDistance))
				{
					goal = buildings [(int)rand];
					Debug.Log ("The goal is " + goal + " at position: " + goal.transform.position);
					checkPassed = true;

				}
				else
				{
					//Debug.Log ("Building is not in range... " + " or something else went wrong ):");

					//buildings.Remove (buildings [(int)rand]);
					//Debug.Log ("Removing " + buildings [(int)rand]);

					//rand = Random.Range (0, buildings.Length - 1);

				}

			}

			goal.GetComponent<buildingMemory> ().isGoal = true;
			targetIsBuildingGoal = true;
		}
	}

	IEnumerator goalChange()
	{
		player.GetComponent<playerMovement> ().inTransition = true;

		StartCoroutine ("FadeOut");
		if (targetIsBuildingGoal) 
		{
			deSelectGoal ();
			yield return new WaitForSeconds (FadeInOutSeconds);
			StartCoroutine ("FadeIn");
			setSpawnGoal ();
			player.GetComponent<playerMovement> ().inTransition = false;
		} 
		else 
		{
			deSelectSpawnGoal ();
			yield return new WaitForSeconds (FadeInOutSeconds);
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
