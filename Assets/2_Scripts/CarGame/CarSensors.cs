using UnityEngine;
using System.Collections;

public class CarSensors : MonoBehaviour {

	public GameObject sensor;
//	Ray ray;

	// Use this for initialization
	void Start () {
//		ray = new Ray (sensor.transform.position, sensor.transform.forward);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	void OnDrawGizmos () {
		// Display the explosion radius when selected
		Gizmos.color = Color.red;
		Gizmos.DrawRay (sensor.transform.position, sensor.transform.forward); 
	}
}

