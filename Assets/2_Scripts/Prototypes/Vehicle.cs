using UnityEngine;
using System.Collections;

public class Vehicle : MonoBehaviour
{
	int currentPosition;	// -3, 0, 3


	// Use this for initialization
	void Start ()
	{
	
	}
	
	// Update is called once per frame
	void Update ()
	{
		if(Input.GetKeyDown(KeyCode.A) && currentPosition != -3)
		{
//			LeanTween.moveLocalX(gameObject, transform.localPosition.x - 3f, .1f);
			transform.position = new Vector3(transform.localPosition.x - 3f, transform.localPosition.y,transform.localPosition.z);
			currentPosition = (int) transform.localPosition.x;
		}

		if(Input.GetKeyDown(KeyCode.D) && currentPosition != 3)
		{
//			LeanTween.moveLocalX(gameObject, transform.localPosition.x + 3f, .1f);
			transform.position = new Vector3(transform.localPosition.x + 3f, transform.localPosition.y,transform.localPosition.z);
			currentPosition = (int) transform.localPosition.x;
		}
	}



}
