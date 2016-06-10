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
		Messenger.RemoveListener <bool> (EventManager.GUI.TOGGLE_PLAYER_PNL.ToString (), OnTogglePlayerPanel);
	}

	void OnTogglePlayerPanel (bool _isOn) {
		mCanvasGroup.alpha = _isOn ? 1f : 0f;
		mCanvasGroup.interactable = _isOn ? true : false;
		mCanvasGroup.blocksRaycasts = _isOn ? true : false;
	}
}
