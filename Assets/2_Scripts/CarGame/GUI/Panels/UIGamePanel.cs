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
		Messenger.AddListener <bool, Transform> (EventManager.AR.MAP_IMAGE_TRACKING.ToString (), HandleMapTracking);
	}

	void OnDisable () {
		Messenger.RemoveListener  <bool, Transform> (EventManager.AR.MAP_IMAGE_TRACKING.ToString (), HandleMapTracking);
	}

	void Start () {
		mCanvasGroup = GetComponent <CanvasGroup> ();
	}
	#endregion Mono

	#region public functions
	#endregion public functions

	#region private functions
	private void HandleMapTracking (bool _isFound, Transform transform) {
		if (mCanvasGroup == null) return;

		mCanvasGroup.alpha = _isFound ? 1f : 0f;
		mCanvasGroup.blocksRaycasts = _isFound ? true : false;
		mCanvasGroup.interactable = _isFound ? true : false;
	}
	#endregion private functions

}
