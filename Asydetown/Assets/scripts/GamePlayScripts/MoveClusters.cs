using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveClusters : MonoBehaviour {

	GameObject cluster1;
	GameObject cluster2;

	Vector3 fromSpawnToGoal;
	Vector3 fromGoalToSpawn;
	public Vector3 ClustersCenter;

	Bounds clusterBounds1;
	Bounds clusterBounds2;

	public GoalManager goalMan;

	bool shouldMoveCluster;

	//New MoveCluster should be created when ready to us
	// Use this for initialization
	void Start () 
	{
		goalMan = GetComponent<GoalManager> ();
		//group buildings based on active, and proximity to the goals gotten via overlapsphere
		//parent those objects to 1 of 2 objects (1 for each goal)
		groupClusters();
		//set bounds for each parent and their children
		setClusterBounds();
		// move those parents to just outside of their bounds (to be done in update?)
		CalculateClusterDistance();
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(shouldMoveCluster)
			moveClusters ();
	}

	void groupClusters()
	{
		//create empty gameobject at goals' positions
		cluster1 = new GameObject("Cluster1");
		if (goalMan.goal == null)
			cluster1.transform.position = goalMan.previousGoal.transform.position;
		else
			cluster1.transform.position = goalMan.goal.transform.position;
		cluster2 = new GameObject ("Cluster2");
		cluster2.transform.position = goalMan.spawnGoal.transform.position;

		Vector3 fromGoaltoGoal = cluster1.transform.position - cluster2.transform.position;
		float fromGoaltoGoalDist = Mathf.Round(fromGoaltoGoal.magnitude);
		float dist = fromGoaltoGoalDist / 2;

		ClustersCenter = (fromGoaltoGoal.normalized * dist) + cluster2.transform.position;

		//Debug.Log (fromGoaltoGoalDist + " is the distance from goal to goal. And " + dist + " is half of that distance.");

		//get array of colliders via overlapsphere
		Collider[] hitColliders1 = Physics.OverlapSphere(cluster1.transform.position, dist);
		Collider[] hitColliders2 = Physics.OverlapSphere(cluster2.transform.position, dist);

		//get those colliders tranforms and parent them to parent transforms
		for (int i = 0; i < hitColliders1.Length; i++) 
		{
			if (hitColliders1 [i].gameObject.CompareTag ("building")) 
			{
				//hitColliders1 [i].gameObject.isStatic = false;
				hitColliders1 [i].transform.SetParent (cluster1.transform);
			}
				
		}
		for (int h = 0; h < hitColliders2.Length; h++) 
		{
			if (hitColliders2 [h].gameObject.CompareTag ("building")) 
			{
				//hitColliders2 [h].gameObject.isStatic = false;
				hitColliders2 [h].transform.SetParent (cluster2.transform);
			}
		}
	}
	void setClusterBounds()
	{
		clusterBounds1 = new Bounds (cluster1.transform.position, Vector3.zero);
		clusterBounds2 = new Bounds (cluster2.transform.position, Vector3.zero);

		foreach (Transform child in cluster1.transform) 
		{
			if (child != cluster1.transform) 
			{
				//Debug.Log ("The child is " + child.gameObject);
				clusterBounds1.Encapsulate (child.transform.position);
			}
		}

		foreach (Transform child in cluster2.transform) 
		{
			if (child != cluster2.transform) 
			{
				//Debug.Log ("The child is " + child.gameObject);
				clusterBounds2.Encapsulate (child.transform.position);
			}
		}

	

	}

	void CalculateClusterDistance()
	{
		//move one of the clusters towardes each other by their clusterDist
		fromSpawnToGoal = cluster1.transform.position - cluster2.transform.position;
		fromSpawnToGoal = new Vector3 (fromSpawnToGoal.x, 0, fromSpawnToGoal.z);
		fromGoalToSpawn = cluster2.transform.position - cluster1.transform.position;
		fromGoalToSpawn =  new Vector3 (fromGoalToSpawn.x, 0, fromGoalToSpawn.z);
		Debug.Log ((fromSpawnToGoal * .5f).ToString ());

		float fromSpawnToGoalMag = fromSpawnToGoal.magnitude;
		Debug.Log ("FromSpawnToGoal third magnitude: " + fromSpawnToGoalMag * .3f);
		float fromGoalToSpawnMag = fromGoalToSpawn.magnitude;
		Debug.Log ("FromGoalToSpawn third magnitude: " + fromGoalToSpawnMag * .3f);

		float cB1radius = clusterBounds1.extents.magnitude;
		Debug.Log ("Cluster1 Bounds Extents magnitude: " + cB1radius);
		float cB2radius = clusterBounds2.extents.magnitude;
		Debug.Log ("Cluster2 Bounds Extents magnitude: " + cB2radius);

		fromSpawnToGoal = fromSpawnToGoal.normalized * (fromSpawnToGoalMag*.5f + cB1radius) ;// sets the magnitude to half the original magnitude minus the extends magnitude
		//Debug.Log(fromSpawnToGoal + "is the S2G vector");
		fromSpawnToGoal += cluster2.transform.position;


		fromGoalToSpawn = fromGoalToSpawn.normalized * (fromGoalToSpawnMag*.5f + cB2radius) ;
		//Debug.Log(fromGoalToSpawn + "is the G2S vector");

		fromGoalToSpawn += cluster1.transform.position;

		if ((Mathf.Round(cB1radius) < fromSpawnToGoalMag * .3f) && (Mathf.Round(cB2radius) < fromSpawnToGoalMag * .3f))
			shouldMoveCluster = true;
		else
			shouldMoveCluster = false;


	}
	void moveClusters()
	{
		

		//close but not quite
		//try movetowards
		cluster1.transform.position = Vector3.MoveTowards(cluster1.transform.position, fromSpawnToGoal, 5*Time.deltaTime);
		cluster2.transform.position = Vector3.MoveTowards (cluster2.transform.position, fromGoalToSpawn, 5 * Time.deltaTime);
		//cluster1.transform.Translate(fromGoalToSpawn * Time.deltaTime);
		//cluster2.transform.Translate(fromGoalToSpawn * Time.deltaTime);

	}
}
