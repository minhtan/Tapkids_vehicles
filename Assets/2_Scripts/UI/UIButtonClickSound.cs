using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UIButtonClickSound : MonoBehaviour {

	// Use this for initialization
	void Start () {
		Button mButton = GetComponent <Button> ();
		if (mButton != null) {
			mButton.onClick.AddListener (delegate {
				AudioManager.Instance.PlayAudio(AudioKey.UNIQUE_KEY.BUTTON_CLICK);	
			});
		}
	}

}
