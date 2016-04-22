using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class CollectedPanel : MonoBehaviour {
	public GameObject collectedTextPrefab;
	 
	private List <GameObject> collectedLetters = new List<GameObject> ();

	private Transform mTransform;
	void OnEnable () {
		Messenger.AddListener <string> (EventManager.GameState.INITGAME.ToString (), HandleInitGame);
		Messenger.AddListener <string> (EventManager.GUI.ADDLETTER.ToString (), HandleAddLetter);
		Messenger.AddListener <string> (EventManager.GUI.REMOVELETTER.ToString (), HandleRemoveLetter);

		// TODO: handle drop text
	}

	void Disable () {
		Messenger.RemoveListener <string> (EventManager.GameState.INITGAME.ToString (), HandleInitGame);
		Messenger.RemoveListener <string> (EventManager.GUI.ADDLETTER.ToString (), HandleAddLetter);
		Messenger.RemoveListener <string> (EventManager.GUI.REMOVELETTER.ToString (), HandleRemoveLetter);
	}

	void Start () {
		mTransform = GetComponent <Transform> ();
	}

	void HandleInitGame (string _letters) {
		if (collectedTextPrefab == null) return;

		// TODO: convert this to pool 
		for (int i = 0; i < collectedLetters.Count; i++) {
			Destroy(collectedLetters[i]);
		}
		collectedLetters.Clear ();

		// create letter on gui
		if (_letters.Length > 0) {
			for (int i = 0; i < _letters.Length; i++ ) {
				GameObject letter = Instantiate (collectedTextPrefab) as GameObject;
				letter.transform.SetParent (mTransform, false);
				collectedLetters.Add (letter);
			}
		}
	}

	void HandleAddLetter (string _letter) {
		for (int i = 0; i < collectedLetters.Count; i++) {
			if (string.IsNullOrEmpty (collectedLetters [i].GetComponentInChildren <Text> ().text)) {
				collectedLetters [i].GetComponentInChildren <Text> ().text = _letter;
				return;
			}
		}
	}

	void HandleRemoveLetter (string _letter) {
		for (int i = 0; i < collectedLetters.Count; i++) {
			if (collectedLetters [i].GetComponentInChildren <Text> ().text == _letter) {
				collectedLetters [i].GetComponentInChildren <Text> ().text = "";
				return;
			}
		}

	}
}
