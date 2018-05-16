using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StationManager : MonoBehaviour {

	public List<GameObject> stations;
	GameObject player;
	bool clickOnStation = false;

	// Use this for initialization
	void Start () {
		
		
	}
	
	// Update is called once per frame
	void Update () 
	{
		//Debug.Log ("bool clickOnStation is " + clickOnStation);
		if (clickOnStation) 
		{
			if (Input.GetMouseButton (0))
				ClickOnStation ();
		}
	}

	public void manageStations()
	{
		player = GameObject.FindGameObjectWithTag ("Player");


		if (stations.Count == 0) 
		{
			foreach (GameObject station in GameObject.FindGameObjectsWithTag ("station")) 
			{
				stations.Add (station);
			}

		}
	}

	public void chooseStationDestination()
	{
		//zoom out
		ZoomOut();
		//click on station

		clickOnStation = true;
		
		//Fade In Out

	}

	void ClickOnStation()
	{
		
		Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
		RaycastHit hit;

			

		if (Physics.Raycast(ray, out hit, 200f))
		{
			//set clicked on station as destination
			if (hit.collider.CompareTag ("station")) 
			{
				Vector3 destinationLocation = hit.collider.gameObject.transform.position;
				//hit.collider.gameObject.GetComponent<trainStationTootToot> ().visited ();
				//arrive at station
				player.GetComponent<playerMovement> ().setNewPosition (destinationLocation);
			}
		}

		clickOnStation = false;

	}

	void ZoomOut()
	{
		Camera.main.GetComponent<CameraBehavior> ().setToStartingHeight ();
		Camera.main.GetComponent<CameraBehavior> ().gameOver = true;

	}
	public void exitingStation()
	{
		Camera.main.GetComponent<CameraBehavior> ().gameOver = false;
		clickOnStation = false;

	}

}
