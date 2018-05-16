using System.Collections;
using System.Collections.Generic;
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

	// Use this for initialization
	void Awake () 
	{
		if (spawns == null || spawns.Length == 0)
			spawns = GameObject.FindGameObjectsWithTag ("spawn");

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
		if (counter >= maxRounds) 
		{
			StartCoroutine ("endGame");
		}
	}
	IEnumerator endGame()
	{
		Camera.main.GetComponent<CameraBehavior> ().gameOver = true;
		Camera.main.GetComponent<CameraBehavior> ().zoomToStartingHeight ();

		gamePlayer.GetComponent<playerMovement> ().gameOver = true;
		yield return new WaitForSeconds (3f);
		Time.timeScale = 0;
		
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
