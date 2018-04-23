using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;

public class playerMovement : MonoBehaviour {

	public bool inTransition = false;
	public bool playerHasMoved = false;
	public bool gameOver = false;

	//used for calculating distance from camera to ground
	public GameObject ground;
	public Camera mainCam;

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
		rb = GetComponent<Rigidbody> ();
		mainCam = Camera.main;
		ground = GameObject.FindGameObjectWithTag ("ground");
		targetPos = transform.position;
		isMoving = false;


	}

	void Update()
	{


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

	}
	// Update is called once per frame
	void FixedUpdate () 
	{
		if (!gameOver) 
		{
			if (!inTransition)
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
}
