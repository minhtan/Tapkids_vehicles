using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIMenuButton : MonoBehaviour {

	#region public members
	#endregion public members

	#region private members
	private CanvasGroup mCanvasGroup;
	private Button mButton;
	#endregion private members

	#region Mono
	void OnEnable () {
		Messenger.AddListener <bool> (EventManager.GUI.TOGGLE_MENU_BTN.ToString (), HandleToggleMenuBtn);
	}

	void Start () {
		mCanvasGroup = GetComponent <CanvasGroup> ();
		mButton = GetComponent <Button> ();

		if (mButton != null) {
			mButton.onClick.AddListener (delegate {
				Messenger.Broadcast<bool> (EventManager.GameState.PAUSE.ToString(), true);
			});
		}
	}

	void OnDisable () {
		Messenger.RemoveListener <bool> (EventManager.GUI.TOGGLE_MENU_BTN.ToString (), HandleToggleMenuBtn);
	}

	#endregion Mono

	#region public functions
	#endregion public functions

	#region private functions
	private void HandleToggleMenuBtn (bool _isToggled) {
		mCanvasGroup.alpha = _isToggled ? 1f : 0f;
		mCanvasGroup.interactable = _isToggled ? true : false;
		mCanvasGroup.blocksRaycasts = _isToggled ? true : false;
	}

	#endregion private functions

}
