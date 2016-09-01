using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UIGameTimeText : MonoBehaviour {

	private Text mText;
	private CanvasGroup mCanvasGroup;
	bool flag = false;
	float startTime;
	#region MONO
	void OnEnable () {
		Messenger.AddListener (EventManager.GUI.FINISH_COUNTDOWN.ToString (), HandleCountDown);
		Messenger.AddListener <int> (EventManager.GameState.GAMEOVER.ToString (), HandleGameOver);
	}
	void OnDisable () {
		Messenger.RemoveListener (EventManager.GUI.FINISH_COUNTDOWN.ToString (), HandleCountDown);
		Messenger.RemoveListener <int> (EventManager.GameState.GAMEOVER.ToString (), HandleGameOver);
	}

	void Start () {
		mText = GetComponent <Text> ();
		mCanvasGroup = GetComponent <CanvasGroup> ();
	}
	void Update (){
		if (flag) {
			float t = Time.time - startTime;
			mText.text = SecondsToHhMmSs(t);
//			mText.text = SecondsToHhMmSs((int)t);
//			Debug.Log (SecondsToHhMmSs((int)t));
		}
	}

	#endregion MONO

	#region private methods
	private void HandleCountDown (){
		Debug.Log ("start game time");
		flag = true;
		startTime = Time.time;

		mCanvasGroup.alpha = 1f;
	}

	private void HandleGameOver (int _star) {
		flag = false;
	}
	private string SecondsToHhMmSs(int seconds)
	{
		return string.Format("{0:00}:{1:00}", (seconds/60)%60, seconds%60);
	}

	private string SecondsToHhMmSs(float t)
	{
		return ((int) t / 60).ToString() + ":" + (t % 60).ToString("f0");
	}
	#endregion private methods


}
