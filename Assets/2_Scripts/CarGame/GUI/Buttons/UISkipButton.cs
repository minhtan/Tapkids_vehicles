using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UISkipButton : MonoBehaviour {

	private Button mButton;

	void Start () {
		mButton = GetComponent <Button> ();
		if (mButton != null) {
			mButton.onClick.AddListener (delegate {
				Messenger.Broadcast (EventManager.GUI.SKIP_TUT_BTN.ToString ());				
			});
		}
	}
}
