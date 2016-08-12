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
		if (_letters == "excavator")
			mImage.sprite = DataUltility.GetVehicleImage (_letters[1].ToString().ToLower ());
		mImage.sprite = DataUltility.GetVehicleImage (_letters[0].ToString().ToLower ());
	}
}
