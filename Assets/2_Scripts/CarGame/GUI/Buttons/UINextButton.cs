using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UINextButton : MonoBehaviour {

	private Button mButton;
	// Use this for initialization
	void Start () {
		mButton = GetComponent <Button> ();
		if (mButton != null) {
			mButton.onClick.AddListener (delegate {
				Messenger.Broadcast (EventManager.GUI.NEXT.ToString ());	
				AudioManager.Instance.PlayAudio (AudioKey.UNIQUE_KEY.BUTTON_CLICK);
			});
		}
	}
}
