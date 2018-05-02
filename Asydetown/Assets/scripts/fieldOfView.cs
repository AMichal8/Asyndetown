using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fieldOfView : MonoBehaviour {


	public float viewRadius;
	[Range(0,360)]
	public float viewAngle;

	public LayerMask viewMask;

	public List<Transform> visibleTargets = new List<Transform> ();

	public MeshFilter viewMeshFilter;
	Mesh viewMesh;


	List<Vector3> vAngles = new List<Vector3>();

	buildingMemory currentBuildingMid;
	buildingMemory currentBuildingLeftMid;
	buildingMemory currentBuildingLeft;
	buildingMemory currentBuildingRightMid;
	buildingMemory currentBuildingRight;

	[Range(0, 1)]
	public float meshResolution = .5f;

	void Start()
	{
		viewMesh = new Mesh ();
		viewMesh.name = "ViewMesh";
		viewMeshFilter.mesh = viewMesh;

		if (vAngles != null)
			vAngles.Clear();
		


	}

	void Update()
	{
		DrawFieldOfView ();
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

	ViewCastInfo ViewCast(float globalAngle)
	{
		Vector3 dir = DirectionFromAngle (globalAngle, true);
		RaycastHit hit;
		if (Physics.Raycast (transform.position, dir, out hit, viewRadius)) 
		{
			return new ViewCastInfo (true, hit.point, hit.distance, globalAngle);
		} 
		else 
		{
			return new ViewCastInfo (false, transform.position +dir * viewRadius,viewRadius, globalAngle);
		}
	}

	void DrawFieldOfView()
	{
		int rayCount = Mathf.RoundToInt(viewAngle * meshResolution);
		float rayAngleSize = viewAngle / rayCount;
		List<Vector3> viewPoints = new List<Vector3> ();

		for (int i = 0; i <= rayCount; i++) 
		{
			float angle = transform.eulerAngles.y - viewAngle/2 + rayAngleSize * i;
			ViewCastInfo newViewCast = ViewCast (angle);
			viewPoints.Add (newViewCast.point);
		}

		int vertexCount = viewPoints.Count + 1;
		Vector3[] vertices = new Vector3[vertexCount];
		int[] triangles = new int[(vertexCount - 2) * 3];

		vertices [0] = Vector3.zero;
		for (int i = 0; i < vertexCount - 1; i++) 
		{
			vertices [i + 1] = transform.InverseTransformPoint(viewPoints [i]);
			if (i < vertexCount - 2) 
			{
				
				triangles [i * 3] = 0;
				triangles [i * 3 + 1] = i + 1;
				triangles [i * 3 + 2] = i + 2;
			}


		}

		viewMesh.Clear ();
		viewMesh.vertices = vertices;
		viewMesh.triangles = triangles;
		viewMesh.RecalculateNormals ();
	}

	public struct ViewCastInfo{
		public bool hit;
		public Vector3 point;
		public float distance;
		public float angle;

		public ViewCastInfo(bool _hit, Vector3 _point, float _dst, float _angle)
		{
			hit = _hit;
			point = _point;
			distance = _dst;
			angle = _angle;
		}
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
