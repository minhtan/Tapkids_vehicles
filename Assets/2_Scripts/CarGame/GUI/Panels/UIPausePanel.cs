using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIPausePanel : MonoBehaviour {

	#region public members
	#endregion public members

	#region private members
	CanvasGroup mCanvasGroup;
	#endregion private members

	#region Mono
	void OnEnable () {
		Messenger.AddListener <bool> (EventManager.GameState.PAUSE.ToString (), HandlePauseGame);
	}
	void Disable () {
		Messenger.RemoveListener <bool> (EventManager.GameState.PAUSE.ToString (), HandlePauseGame);
	}

	void Start () {
		mCanvasGroup = GetComponent <CanvasGroup> ();
	}

	#endregion Mono

	#region public functions
	#endregion public functions

	#region private functions
	private void HandlePauseGame (bool _isPaused) {
//		Debug.Log ("Pause");
		mCanvasGroup.alpha = _isPaused ? 1f : 0f;
		mCanvasGroup.interactable = _isPaused ? true : false;
		mCanvasGroup.blocksRaycasts = _isPaused ? true : false;
	}

	#endregion private functions

}
