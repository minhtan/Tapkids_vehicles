using UnityEngine;
using System.Collections;

public class UISelectMaterialButton : MonoBehaviour {

	public int matId;
	public bool isOn;
	private MainMenuController3D menu;
	void OnEnable () {
		Messenger.AddListener <int> (EventManager.GUI.MENU_BTN_TAP.ToString (), OnButtonTap);
		Messenger.AddListener <int> (EventManager.GUI.CHANGE_MATERIAL.ToString (), HandleChangeMaterial);
	}
	void Start () {
		menu = FindObjectOfType <MainMenuController3D> ();
	}
	void OnDisable () {
		Messenger.RemoveListener <int> (EventManager.GUI.MENU_BTN_TAP.ToString (), OnButtonTap);
		Messenger.RemoveListener <int> (EventManager.GUI.CHANGE_MATERIAL.ToString (), HandleChangeMaterial);
	}
	public void OnButtonTap (int _id) {
		if (!menu.IsInMenu) {
			if (gameObject.GetInstanceID () == _id) {
				if (!isOn) {
					Messenger.Broadcast <int> (EventManager.GUI.CHANGE_MATERIAL.ToString (), matId);
					AudioManager.Instance.PlayAudio (AudioKey.UNIQUE_KEY.BUTTON_CLICK);
					isOn = true;
				}
			}
		}
	}

	void HandleChangeMaterial (int _matId) {
		if (_matId != matId) {
			isOn = false;
		}
	}


}
