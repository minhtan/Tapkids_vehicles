using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class UICollectedPanel : MonoBehaviour {
	public GameObject collectedTextPrefab;
	 
	private List <GameObject> collectedLetters = new List<GameObject> ();

	private Transform mTransform;
	private Image mImage;
	void OnEnable () {
		Messenger.AddListener <string, string> (EventManager.GameState.INIT.ToString (), HandleInitGame);
		Messenger.AddListener <string> (EventManager.Vehicle.COLLECT_LETTER.ToString (), HandleCollectLetter);
		Messenger.AddListener <string> (EventManager.GUI.REMOVE_LETTER.ToString (), HandleRemoveLetter);
//		Messenger.AddListener (EventManager.GUI.CORRECTWORD.ToString (), HandleCorrectWord);
		// TODO: handle drop text
	}

	void Disable () {
		Messenger.RemoveListener <string, string> (EventManager.GameState.INIT.ToString (), HandleInitGame);
		Messenger.RemoveListener <string> (EventManager.Vehicle.COLLECT_LETTER.ToString (), HandleCollectLetter);
		Messenger.RemoveListener <string> (EventManager.GUI.REMOVE_LETTER.ToString (), HandleRemoveLetter);
//		Messenger.RemoveListener (EventManager.GUI.CORRECTWORD.ToString (), HandleCorrectWord);
	}

	void Awake () {
		mTransform = GetComponent <Transform> ();
		mImage = GetComponent <Image> ();
	}

	void HandleInitGame (string envLetter, string _letters) {
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

				collectedLetters.Add (letter);
			}
		}
	}

	void HandleCollectLetter (string _letter) {

		for (int i = 0; i < collectedLetters.Count; i++) {
			if (string.IsNullOrEmpty (collectedLetters [i].GetComponentInChildren <Text> ().text)) {
				collectedLetters [i].GetComponentInChildren <Text> ().text = _letter;
				collectedLetters [i].transform.SetParent (mTransform, false);
				if (!mImage.IsActive () && mTransform.childCount > 0)
					mImage.enabled = true;
				return;
			}
		}
	}

	void HandleRemoveLetter (string _letter) {
		for (int i = 0; i < collectedLetters.Count; i++) {
			if (collectedLetters [i].GetComponentInChildren <Text> ().text == _letter) {
				collectedLetters [i].GetComponentInChildren <Text> ().text = "";
				collectedLetters [i].transform.SetParent (null);
				if (mImage.IsActive () && mTransform.childCount <= 0) {
					mImage.enabled = false;
				}
				return;
			}
		}
	}

//	void HandleCorrectWord () {
//		for (int i = 0; i < collectedLetters.Count; i++) {
//			collectedLetters [i].GetComponentInChildren <Text> ().text = "";
//		}
//	}
}
