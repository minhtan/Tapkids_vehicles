using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIPreviousStepButton : MonoBehaviour {
	private Button mButton;

	void Start () {
		mButton = GetComponent <Button> ();
		if (mButton != null) {
			mButton.onClick.AddListener (delegate {
				Messenger.Broadcast <int> (EventManager.GUI.PREVIOUS_STEP_BTN.ToString (), -1);				
			});
		}
	}
}
