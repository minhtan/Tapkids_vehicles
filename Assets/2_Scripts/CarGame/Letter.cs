using UnityEngine;
using System.Collections;

public class Letter : MonoBehaviour {

	public string name;

	void OnEnable () {
//		CarGameEventController.ResetGame += OnResetGame;
	}

	void Disable () {
//		CarGameEventController.ResetGame -= OnResetGame;
	}

//	void OnResetGame () {
//		if(gameObject.activeInHierarchy) 
//			TrashMan.despawn(gameObject);	
//	}

	void OnTriggerEnter (Collider other) {

		// disable letter
		TrashMan.despawn(gameObject);
		// collect letter
		CarGameEventController.OnCollectLetter (name);

	}

}
