using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIPurchaseButton : MonoBehaviour {

	private Button mButton;
	private CanvasGroup mCanvasGroup;

	void OnEnable () {
		Messenger.AddListener <Vehicle> (EventManager.GUI.UPDATEVEHICLE.ToString (), HandleUpdateVehicle);
	}

	void OnDisable () {
		Messenger.RemoveListener <Vehicle> (EventManager.GUI.UPDATEVEHICLE.ToString (), HandleUpdateVehicle);
	}
	void Start () {
		mCanvasGroup = GetComponent <CanvasGroup> ();
		mButton = GetComponent <Button> ();

		if (mButton != null) {
			mButton.onClick.AddListener (delegate {
				Messenger.Broadcast (EventManager.GUI.PURCHASEVEHICLE.ToString ());
			});
		}
	}

	void HandleUpdateVehicle (Vehicle _vehicle) {
		if (PlayerDataController.Instance.mPlayer.unlockedVehicles.Contains(_vehicle.id)) {
			mCanvasGroup.alpha = 0f;
			mCanvasGroup.interactable = false;
			mCanvasGroup.blocksRaycasts = false;
		} else {
			mCanvasGroup.alpha = 1f;
			mCanvasGroup.interactable = true;
			mCanvasGroup.blocksRaycasts = true;
		}
	}
}
