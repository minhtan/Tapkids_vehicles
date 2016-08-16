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
		Messenger.AddListener <string, string> (EventManager.GameState.INIT.ToString (), HandleInitGame);
	}
	void OnDislabe () {
		Messenger.RemoveListener <string, string> (EventManager.GameState.INIT.ToString (), HandleInitGame);
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
//				gameObject.SetActive(false);
			}
		}
	}

	#endregion MONO

	#region private methods
	private void HandleInitGame (string _firstLetter, string _givenLetters){
		flag = true;
		mCanvasGroup.alpha = 1f;
	}
	#endregion private methods
}
