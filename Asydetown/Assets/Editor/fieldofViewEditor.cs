using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(fieldOfView))]

public class fieldofViewEditor : Editor {

	void OnSceneGUI()
	{
		fieldOfView fov = (fieldOfView)target;
		Handles.color = Color.white;
		Handles.DrawWireArc (fov.transform.position, Vector3.up, Vector3.forward, 360, fov.viewRadius);
		Vector3 viewAngleA = fov.DirectionFromAngle (-fov.viewAngle / 2, false);
		Vector3 viewAngleB = fov.DirectionFromAngle (fov.viewAngle / 2, false);

		Handles.DrawLine (fov.transform.position, fov.transform.position + viewAngleA * fov.viewRadius);
		Handles.DrawLine (fov.transform.position, fov.transform.position + viewAngleB * fov.viewRadius);



	}
}
