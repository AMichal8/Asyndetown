﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBehavior : MonoBehaviour {


	/*public*/ GameObject target;

	[SerializeField]
	private float smoothTime = .4f;

	private Vector3 velocity = Vector3.zero;

	/*public*/ float minZoom = 30f;
	/*public*/ float maxZoom = 120f;
	/*public*/ float zoomLimiter = 150f;

	public float offsetHeight = 20f;

	GameObject player;
	Vector3 playerPosition;
	Vector3 buildingPosition;
	Vector3 offset;
	private Camera mainCam;

	// Called on awake
	void Awake () 
	{
		player = GameObject.FindGameObjectWithTag ("Player");

		offset = new Vector3 (0, offsetHeight, 0);
		mainCam = Camera.main;
	}
	
	// Update is called once per frame
	void Update()
	{
		if (Input.GetKeyDown (KeyCode.UpArrow))
			offsetHeight += 5f;
		if (Input.GetKeyDown (KeyCode.DownArrow))
			offsetHeight -= 5f;
	}
	void LateUpdate () 
	{
		FollowPlayer ();

		//Move ();
		//Zoom ();
	}
	public void setTargetBuildingPos(Vector3 pos)
	{
		buildingPosition = pos;
	}
	void FollowPlayer()
	{
		playerPosition = player.transform.position;
		offset = new Vector3 (0, offsetHeight, 0);
		transform.position = Vector3.SmoothDamp (transform.position, playerPosition + offset, ref velocity, smoothTime);
	}
	void Move()
	{
		Vector3 center = GetCenterPosition ();

		transform.position = Vector3.SmoothDamp (transform.position, center, ref velocity, smoothTime);
	}
	void Zoom()
	{
		float newZoom = Mathf.Lerp (minZoom, maxZoom, GetBoundsWidth () / zoomLimiter);

		mainCam.fieldOfView = Mathf.Lerp(mainCam.fieldOfView, newZoom, Time.deltaTime);

	}

	float GetBoundsWidth()
	{
		
		var bounds = new Bounds (buildingPosition, Vector3.zero);

		bounds.Encapsulate (playerPosition);



		return bounds.size.x;
	}

	private Vector3 GetCenterPosition()
	{
		playerPosition = player.transform.position;

		if (playerPosition == Vector3.zero && buildingPosition == Vector3.zero)
			return this.transform.position;
		else if (playerPosition == Vector3.zero)
			return buildingPosition;
		else
			return playerPosition + Vector3.up * 10;



		var bounds = new Bounds (buildingPosition, Vector3.zero);

		bounds.Encapsulate (playerPosition);

		Vector3 center = new Vector3 (bounds.center.x, bounds.center.y, transform.position.z);

		return center;
	}
}