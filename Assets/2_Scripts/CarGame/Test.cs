using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Test : MonoBehaviour {
	Text mText;

	void Start () {
		mText = GetComponent <Text> ();
	}

	void FixedUpdate () {
		Debug.Log (SecondsToHhMmSs ((int)Time.time));
	}
	private string SecondsToHhMmSs(int seconds)
	{
//		return string.Format("{0:00}:{1:00}:{2:00}",seconds/3600,(seconds/60)%60,seconds%60);
		return string.Format("{0:00}:{1:00}", (seconds/60)%60, seconds%60);
	}
}
