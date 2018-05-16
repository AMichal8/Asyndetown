using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class recordPath : MonoBehaviour {

	List<Vector3> pathCorners = new List<Vector3>();

	playerMovement player;
	bool canRecordCorners = false;

	// Use this for initialization
	void Start () 
	{
		
		player = GetComponent<playerMovement> ();

		if (pathCorners != null)
			pathCorners.Clear ();
		
	}
	
	// Update is called once per frame
	void Update () 
	{
		

	}


	//THIS SCRIPT ONLY DRAWS THE LINE OF THE PLAYERS CURRENT PATH


//	bool agentReachedDestination()
//	{
//		var nav = GetComponent<NavMeshAgent> ();
//
//		float dist = nav.remainingDistance;
//		return (dist != Mathf.Infinity && nav.pathStatus == NavMeshPathStatus.PathComplete && nav.remainingDistance == 0);
//	}
//	void addCorners()
//	{
//		List<Vector3> tempCorners = new List<Vector3> ();
//		var nav = GetComponent<NavMeshAgent> ();
//		Vector3 corner = new Vector3 ();
//
//		var path = nav.path;
//		if(path.corners.Length > 2)
//		{
//			for (int i = 0; i < path.corners.Length; i++) 
//			{
//				corner = path.corners [i];
//				Debug.Log ("corner is : " + corner);
//				tempCorners.Add (corner); //should be adding a vector3 for each place the agent turns along the path
//
//			}
//		}
//
//		if (agentReachedDestination ())
//			pathCorners = tempCorners;
//	}

	//Visualize Path (to see, have player object selected in hierarchy)
	void OnDrawGizmosSelected()
	{
		
		var nav = GetComponent<NavMeshAgent> ();
		if (nav == null || nav.path == null)
			return;
		

		var line = this.GetComponent<LineRenderer> ();
		if (line == null) 
		{
			line = this.gameObject.AddComponent<LineRenderer> ();
			line.material = new Material (Shader.Find ("Sprites/Default")) { color = Color.yellow };
			line.startWidth = .5f;
			line.endWidth = .5f;
			line.startColor = Color.yellow;
			line.endColor = Color.yellow;
		}

		var path = nav.path;

		line.positionCount = path.corners.Length;

		for (int i = 0; i < path.corners.Length; i++)
			line.SetPosition (i, path.corners [i]);
	}

}
