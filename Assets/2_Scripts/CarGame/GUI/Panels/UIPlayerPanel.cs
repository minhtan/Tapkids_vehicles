using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIPlayerPanel : MonoBehaviour {

	#region private members
	private CanvasGroup mCanvasGroup;
	#endregion private members
	// Use this for initialization
	void Start () {
		mCanvasGroup = GetComponent <CanvasGroup> ();
	}
	void OnEnable () {
		Messenger.AddListener <bool> (EventManager.GUI.TOGGLE_PLAYER_PNL.ToString (), OnTogglePlayerPanel);
	}

	void OnDisable () {
		Messenger.AddListener <bool> (EventManager.GUI.TOGGLE_PLAYER_PNL.ToString (), OnTogglePlayerPanel);
	}

	void OnTogglePlayerPanel (bool _isToggled) {
		mCanvasGroup.alpha = _isToggled ? 1f : 0f;
		mCanvasGroup.interactable = _isToggled ? true : false;
		mCanvasGroup.blocksRaycasts = _isToggled ? true : false;
	}
}
