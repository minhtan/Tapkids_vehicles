using UnityEngine;
using System.Collections;

public class Obstacle : MonoBehaviour {

	public float speed = -.5f;
	
	// Update is called once per frame
	void Update () {
		transform.Translate (0f, 0f, speed);
	}

	void OnBecameInvisble () {
		TrashMan.despawn (gameObject);
	}
}
