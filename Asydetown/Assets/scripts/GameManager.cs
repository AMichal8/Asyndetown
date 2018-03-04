using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

	GameObject[] spawns;
	public int maxRounds = 3;

	public GameObject playerPrefab;
	public GameObject playerSpawn;
	int counter = 0;

	// Use this for initialization
	void Start () 
	{
		if (spawns == null || spawns.Length == 0)
			spawns = GameObject.FindGameObjectsWithTag ("spawn");

		selectSpawnPosition ();

		GameObject player = Instantiate (playerPrefab, playerSpawn.transform.position, transform.rotation) as GameObject;

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
		Camera.main.GetComponent<CameraBehavior> ().zoomToStartingHeight ();
		yield return new WaitForSeconds (1f);
		Time.timeScale = 0;
		
	}
	void selectSpawnPosition()
	{
		if (spawns.Length != 0) 
		{
			float rand = Random.Range (0, spawns.Length - 1);
			Debug.Log ((int)rand + " is the randomly choosen index in array.");
			playerSpawn = spawns [(int)rand];
		}
	}

	public void roundCounter(int count)
	{
		counter += count;
	}
}
