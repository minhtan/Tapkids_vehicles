using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class NotifyText : MonoBehaviour {

	private Text mText;

	void OnEnable () {
//		CarGameEventController.NotifyText += OnNotifyText;
		Messenger.AddListener <string> (EventManager.GUI.NOTIFY.ToString (), OnNotifyText);
	}

	void OnDisable () {
		Messenger.RemoveListener <string> (EventManager.GUI.NOTIFY.ToString (), OnNotifyText);
//		CarGameEventController.NotifyText -= OnNotifyText;
	}
	// Use this for initialization
	void Start () {
		mText = GetComponent <Text> ();

		if (mText != null) {
			
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	private void OnNotifyText (string _text) {
		
		mText.text = _text;
		LeanTween.scale (gameObject, Vector3.one, 1f);
		LeanTween.delayedCall (gameObject, 1.5f, FadeOut);
	}

	private void FadeOut () {
		LeanTween.scale (gameObject, Vector3.zero, 1f);
	}
}
