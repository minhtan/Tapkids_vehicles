using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIInputPanel : MonoBehaviour {

	private CanvasGroup mCanvasGroup;


	void OnEnable () {
		Messenger.AddListener <bool> (EventManager.GameState.START.ToString (), HandleStartGame);
	}

	void OnDisable () {
		Messenger.RemoveListener <bool> (EventManager.GameState.START.ToString (), HandleStartGame);

	}
	// Use this for initialization
	void Start () {
		mCanvasGroup = GetComponent <CanvasGroup> ();
	}

	void HandleStartGame (bool state) {
		mCanvasGroup.alpha = state ? 1f : 0f;
		mCanvasGroup.blocksRaycasts = state ? true : false;
		mCanvasGroup.interactable = state ? true : false;
	}
}
