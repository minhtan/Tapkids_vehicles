using UnityEngine;
using UnityEngine.UI;
using System.Collections;


public class UISuggestionImage : MonoBehaviour {

	private Image mImage;

	void OnEnable () {
		Messenger.AddListener <string> (EventManager.GUI.SHOWSUGGESTION.ToString (), HandleLetterTracking);
	}

	void Start () {
		mImage = GetComponent <Image> ();
	}

	void OnDisable () {
		Messenger.RemoveListener <string> (EventManager.GUI.SHOWSUGGESTION.ToString (), HandleLetterTracking);
	}

	void HandleLetterTracking (string _letters) {
		mImage.sprite = DataUltility.GetLetterImage (_letters[0].ToString().ToLower ());
	}
}
