using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class GameManager : MonoBehaviour {

	GameObject[] spawns;
	public int maxRounds = 3;

	public GameObject playerPrefab;
	public GameObject playerSpawn;

	GameObject gamePlayer;

	public GoalManager goalMan;
	public StationManager stationMan;
	[SerializeField]
	int counter = 0;

	GameObject walls;

	bool beginFade = true;

	// Use this for initialization
	void Awake () 
	{
		if (spawns == null || spawns.Length == 0)
			spawns = GameObject.FindGameObjectsWithTag ("spawn");

		if (walls == null)
			walls = GameObject.FindGameObjectWithTag ("walls");

		//goalMan = GetComponent<GoalManager> ();

		InitGame ();

	}
	void InitGame()
	{
		selectSpawnPosition ();
		GameObject player = Instantiate (playerPrefab, playerSpawn.transform.position, transform.rotation) as GameObject;
		gamePlayer = player;
		//Debug.Log ("Calling manageGoal");
		goalMan.manageGoal ();
		stationMan.manageStations ();
	
	}
	void Update()
	{
		if (counter >= maxRounds-1 && beginFade) 
		{
			goalMan.startCoroutine = true;
			beginFade = false;
		}

		if (counter >= maxRounds) 
		{
			StartCoroutine ("endGame");
		}

		if (Input.GetKeyDown (KeyCode.R))
			SceneManager.LoadScene (SceneManager.GetActiveScene ().buildIndex);
		if (Input.GetKeyDown (KeyCode.Escape))
			Application.Quit ();
		if (Input.GetKeyDown (KeyCode.Space))
			SceneManager.LoadScene (0);
			
	}
	IEnumerator endGame()
	{
		Camera.main.GetComponent<CameraBehavior> ().gameOver = true;
		Camera.main.GetComponent<CameraBehavior> ().zoomToStartingHeight (); //change zoom to the building clusters


		gamePlayer.GetComponent<playerMovement> ().gameOver = true;
		goalMan.StartCoroutine ("enableClusters");


		yield return new WaitForSeconds (3f);


		Camera.main.GetComponent<CameraBehavior> ().zoomOnClusters (goalMan.GetComponent<MoveClusters>().ClustersCenter); 

		//delete/disable player
		if (gamePlayer.activeInHierarchy)
			gamePlayer.SetActive(false);
		
		//delete/disable trainstations
		foreach (GameObject station in stationMan.stations) 
		{
			if (station.activeInHierarchy)
				station.SetActive(false);
		}
		//disable spawngoal
		goalMan.deSelectSpawnGoal();

		if (walls != null)
			walls.SetActive (false);
		
	}

	void selectSpawnPosition()
	{
		if (spawns.Length != 0) 
		{
			float rand = Random.Range (0, spawns.Length - 1);
			//Debug.Log ((int)rand + " is the randomly choosen index in array.");
			playerSpawn = spawns [(int)rand];
			GameObject spawn;

			for (int i = 0; i < spawns.Length; i++)
			{
				if (i != rand)
				{
					spawn = spawns [i];
					spawn.SetActive (false);
				}
			}
			//Debug.Log ("GameManager's playerSpawn is " + playerSpawn + " at location " + playerSpawn.transform.position);

		}
	}

	public void roundCounter(int count)
	{
		counter += count;
	}
}
