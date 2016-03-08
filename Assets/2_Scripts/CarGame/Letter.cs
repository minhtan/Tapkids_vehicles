using UnityEngine;
using System.Collections;

public class Letter : MonoBehaviour {

	public string name;

	void OnTriggerEnter (Collider other) {

		Debug.Log ("_________");
		// disable letter
		TrashMan.despawn(gameObject);
		// collect letter
		CarGameEventController.OnCollectLetter (name);

	}

}
