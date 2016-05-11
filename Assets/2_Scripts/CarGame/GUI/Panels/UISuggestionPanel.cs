using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UISuggestionPanel : MonoBehaviour {

	private CanvasGroup mCanvasGroup;

	void OnEnable () {
		Messenger.AddListener <string> (EventManager.GUI.SHOWSUGGESTION.ToString (), HandleLetterTracking);
		Messenger.AddListener (EventManager.GUI.NEXT.ToString (), HandleNextButton);
	}
	void Start () {
		mCanvasGroup = GetComponent <CanvasGroup> ();	
	}
	void OnDisable () {
		Messenger.RemoveListener <string> (EventManager.GUI.SHOWSUGGESTION.ToString (), HandleLetterTracking);
		Messenger.RemoveListener (EventManager.GUI.NEXT.ToString (), HandleNextButton);
	}
	void HandleLetterTracking (string _letters) {
		mCanvasGroup.alpha = 1f;
		mCanvasGroup.interactable = true;
		mCanvasGroup.blocksRaycasts = true;

	}

	void HandleNextButton () {
		mCanvasGroup.alpha = 0f;
		mCanvasGroup.interactable = false;
		mCanvasGroup.blocksRaycasts = false;

	}
}
