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
		Messenger.AddListener <bool, Transform> (EventManager.GUI.TOGGLE_GAME_PNL.ToString (), HandleToggleGamePanel);
	}

	void OnDisable () {
		Messenger.RemoveListener  <bool, Transform> (EventManager.GUI.TOGGLE_GAME_PNL.ToString (), HandleToggleGamePanel);
	}

	void Start () {
		mCanvasGroup = GetComponent <CanvasGroup> ();
	}
	#endregion Mono

	#region public functions
	#endregion public functions

	#region private functions
	private void HandleToggleGamePanel (bool _isFound, Transform transform) {
		if (mCanvasGroup == null) return;

		mCanvasGroup.alpha = _isFound ? 1f : 0f;
		mCanvasGroup.blocksRaycasts = _isFound ? true : false;
		mCanvasGroup.interactable = _isFound ? true : false;
	}
	#endregion private functions

}
