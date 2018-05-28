using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class buildingAlignment : MonoBehaviour {

	public List<Vector3> pathPoints;
	public List<GameObject> activeBuildings;
	public int distanceFromPath = 7;
	public bool canStartAlignment=false;
	bool canMoveBuildings = false;

	public GameManager man;
	public Vector3 spawnGoalPos;
	public Vector3 buildingGoalPos;




	// Use this for initialization
	void Start () 
	{
		if (GameObject.FindGameObjectWithTag ("Player") == null)
			Debug.Log ("Alignment says: Player does not exist");
		spawnGoalPos = man.playerSpawn.transform.position;
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (canStartAlignment) 
		{
			StartCoroutine ("StartAlignment");
		}

//		if(canMoveBuildings)
//			MoveBuildings();


	}
	IEnumerator StartAlignment()
	{
		canStartAlignment = false;
		yield return new WaitForSeconds (2);
		GetPathCorners ();
		FindActiveBuildings ();
//		Align ();
		canMoveBuildings = true;
	}
	void GetPathCorners()
	{
		pathPoints = new List<Vector3> (GameObject.FindGameObjectWithTag ("Player").GetComponent<playerMovement> ().pathCorners);
		if (pathPoints == null)
			pathPoints = GameObject.FindGameObjectWithTag ("Player").GetComponent<playerMovement> ().pathCorners;
	}
	void FindActiveBuildings()
	{
		GameObject[] buildings = GameObject.FindGameObjectsWithTag("building");
		for (int j = 0; j < buildings.Length; j++) 
		{
			if (buildings [j].activeInHierarchy) 
			{
				if (buildings [j].GetComponent<buildingMemory> ().isGoal == false)
					activeBuildings.Add (buildings [j]);
				else
					buildingGoalPos = buildings [j].transform.position;
			}
		}
	}
//	void MoveBuildings()
//	{
//		Debug.Log ("Enter movebuildings");
//		Vector3 straightPath = (buildingGoalPos - spawnGoalPos).normalized;
//
//		float pathBuildingCountDistance = 1 / (float)activeBuildings.Count;
//		Debug.Log (activeBuildings.Count + " is the list count");
//		Debug.Log (pathBuildingCountDistance + " is the count distance");
//
//		float count = 1;
//
//		Debug.Log ("StraightPath is " + straightPath);
//
//		foreach (GameObject building in activeBuildings) 
//		{
//			Vector3 targetPos = spawnGoalPos + straightPath * pathBuildingCountDistance * count;
//			Debug.Log ("Building: " + building + " is moving towards position: " + targetPos);
//			
//			building.transform.position = Vector3.MoveTowards (building.transform.position, targetPos, 5 * Time.deltaTime);
//			Debug.Log ("Called movetowards");
//			count++;
//		}
//
//
//	}
//	void Align()
//	{
//		
//		for (int i = 0; i < pathPoints.Count - 1; i++) 
//		{
//			Vector3 point1 = pathPoints [i];
//			Vector3 point2 = pathPoints [i + 1];
//
//			Vector3 pathSegment = point2 - point1;
//			//get buildings?
//
//				//get active buildings array/list
//				//check position to pathsegment
//					//if posiion
//			foreach (GameObject building in activeBuildings) 
//			{
//				
//				Vector3 buildingPos = building.transform.position;
//				float distanceFromPoint1X = Mathf.Abs(point1.x-buildingPos.x);
//				float distanceFromPoint1Z = Mathf.Abs (point1.z - buildingPos.z);
//
//				float distanceFromPoint2X = Mathf.Abs(point2.x-buildingPos.x);
//				float distanceFromPoint2Z = Mathf.Abs (point2.z - buildingPos.z);
//
//				if ((distanceFromPoint1X <= distanceFromPath && distanceFromPoint1Z <= distanceFromPath) ||
//					(distanceFromPoint2X <= distanceFromPath && distanceFromPoint2Z <= distanceFromPath)) 
//				{
//					float sideOfPath = AngleDir (pathSegment, buildingPos, Vector3.up);
//					Debug.Log ("Building: " + building + " is on SideOfPath: " + sideOfPath);
//
//				}
//			}
//
//			//perpendicular?
//		}
//	}
	float AngleDir(Vector3 pathSeg, Vector3 targetPos, Vector3 up)
	{
		Vector3 perp = Vector3.Cross (pathSeg, targetPos);
		float dir = Vector3.Dot (perp, up);

		if (dir < 0)
			return -1;
		else if (dir > 0)
			return 1;
		else
			return 0;
	}
}
