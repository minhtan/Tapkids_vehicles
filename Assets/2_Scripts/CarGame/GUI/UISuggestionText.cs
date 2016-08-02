using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

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

//		System.Random r = new System.Random();
//		int[] myValues;
//		IEnumerable<int> threeRandom = myValues.OrderBy(x => r.Next()).Take(3);

		for (int i = 0; i < _letters.Length; i++) {
			if (i % 2 == 0) {
				mText.text = string.Concat (mText.text, "_");
			} else {
				mText.text = string.Concat (mText.text, _letters[i]);
			}
		}
	}
}
