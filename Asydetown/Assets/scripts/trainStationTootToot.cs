using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class trainStationTootToot : MonoBehaviour {
	
	public Animator anim;
	public StationManager StationMan;
	[SerializeField]
	int visits=0;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}


	void OnTriggerEnter(Collider other)
	{
		if (other.tag == "Player") 
		{
			visited ();
			anim.SetBool ("isApproached", true);
			other.GetComponent<playerMovement> ().atStation = true;
			StationMan.chooseStationDestination ();
			Debug.Log ("Player is at a station");
		}
	}
	void OnTriggerExit(Collider other)
	{
		if (other.tag == "Player") 
		{
			anim.SetBool("isApproached", false);
			other.GetComponent<playerMovement> ().atStation = false;
			StationMan.exitingStation ();


		}
	}
	public void visited()
	{
		visits++;
	}


}
