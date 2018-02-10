using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fieldOfView : MonoBehaviour {


	public float viewRadius;
	[Range(0,360)]
	public float viewAngle;

	public LayerMask viewMask;

	public List<Transform> visibleTargets = new List<Transform> ();


	void Start()
	{


	}

	void Update()
	{
//		FindBuildings ();
	}



	public Vector3 DirectionFromAngle(float angle, bool angleisGlobal)
	{
		if (!angleisGlobal) 
		{
			angle += transform.eulerAngles.y;
		}
		return new Vector3 (Mathf.Sin (angle * Mathf.Deg2Rad), 0, Mathf.Cos (angle * Mathf.Deg2Rad));
	}
//
//	void FindBuildings()
//	{
//		Quaternion startingAngle = Quaternion.Euler(0, -(viewAngle / 2), 0);
//		Quaternion stepAngle = Quaternion.Euler (0,  (viewAngle*2)/24, 0);
//
//
//		RaycastHit hit;
//
//		Quaternion angle = transform.rotation * startingAngle;
//		Vector3 dir = angle * transform.forward;
//		Vector3 pos = transform.position;
//
//		for (int i = 0; i < 24; i++) 
//		{
//			
//			if (Physics.Raycast (pos, dir, out hit, viewMask)) 
//			{
//				Debug.DrawLine (pos, hit.point, Color.blue);
//
//			}
//
//			dir = stepAngle * dir;
//		}
//	}

}
