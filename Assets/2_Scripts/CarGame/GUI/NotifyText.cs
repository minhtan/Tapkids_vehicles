using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class NotifyText : MonoBehaviour {

	private Text mText;
	private CanvasGroup mCanvasGroup;
	void OnEnable () {
//		CarGameEventController.NotifyText += OnNotifyText;
		Messenger.AddListener <string, float> (EventManager.GUI.NOTIFY.ToString (), HandleNotifyText);
	}

	void OnDisable () {
		Messenger.RemoveListener <string, float> (EventManager.GUI.NOTIFY.ToString (), HandleNotifyText);
//		CarGameEventController.NotifyText -= OnNotifyText;
	}
	// Use this for initialization
	void Awake () {
		mText = GetComponent <Text> ();

		if (mText != null) {
			
		}

		mCanvasGroup = GetComponent <CanvasGroup> ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	private void HandleNotifyText (string _text, float time) {
		mText.text = _text;
		LeanTween.value (gameObject, 0f, 1f, 0f).setOnUpdate ((float alpha) => mCanvasGroup.alpha = alpha);
		LeanTween.delayedCall (gameObject, time, FadeOut);
	}

	private void FadeOut () {
		LeanTween.value (gameObject, 1f, 0f, 0f).setOnUpdate ((float alpha) => mCanvasGroup.alpha = alpha);
	}
}
