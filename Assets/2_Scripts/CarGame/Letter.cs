using UnityEngine;
using System.Collections;

public class Letter : MonoBehaviour {

	public char letterName;

	void OnTriggerEnter (Collider other) {
		// collect letter

		// disable letter
		TrashMan.despawn(gameObject);
	}

}
