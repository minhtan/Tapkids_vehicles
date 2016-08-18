using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UIGameTimeText : MonoBehaviour {

	private Text mText;
	private CanvasGroup mCanvasGroup;
	bool flag = false;

	#region MONO
	void OnEnable () {
		Messenger.AddListener (EventManager.GUI.COUNTDOWN.ToString (), HandleCountDown);
		Messenger.AddListener <int> (EventManager.GameState.GAMEOVER.ToString (), HandleGameOver);
	}
	void OnDislabe () {
		Messenger.RemoveListener (EventManager.GUI.COUNTDOWN.ToString (), HandleCountDown);
		Messenger.RemoveListener <int> (EventManager.GameState.GAMEOVER.ToString (), HandleGameOver);
	}
	void Start () {
		mText = GetComponent <Text> ();
		mCanvasGroup = GetComponent <CanvasGroup> ();
	}
	void Update (){
		if (flag) {
			mText.text = SecondsToHhMmSs((int)Time.time);
			Debug.Log (SecondsToHhMmSs((int)Time.time));
		}
	}

	#endregion MONO

	#region private methods
	private void HandleCountDown (){
		flag = true;
		mCanvasGroup.alpha = 1f;
	}

	private void HandleGameOver (int _star) {
		flag = false;
	}
	private string SecondsToHhMmSs(int seconds)
	{
		return string.Format("{0:00}:{1:00}", (seconds/60)%60, seconds%60);
	}

	#endregion private methods


}
