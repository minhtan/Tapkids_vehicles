using UnityEngine;
using UnityEngine.UI;
using System.Collections;


public class UIPurchaseButton : MonoBehaviour {

//	private Button mButton;
//	private CanvasGroup mCanvasGroup;
//
//	void OnEnable () {
//		Messenger.AddListener <Vehicle> (EventManager.GUI.UPDATE_VEHICLE.ToString (), HandleUpdateVehicle);
//	}
//
//	void OnDisable () {
//		Messenger.RemoveListener <Vehicle> (EventManager.GUI.UPDATE_VEHICLE.ToString (), HandleUpdateVehicle);
//	}
//	void Start () {
//		mCanvasGroup = GetComponent <CanvasGroup> ();
//		mButton = GetComponent <Button> ();
//
//		if (mButton != null) {
//			mButton.onClick.AddListener (delegate {
//				GUIController.Instance.OpenDialog ("Purchase Confirm")
//					.AddButton ("Yes", UIDialogButton.Anchor.BOTTOM_LEFT, 
//						delegate { 
//							Debug.Log("Yes");
//							Messenger.Broadcast (EventManager.GUI.PURCHASE_VEHICLE.ToString ());
//						}
//					).
//					AddButton ("No", 
//						UIDialogButton.Anchor.BOTTOM_LEFT, 
//						delegate { 
//							Debug.Log("No");
//						}
//					);
//			});
//		}
//	}
//
//	void HandleUpdateVehicle (Vehicle _vehicle) {
//		if (PlayerDataController.Instance.unlockedIds.Contains(_vehicle.id)) {
//			mCanvasGroup.alpha = 0f;
//			mCanvasGroup.interactable = false;
//			mCanvasGroup.blocksRaycasts = false;
//		} else {
//			mCanvasGroup.alpha = 1f;
//			mCanvasGroup.interactable = true;
//			mCanvasGroup.blocksRaycasts = true;
//		}
//	}

	private MainMenuController3D menu;
	void OnEnable () {
		Messenger.AddListener <int> (EventManager.GUI.MENU_BTN_TAP.ToString (), OnButtonTap);
	}
	void Start () {
		menu = FindObjectOfType <MainMenuController3D> ();
	}
	void OnDisable () {
		Messenger.RemoveListener <int> (EventManager.GUI.MENU_BTN_TAP.ToString (), OnButtonTap);
	}
	public void OnButtonTap (int _id) {
		if (!menu.IsInMenu) {
			if (gameObject.GetInstanceID () == _id) {
//				Debug.Log (" ????");
				GUIController.Instance.OpenDialog ("Purchase Confirm")
					.AddButton ("No", 
						UIDialogButton.Anchor.BOTTOM_RIGHT, 
						delegate { 
//							Debug.Log ("No");
						}
					)
					.AddButton ("Yes", UIDialogButton.Anchor.BOTTOM_LEFT,  
						delegate { 
							Messenger.Broadcast (EventManager.GUI.PURCHASE_VEHICLE.ToString ());
//							Debug.Log ("Yes");
						}
					);
			}
		}
	}
}
