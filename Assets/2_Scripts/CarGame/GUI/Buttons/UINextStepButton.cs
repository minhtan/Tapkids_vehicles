using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UINextStepButton : MonoBehaviour {

	private Button mButton;

	void Start () {
		mButton = GetComponent <Button> ();
		if (mButton != null) {
			mButton.onClick.AddListener (delegate {
				Messenger.Broadcast <int> (EventManager.GUI.NEXT_STEP_BTN.ToString (), 1);				
			});
		}
	}
}
