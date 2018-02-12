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


	void Start()
	{
		if (vAngles != null)
			vAngles.Clear();
		
		Vector3 test = DirectionFromAngle(-viewAngle/2, false);
		Vector3 test2 = DirectionFromAngle (viewAngle/2, false);

//		Debug.Log ("test  "+test);
//		Debug.Log ("test and test2  " + (test + test2));
//		Debug.Log ("test2  " +test2);
//
//		Debug.DrawRay (transform.position,  test, Color.yellow, 0, false); //test is left hand side endpoint
//		Debug.DrawRay (transform.position,  test+test+test2, Color.yellow, 0, false);		
//		Debug.DrawRay (transform.position,  test+test2, Color.yellow, 0, false); // halfway between test and test2
//		Debug.DrawRay (transform.position,  test+test2+test2, Color.yellow, 0, false);
//		Debug.DrawRay (transform.position,  test2, Color.yellow, 0, false);//test2 is right hand side endpoint


		vAngles.Add (test);
		vAngles.Add (test + test + test2);
		vAngles.Add (test + test2);
		vAngles.Add (test + test2 + test2);
		vAngles.Add (test2);

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
		Vector3 dir = transform.forward * viewRadius;




		if (Physics.Raycast(pos, dir, out hit, viewRadius))
		{
			Debug.Log ("Raycasted!");

			if(hit.collider.CompareTag("building"))
				hit.collider.gameObject.GetComponent<buildingMemory>().contacted();



			}
		}


}
