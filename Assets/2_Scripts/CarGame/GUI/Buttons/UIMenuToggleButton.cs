using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIMenuToggleButton : MonoBehaviour {

	#region public members
	#endregion public members

	#region private members
	private CanvasGroup mCanvasGroup;
	private Toggle mToggle;
	#endregion private members

	#region Mono
	void OnEnable () {
		Messenger.AddListener <bool> (EventManager.GUI.TOGGLE_MENU_BTN.ToString (), HandleToggleMenuBtn);
		Messenger.AddListener <bool> (EventManager.GameState.PAUSE.ToString (), HandlePauseGame);
	}

	void Start () {
		mCanvasGroup = GetComponent <CanvasGroup> ();
		mToggle = GetComponent <Toggle> ();

		if (mToggle != null) {
			mToggle.onValueChanged.AddListener (delegate {
				Messenger.Broadcast<bool> (EventManager.GameState.PAUSE.ToString(), mToggle.isOn);
			});
		}
	}

	void OnDisable () {
		Messenger.RemoveListener <bool> (EventManager.GUI.TOGGLE_MENU_BTN.ToString (), HandleToggleMenuBtn);
		Messenger.RemoveListener <bool> (EventManager.GameState.PAUSE.ToString (), HandlePauseGame);
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

	private void HandlePauseGame (bool _isPaused) {
		if (!_isPaused)
			mToggle.isOn = false;
	}
	#endregion private functions

}
