using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UINextButton : MonoBehaviour {

	private Button mButton;
	private CanvasGroup mCanvasGroup;

	void OnEnable () {
//		Invoke ("Sho4wNextButton", 2f);
	}

	void Start () {
		mCanvasGroup = GetComponent <CanvasGroup> ();
		mButton = GetComponent <Button> ();
		if (mButton != null) {
			mButton.onClick.AddListener (delegate {
				Messenger.Broadcast (EventManager.GUI.NEXT.ToString ());	
				AudioManager.Instance.PlayAudio (AudioKey.UNIQUE_KEY.BUTTON_CLICK);
			});
		}
	}

	void ShowNextButton () {
		LeanTween.value (gameObject, 0f, 1f, 1f)
			.setOnUpdate ((float alpha) => mCanvasGroup.alpha = alpha)
			.setOnComplete (() => {
				mCanvasGroup.interactable = true;
				mCanvasGroup.blocksRaycasts =true;
		});
	}
}
