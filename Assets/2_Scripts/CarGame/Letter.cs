using UnityEngine;
using System.Collections;

public class Letter : MonoBehaviour {

	public string name;

	void OnTriggerEnter (Collider other) {
		// collect letter
		CarGameEventController.OnCollectLetter (name);
		// disable letter
		TrashMan.despawn(gameObject);


	}

}
