using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIFocusPanel : MonoBehaviour {

	private CanvasGroup mCanvasGroup;

	void OnEnable () {
		Messenger.AddListener <bool, Transform> (EventManager.AR.MAPTRACKING.ToString (), HandleMapTracking);
	}

	void OnDisable () {
		Messenger.RemoveListener  <bool, Transform> (EventManager.AR.MAPTRACKING.ToString (), HandleMapTracking);
	}
	void Awake () {
		mCanvasGroup = GetComponent <CanvasGroup> ();

	}
	
	private void HandleMapTracking (bool _isFound, Transform transform) {
		if (mCanvasGroup == null) return;

		mCanvasGroup.alpha = _isFound ? 0f : 1f;
	}
}
