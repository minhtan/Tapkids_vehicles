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
		Messenger.AddListener (EventManager.GUI.NEXTBUTTON.ToString (), HandleNextButton);
	}
	void OnDislabe () {
		Messenger.RemoveListener (EventManager.GUI.NEXTBUTTON.ToString (), HandleNextButton);
	}
	void Start () {
		mText = GetComponent <Text> ();
		mCanvasGroup = GetComponent <CanvasGroup> ();
		countDownTimer = 3f;
	}

	void Update (){
		if (flag) {
			countDownTimer -= Time.deltaTime;
			if (countDownTimer > 0)
				mText.text = countDownTimer.ToString("f0");
			else {
				flag = false;
				mCanvasGroup.alpha = 0f;
				Messenger.Broadcast (EventManager.GUI.COUNTDOWN.ToString ());
			}
		}
	}

	#endregion MONO

	#region private methods
	private void HandleNextButton (){
		flag = true;
		mCanvasGroup.alpha = 1f;
	}
	#endregion private methods
}
