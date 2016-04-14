using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class CollectedPanel : MonoBehaviour {

	public GameObject collectedTextPrefab;
	 
	private List <GameObject> collectedTexts;

	void OnEnable () {
		Messenger.AddListener <string> (EventManager.GameState.INITGAME.ToString (), HandleInitGame);
		Messenger.AddListener <string> (EventManager.Vehicle.COLLECT.ToString (), HandleCollectText);

		// TODO: handle drop text
	}

	void Disable () {
		Messenger.RemoveListener <string> (EventManager.GameState.INITGAME.ToString (), HandleInitGame);
		Messenger.RemoveListener <string> (EventManager.Vehicle.COLLECT.ToString (), HandleCollectText);
	}

	void HandleInitGame (string _letters) {
		if (collectedTextPrefab == null) return;
		// create letter on gui
		if (_letters.Length > 0) {
			for (int i = 0; i < _letters.Length; i++ ) {
				GameObject letter = Instantiate (collectedTextPrefab) as GameObject;
				letter.transform.SetParent (transform);
				collectedTexts.Add (letter);
			}
		}
	}

	void HandleCollectText (string _letter) {
		
	}



}
