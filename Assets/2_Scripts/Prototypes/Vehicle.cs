using UnityEngine;
using System.Collections;

public class Vehicle : MonoBehaviour
{
	int currentPosition;	// -1, 0, 1


	// Use this for initialization
	void Start ()
	{
	
	}
	
	// Update is called once per frame
	void Update ()
	{
		// get horizontal input
		int hInput =  (int) Input.GetAxisRaw("Horizontal");
		switch (hInput) {
		case 0:
			break;
		case 1:
			TurnRight();
			break;
		case -1:
			TurnLeft();
			break;
		}
	}

	void TurnLeft ()
	{
		if (currentPosition != -1)
			currentPosition--;
		LeanTween.move(gameObject, new Vector3 (transform.position.x - 1f, transform.position.y, transform.position.z), .2f);
//		Debug.Log(currentPosition);
	}

	void TurnRight () 
	{
		if (currentPosition != 1)
			currentPosition++;
		LeanTween.move(gameObject, new Vector3 (transform.position.x + 1f, transform.position.y, transform.position.z), .2f);
//		Debug.Log(currentPosition);
	}

}
