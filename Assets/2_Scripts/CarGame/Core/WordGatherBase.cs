using UnityEngine;
using System.Collections;

public class WordGatherBase : MonoBehaviour {

	// handle car gather word
	// TODO: make some effects to notify player when they collected correct word or wrong word

	void Start () {

	}

	void OnTriggerEnter () {
//		CarGameEventController.OnGatherLetter();
		Messenger.Broadcast (EventManager.Vehicle.GATHER.ToString ());
	}

	// check valid word here?
}
