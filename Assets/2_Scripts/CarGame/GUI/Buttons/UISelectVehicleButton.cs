using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UISelectVehicleButton : MonoBehaviour {

	#region public members
	public int index = 1;
	#endregion public members

	#region private members
	private Button mButton;
	private MainMenuController3D menu;
	#endregion private members

	#region Mono
	void OnEnable () {
		Messenger.AddListener <int> (EventManager.GUI.MENU_BTN_TAP.ToString (), HandleButtonTap);
	}

	void Start () {
		menu = FindObjectOfType <MainMenuController3D> ();
	}
	void OnDisable () {
		Messenger.RemoveListener <int> (EventManager.GUI.MENU_BTN_TAP.ToString (), HandleButtonTap);
	}
	#endregion Mono

	#region public functions
	#endregion public functions

	#region private functions
	void HandleButtonTap (int _id) {
		if (_id == gameObject.GetInstanceID () && !menu.IsInMenu) {
			Messenger.Broadcast <int> (EventManager.GUI.SELECT_VEHICLE.ToString (), index);
			AudioManager.Instance.PlayAudio (AudioKey.UNIQUE_KEY.BUTTON_CLICK);
		}
	}
	#endregion private functions

}
