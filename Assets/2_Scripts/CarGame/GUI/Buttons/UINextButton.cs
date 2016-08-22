using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UINextButton : MonoBehaviour {

	private Button mButton;
	private CanvasGroup mCanvasGroup;


	void OnEnable () {
		Messenger.AddListener <string> (EventManager.GUI.SHOWSUGGESTION.ToString (), ShowNextButton);
	}

	void OnDisable () {
		Messenger.RemoveListener <string> (EventManager.GUI.SHOWSUGGESTION.ToString (), ShowNextButton);
	}

	void Start () {
		mCanvasGroup = GetComponent <CanvasGroup> ();
		mButton = GetComponent <Button> ();
		if (mButton != null) {
			mButton.onClick.AddListener (delegate {
				Messenger.Broadcast (EventManager.GUI.NEXTBUTTON.ToString ());	
				AudioManager.Instance.PlayAudio (AudioKey.UNIQUE_KEY.BUTTON_CLICK);
			});
		}
//		Invoke ("ShowNextButton", 3f);
	}

	void ShowNextButton (string _) {
		LeanTween.value (gameObject, 0f, 1f, 5f)
			.setOnUpdate ((float alpha) => mCanvasGroup.alpha = alpha)
			.setOnComplete (() => {
				mCanvasGroup.interactable = true;
				mCanvasGroup.blocksRaycasts =true;
		});
	}
}
