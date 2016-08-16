using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UIGameTimeText : MonoBehaviour {

	private Text mText;
	private	float countDownTimer = 3f;
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
	}
	void Update (){
		if (flag) {
			mText.text = (countDownTimer - Time.deltaTime).ToString("0f");
		}
	}

	#endregion MONO

	#region private methods
	private void HandleInitGame (string _firstLetter, string _givenLetters){
		flag = true;
	}


	#endregion private methods


}
