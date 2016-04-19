using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class CollectedPanel : MonoBehaviour {
	public GameObject collectedTextPrefab;
	 
	private List <GameObject> collectedLetters = new List<GameObject> ();

	void OnEnable () {
		Messenger.AddListener <string> (EventManager.GameState.INITGAME.ToString (), HandleInitGame);
		//
		Messenger.AddListener <string> (EventManager.GUI.UPDATECOLLECTEDLETTER.ToString (), HandleUpdateCollectLetter);

		// TODO: handle drop text
	}

	void Disable () {
		Messenger.RemoveListener <string> (EventManager.GameState.INITGAME.ToString (), HandleInitGame);

		Messenger.RemoveListener <string> (EventManager.GUI.UPDATECOLLECTEDLETTER.ToString (), HandleUpdateCollectLetter);
	}

	void HandleInitGame (string _letters) {
		if (collectedTextPrefab == null) return;


		// convert this to pool 
		for (int i = 0; i < collectedLetters.Count; i++) {
			Destroy(collectedLetters[i]);
		}
		collectedLetters.Clear ();

		// create letter on gui
		if (_letters.Length > 0) {
			for (int i = 0; i < _letters.Length; i++ ) {
				GameObject letter = Instantiate (collectedTextPrefab) as GameObject;
				letter.transform.SetParent (transform);
				collectedLetters.Add (letter);
			}
		}
	}

	void HandleUpdateCollectLetter (string _letters) {
		for (int i = 0; i < collectedLetters.Count; i++) {
			collectedLetters [i].GetComponentInChildren <Text> ().text = "";
		}
		for (int i = 0; i < _letters.Length; i++) {
			collectedLetters [i].GetComponentInChildren <Text> ().text = _letters[i].ToString ();
		}
	}
}
