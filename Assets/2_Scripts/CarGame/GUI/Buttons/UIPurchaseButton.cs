using UnityEngine;
using UnityEngine.UI;
using System.Collections;


public class UIPurchaseButton : MonoBehaviour {
	
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
				AudioManager.Instance.PlayAudio (AudioKey.UNIQUE_KEY.BUTTON_CLICK);
				GUIController.Instance.OpenDialog ("Purchase Confirm")
					.AddButton ("No", 
						UIDialogButton.Anchor.BOTTOM_RIGHT, 
						delegate { 
							AudioManager.Instance.PlayAudio (AudioKey.UNIQUE_KEY.BUTTON_CLICK);
						}
					)
					.AddButton ("Yes", UIDialogButton.Anchor.BOTTOM_LEFT,  
						delegate { 
							Messenger.Broadcast (EventManager.GUI.PURCHASE_VEHICLE.ToString ());
							AudioManager.Instance.PlayAudio (AudioKey.UNIQUE_KEY.BUTTON_CLICK);
						}
					);
			}
		}
	}
}
