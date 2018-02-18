using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fieldOfView : MonoBehaviour {


	public float viewRadius;
	[Range(0,360)]
	public float viewAngle;

	public LayerMask viewMask;

	public List<Transform> visibleTargets = new List<Transform> ();

	List<Vector3> vAngles = new List<Vector3>();

	buildingMemory currentBuildingMid;
	buildingMemory currentBuildingLeftMid;
	buildingMemory currentBuildingLeft;
	buildingMemory currentBuildingRightMid;
	buildingMemory currentBuildingRight;


	void Start()
	{
		if (vAngles != null)
			vAngles.Clear();
		


	}

	void Update()
	{
		
		FindBuildings ();
	}

	public Vector3 DirectionFromAngle(float angle, bool angleisGlobal)
	{
		if (!angleisGlobal) 
		{
			angle += transform.eulerAngles.y;
		}
		return new Vector3 (Mathf.Sin (angle * Mathf.Deg2Rad), 0, Mathf.Cos (angle * Mathf.Deg2Rad));
	}

	void FindBuildings()
	{

		RaycastHit hit;


		Vector3 pos = transform.position;
		Vector3 mid = transform.forward * viewRadius;




		if (Physics.Raycast(pos, mid, out hit, viewRadius))
		{
			//Debug.Log ("Raycasted!");

			if (hit.collider.CompareTag ("building")) {

				if (currentBuildingMid == null) 
				{
					currentBuildingMid = hit.collider.gameObject.GetComponent<buildingMemory> ();
					currentBuildingMid.contacted ();

				} 
				else if (hit.collider.gameObject.GetComponent<buildingMemory> () == currentBuildingMid) 
				{
					currentBuildingMid.contacted ();
				} 
				else 
				{
					currentBuildingMid.uncontacted ();
					currentBuildingMid = null;
				}

			} 
				
		}

		Vector3 left = DirectionFromAngle(-viewAngle/2, false) * viewRadius;

		if (Physics.Raycast(pos, left, out hit, viewRadius))
		{
			//Debug.Log ("Raycasted!");

			if (hit.collider.CompareTag ("building")) {

				if (currentBuildingLeft == null) 
				{
					currentBuildingLeft = hit.collider.gameObject.GetComponent<buildingMemory> ();
					currentBuildingLeft.contacted ();

				} 
				else if (hit.collider.gameObject.GetComponent<buildingMemory> () == currentBuildingLeft) 
				{
					currentBuildingLeft.contacted ();
				} 
				else 
				{
					currentBuildingLeft.uncontacted ();
					currentBuildingLeft = null;
				}

			} 

		}



		Vector3 right = DirectionFromAngle (viewAngle/2, false) * viewRadius;

		if (Physics.Raycast(pos, right, out hit, viewRadius))
		{
			//Debug.Log ("Raycasted!");

			if (hit.collider.CompareTag ("building")) 
			{

				if (currentBuildingRight == null) 
				{
					currentBuildingRight = hit.collider.gameObject.GetComponent<buildingMemory> ();
					currentBuildingRight.contacted ();

				} 
				else if (hit.collider.gameObject.GetComponent<buildingMemory> () == currentBuildingRight) 
				{
					currentBuildingRight.contacted ();
				} 
				else 
				{
					currentBuildingRight.uncontacted ();
					currentBuildingRight = null;
				}

			} 

		}

		Vector3 leftMid = left + mid * viewRadius;

		if (Physics.Raycast(pos, leftMid, out hit, viewRadius))
		{
			//Debug.Log ("Raycasted!");

			if (hit.collider.CompareTag ("building")) {

				if (currentBuildingLeftMid == null) 
				{
					currentBuildingLeftMid = hit.collider.gameObject.GetComponent<buildingMemory> ();
					currentBuildingLeftMid.contacted ();

				} 
				else if (hit.collider.gameObject.GetComponent<buildingMemory> () == currentBuildingLeftMid) 
				{
					currentBuildingLeftMid.contacted ();
				} 
				else 
				{
					currentBuildingLeftMid.uncontacted ();
					currentBuildingLeftMid = null;
				}

			} 

		}



		Vector3 rightMid = right + mid * viewRadius;

		if (Physics.Raycast(pos, rightMid, out hit, viewRadius))
		{
			//Debug.Log ("Raycasted!");

			if (hit.collider.CompareTag ("building")) {

				if (currentBuildingRightMid == null) 
				{
					currentBuildingRightMid = hit.collider.gameObject.GetComponent<buildingMemory> ();
					currentBuildingRightMid.contacted ();

				} 
				else if (hit.collider.gameObject.GetComponent<buildingMemory> () == currentBuildingRightMid) 
				{
					currentBuildingRightMid.contacted ();
				} 
				else 
				{
					currentBuildingRightMid.uncontacted ();
					currentBuildingRightMid = null;
				}

			} 

		}




	}


}
