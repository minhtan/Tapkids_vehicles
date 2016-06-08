using UnityEngine;
using UnityEngine.UI;

using System.Collections;

public class UIGatherButton : MonoBehaviour {
	private Button mButton;

	// Use this for initialization
	void Start () {
		mButton = GetComponent <Button> ();
		if (mButton != null) {
			mButton.onClick.AddListener (delegate {
				Messenger.Broadcast (EventManager.Vehicle.GATHER_LETTER.ToString ());	
			});
		}
	}
}
