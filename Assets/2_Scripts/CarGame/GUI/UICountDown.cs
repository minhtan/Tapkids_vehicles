using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class UICountDown : MonoBehaviour {
	
	private Text mText;
	private CanvasGroup mCanvasGroup;
	private	float countDownTimer;
	bool flag = false;

	#region MONO
	void OnEnable () {
		Messenger.AddListener (EventManager.GUI.NEXTBUTTON.ToString (), ActivateCountDown);
		Messenger.AddListener (EventManager.GUI.ACTIVATE_COUNT_DOWN.ToString (), ActivateCountDown);
	}
	void OnDisable () {
		Messenger.RemoveListener (EventManager.GUI.NEXTBUTTON.ToString (), ActivateCountDown);
		Messenger.RemoveListener (EventManager.GUI.ACTIVATE_COUNT_DOWN.ToString (), ActivateCountDown);
	}

	void Start () {
		mText = GetComponent <Text> ();
		mCanvasGroup = GetComponent <CanvasGroup> ();
	}

	void Update (){
		if (flag) {
			countDownTimer -= Time.deltaTime;
			if (countDownTimer > 0)
				mText.text = countDownTimer.ToString("f0");
			else {
				flag = false;
				mCanvasGroup.alpha = 0f;
				Messenger.Broadcast (EventManager.GUI.FINISH_COUNTDOWN.ToString ());
			}
		}
	}

	#endregion MONO

	#region private methods
	private void ActivateCountDown (){
		Debug.Log ("ActivateCountDown");
		flag = true;
		countDownTimer = 3f;
		if (mCanvasGroup != null) {
			mCanvasGroup.alpha = 1f;
		} else {
			mCanvasGroup = GetComponent <CanvasGroup> ();
			mCanvasGroup.alpha = 1f;
		}
		
	}
	#endregion private methods
}
