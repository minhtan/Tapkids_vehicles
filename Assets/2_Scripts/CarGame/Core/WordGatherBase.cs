using UnityEngine;
using System.Collections;

public class WordGatherBase : MonoBehaviour {
	// handle car gather word
	// TODO: make some effects to notify player when they collected correct word or wrong word

	void OnTriggerEnter () {
		Messenger.Broadcast (EventManager.Vehicle.GATHER_LETTER.ToString ());
	}
}
