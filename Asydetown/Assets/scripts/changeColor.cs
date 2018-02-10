using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class changeColor : MonoBehaviour {

	Color originalColor;
	Color lerpedColor;

	public Color newColor;
	float originalTransparency;
	Renderer myRend;



	// Use this for initialization
	void Start () 
	{
		myRend = GetComponent<Renderer> ();
		originalColor = myRend.material.color;
		originalTransparency = myRend.material.GetFloat ("_Transparency");
	}
	

	public void changeObjColor(float transparency, float lerpAmount)
	{	
		//Color newColor = Color.yellow;
		lerpedColor = Color.Lerp (originalColor, newColor, lerpAmount);
		myRend.material.color = lerpedColor;
		myRend.material.SetFloat ("_Transparency", transparency);
	}

	public float getTransparency()
	{
		float a = myRend.material.GetFloat ("_Transparency");
		return a;

	}

	public void resetObjColor()
	{
		myRend.material.color = originalColor;
		myRend.material.SetFloat ("_Transparency", originalTransparency);
	}

//	void OnTriggerEnter(Collider other)
//	{
//		if (other.CompareTag ("Player")) 
//		{
//			changeObjColor (1f);
//		}
//	}
//	void OnTriggerExit(Collider other)
//	{
//		if (other.CompareTag ("Player")) 
//		{
//			resetObjColor();
//		}
//	}
}
