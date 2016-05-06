using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UISuggestionText : MonoBehaviour {

	private Text mText;

	void OnEnable () {
		Messenger.AddListener <string> (EventManager.GUI.SHOWSUGGESTION.ToString (), HandleLetterTracking);
	}
	void Start () {
		mText = GetComponent <Text> ();
	}

	void OnDisable () {
		Messenger.RemoveListener <string> (EventManager.GUI.SHOWSUGGESTION.ToString (), HandleLetterTracking);
	}
	void HandleLetterTracking (string _letters) {
		mText.text = "";
		for (int i = 0; i < _letters.Length; i++) {
			if (i % 2 == 0) {
				mText.text = string.Concat (mText.text, "_");
			} else {
				mText.text = string.Concat (mText.text, _letters[i]);
			}
		}
	}


}
