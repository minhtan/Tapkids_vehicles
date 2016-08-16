﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UIGameTimeText : MonoBehaviour {

	private Text mText;
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
		countDownTimer = 3f;
	}
	void Update (){
		if (flag) {
			countDownTimer -= Time.deltaTime;
			if (countDownTimer > 0)
				mText.text = countDownTimer.ToString("f0");
		}
	}

	#endregion MONO

	#region private methods
	private void HandleInitGame (string _firstLetter, string _givenLetters){
		flag = true;
	}


	#endregion private methods


}
