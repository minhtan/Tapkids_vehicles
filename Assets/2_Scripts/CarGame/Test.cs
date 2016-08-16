using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Test : MonoBehaviour {
	Text mText;
	float countDownTimer = 10f;
	float mTime;

	void Start () {
		mText = GetComponent <Text> ();

	}

	void Update () {
		countDownTimer -= Time.deltaTime;
		Debug.Log (countDownTimer.ToString("f0"));
	}

}
