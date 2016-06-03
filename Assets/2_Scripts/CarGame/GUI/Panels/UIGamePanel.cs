using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIGamePanel : MonoBehaviour {

	#region public members
	#endregion public members

	#region private members
	private CanvasGroup mCanvasGroup;
	#endregion private members

	#region Mono
	void OnEnable () {
//		Messenger.AddListener <bool> (EventManager.GUI.TOGGLE_INGAME.ToString (), OnToggleInGamePanel);
	}

	void OnDisable () {
//		Messenger.RemoveListener <bool> (EventManager.GUI.TOGGLE_INGAME.ToString (), OnToggleInGamePanel);
	}

	void Start () {
		mCanvasGroup = GetComponent <CanvasGroup> ();
	}
	#endregion Mono

	#region public functions
	#endregion public functions

	#region private functions
//	private void OnToggleInGamePanel (bool _isToggled) {
//		mCanvasGroup.alpha = _isToggled ? 1f : 0f;
//		mCanvasGroup.interactable = _isToggled ? true : false;
//		mCanvasGroup.blocksRaycasts = _isToggled ? true : false;
//	}
	#endregion private functions

}
