using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UICountdownText : MonoBehaviour {

	float timeLeft = 3f;

	private Text mText;
	private bool isPaused;

	void Start () {
		mText = GetComponent <Text> ();
	}

	void Update()
	{
		if (!isPaused) {
			timeLeft -= Time.deltaTime;
			mText.text = "" + Mathf.RoundToInt(timeLeft);
			if(timeLeft < .5f)
			{
				isPaused = true;
				gameObject.SetActive (false);
			}
		}
	}

}
