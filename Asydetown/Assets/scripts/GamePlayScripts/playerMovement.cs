﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;

public class playerMovement : MonoBehaviour {

	public bool inTransition = false;
	public bool atStation = false;
	public bool playerHasMoved = false;
	public bool gameOver = false;

	//used for calculating distance from camera to ground
	public GameObject ground;
	public Camera mainCam;

	public List<Vector3> pathCorners = new List<Vector3>();

	//used for movement
	public float speed = 5f;
	Rigidbody rb;
	Vector3 movement;
	Vector3 mousePos;

	Vector3 targetPos;
	bool isMoving;


	NavMeshAgent agent;




	// Use this for initialization
	void Start () 
	{
		agent = GetComponent<NavMeshAgent> ();
		pathCorners.Clear ();

		rb = GetComponent<Rigidbody> ();
		mainCam = Camera.main;
		ground = GameObject.FindGameObjectWithTag ("ground");
		targetPos = transform.position;
		isMoving = false;


	}
	public void setNewPosition(Vector3 newPos)
	{
		Vector3 offset = new Vector3 (-5f, 0, 0);
		targetPos = newPos + offset;
		//agent.updateRotation = false;
		agent.Warp (newPos);
		//agent.updateRotation = true;
		agent.SetDestination (targetPos);

	}
	void MovePlayer()
	{
		
		//transform.LookAt (targetPos); //Need to have this only play at setting of destination and lock look at to 180 degrees

		Vector3 direction = (targetPos - transform.position).normalized;
		//Debug.Log (direction + " is direction.");

		//rb.MovePosition (transform.position + direction * speed * Time.deltaTime);
		//rb.velocity = Vector3.zero;
		agent.speed = speed;
		//agent.destination = targetPos;
		agent.SetDestination (targetPos);


		if (Mathf.RoundToInt( transform.position.x) == Mathf.RoundToInt( targetPos.x) && Mathf.RoundToInt(transform.position.z) == Mathf.RoundToInt(targetPos.z)) 
		{

			rb.velocity = Vector3.zero;
			isMoving = false;
			Debug.Log ("Stopped moving.");
		}

	}
	void SetTargetPosition()
	{
		Plane plane = new Plane (Vector3.up, transform.position);
		Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
		float point = 0f;

		if (plane.Raycast (ray, out point))
			targetPos = ray.GetPoint (point);

		//Debug.Log (targetPos);
		isMoving = true;
		playerHasMoved = true;

		NavMeshPath path = agent.path;

		//ADDS CORNER LOCATION TO PATHCORNERS LIST
		for (int i = 0; i < path.corners.Length; i++) 
		{
			//Debug.Log ("The length of path.corners is: " + path.corners.Length);
			Vector3 corner = new Vector3( Mathf.Round(path.corners [i].x), Mathf.Round(path.corners [i].y), Mathf.Round(path.corners [i].z));
			if(pathCorners.Contains(corner))
			{
				//Debug.Log ("Already contains corner: " + corner);
			}
			else //need to only add corners if they have walked there...
				pathCorners.Add (corner);
		}



	}
	public bool getIsMoving()
	{
		bool movingStatus;
		movingStatus = isMoving;
		return movingStatus;
	}

	// Update is called once per frame
	void FixedUpdate () 
	{
		if (!gameOver) 
		{
			if (!inTransition)
			{
				if (!atStation) 
				{
					if (Input.GetMouseButton (0))
						SetTargetPosition ();

					if (isMoving)
						MovePlayer ();

					//movement = new Vector3(Input.GetAxis("Horizontal") * speed, 0f, Input.GetAxis("Vertical") * speed);
					//rb.velocity = movement;


					//Set LookAt here?
					mousePos = Input.mousePosition;


					mousePos.z = Vector3.Distance(mainCam.transform.position, ground.transform.position);
					mousePos = Camera.main.ScreenToWorldPoint (mousePos);

					mousePos = new Vector3(mousePos.x, transform.position.y, mousePos.z);
					//transform.LookAt (mousePos, Vector3.up);
					//Debug.Log ("The mousePos is : " + mousePos);
				}

			}

		}
//		else
//			Debug.Log ("PathCorners contains " + pathCorners.Count + " corners arrays");


	}
}
