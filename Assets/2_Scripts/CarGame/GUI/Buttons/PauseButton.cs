using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PauseButton : MonoBehaviour {

	#region public members
	#endregion public members

	#region private members
	private CanvasGroup mCanvasGroup;
	private Button pauseButton;
	#endregion private members

	#region Mono
	void OnEnable () {
		Messenger.AddListener <bool> (EventManager.GUI.TOGGLE_INGAME.ToString (), OnToggleInGamePanel);
	}

	void Start () {
		mCanvasGroup = GetComponent <CanvasGroup> ();
		pauseButton = GetComponent <Button> ();
		if(pauseButton != null) {
			pauseButton.onClick.AddListener (delegate {
				Messenger.Broadcast<bool> (EventManager.GameState.PAUSE.ToString(), true);
			});
		}
	}


	void OnDisable () {
		Messenger.RemoveListener <bool> (EventManager.GUI.TOGGLE_INGAME.ToString (), OnToggleInGamePanel);
	}

	#endregion Mono

	#region public functions
	#endregion public functions

	#region private functions
	private void OnToggleInGamePanel (bool _isToggled) {
		mCanvasGroup.alpha = _isToggled ? 1f : 0f;
		mCanvasGroup.interactable = _isToggled ? true : false;
		mCanvasGroup.blocksRaycasts = _isToggled ? true : false;
	}
	#endregion private functions

}
