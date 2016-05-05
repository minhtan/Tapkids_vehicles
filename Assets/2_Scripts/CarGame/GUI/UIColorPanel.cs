using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIColorPanel : MonoBehaviour {
	private CanvasGroup mCanvasGroup;

	void OnEnable () {
		Messenger.AddListener <Vehicle> (EventManager.GUI.UPDATE_VEHICLE.ToString (), HandleUpdateVehicle);
	}

	void OnDisable () {
		Messenger.RemoveListener <Vehicle> (EventManager.GUI.UPDATE_VEHICLE.ToString (), HandleUpdateVehicle);
	}
	// Use this for initialization
	void Start () {
		mCanvasGroup = GetComponent <CanvasGroup> ();
	}

	void HandleUpdateVehicle (Vehicle _vehicle) {
		if (PlayerDataController.Instance.mPlayer.unlockedVehicles.Contains(_vehicle.id)) {
			mCanvasGroup.alpha = 1f;
			mCanvasGroup.interactable = true;
			mCanvasGroup.blocksRaycasts = true;
		} else {
			mCanvasGroup.alpha = 0f;
			mCanvasGroup.interactable = false;
			mCanvasGroup.blocksRaycasts = false;
		}
	}
}
