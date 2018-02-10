using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerMovement : MonoBehaviour {


	public float speed = 5f;
	Rigidbody rb;
	Vector3 movement;
	Vector3 mousePos;

	Vector3 targetPos;
	bool isMoving;

	// Use this for initialization
	void Start () 
	{

		rb = GetComponent<Rigidbody> ();

		targetPos = transform.position;
		isMoving = false;
	}

	void Update()
	{


	}

	void MovePlayer()
	{
		
		transform.LookAt (targetPos);

		Vector3 direction = (targetPos - transform.position).normalized;
		//Debug.Log (direction + " is direction.");

		rb.MovePosition (transform.position + direction * speed * Time.deltaTime);
		rb.velocity = Vector3.zero;

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
	}
	// Update is called once per frame
	void FixedUpdate () 
	{
		
		if (Input.GetMouseButton (0))
			SetTargetPosition ();

		if (isMoving)
			MovePlayer ();
		
		//movement = new Vector3(Input.GetAxis("Horizontal") * speed, 0f, Input.GetAxis("Vertical") * speed);
		//rb.velocity = movement;

		mousePos = Input.mousePosition;
		mousePos.z = 20;
		mousePos = Camera.main.ScreenToWorldPoint (mousePos);
		mousePos = new Vector3(mousePos.x, transform.position.y, mousePos.z);
		transform.LookAt (mousePos);
		//Debug.Log ("The mousePos is : " + mousePos);



	
	}
}
