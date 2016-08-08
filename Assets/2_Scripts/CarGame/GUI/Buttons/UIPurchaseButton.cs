using UnityEngine;
using UnityEngine.UI;
using System.Collections;


public class UIPurchaseButton : MonoBehaviour {
	public Sprite coin;
	private MainMenuController3D menu;
	private GaragaController garage;
	void OnEnable () {
		Messenger.AddListener <int> (EventManager.GUI.MENU_BTN_TAP.ToString (), OnButtonTap);
	}
	void Start () {
		menu = FindObjectOfType <MainMenuController3D> ();
		garage = FindObjectOfType <GaragaController> ();
	}
	void OnDisable () {
		Messenger.RemoveListener <int> (EventManager.GUI.MENU_BTN_TAP.ToString (), OnButtonTap);
	}
	public void OnButtonTap (int _id) {
		if (!menu.IsInMenu) {
			if (gameObject.GetInstanceID () == _id) {
				AudioManager.Instance.PlayAudio (AudioKey.UNIQUE_KEY.BUTTON_CLICK);
				menu.SetTweenLock (true);
				int cost = garage.vehicles[garage.currentSelectedIndex].GetComponent <ArcadeCarController> ().vehicle.costPoint;
				GUIController.Instance.OpenDialog ("Do you want to purchase this vehicle for " + cost + "  *  ?", coin)
					.AddButton ("No", 
						UIDialogButton.Anchor.CENTER, 0, -135, 
						delegate { 
							AudioManager.Instance.PlayAudio (AudioKey.UNIQUE_KEY.BUTTON_CLICK);
							menu.SetTweenLock (false);
						}
					)
					.AddButton ("Yes", UIDialogButton.Anchor.CENTER, 0, -25,
						delegate { 
							Messenger.Broadcast (EventManager.GUI.PURCHASE_VEHICLE.ToString ());
							AudioManager.Instance.PlayAudio (AudioKey.UNIQUE_KEY.BUTTON_CLICK);
							menu.SetTweenLock (false);
						}
					);
			}
		}
	}
}
