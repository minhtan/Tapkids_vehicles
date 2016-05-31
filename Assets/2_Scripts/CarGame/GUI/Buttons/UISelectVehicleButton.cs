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
	float initY;
	bool isPressed = false;
	#endregion private members

	#region Mono
	void OnEnable () {
		Messenger.AddListener <int> (EventManager.GUI.MENU_BTN_TAP.ToString (), HandleButtonTap);
		Messenger.AddListener <int> (EventManager.GUI.MENU_BTN_DOWN.ToString (), HandleBtnDown);
		Messenger.AddListener (EventManager.GUI.MENU_BTN_UP.ToString (), HandleBtnUp);
	}

	void Start () {
		menu = FindObjectOfType <MainMenuController3D> ();
		initY = transform.localPosition.y;
	}
	void OnDisable () {
		Messenger.RemoveListener <int> (EventManager.GUI.MENU_BTN_TAP.ToString (), HandleButtonTap);
	}
	#endregion Mono

	#region public functions
	#endregion public functions

	#region private functions
	void HandleBtnDown(int _id){
		if (_id == gameObject.GetInstanceID () && !menu.IsInMenu ) {
			LeanTween.moveLocalY (gameObject, -2*initY, 0.1f);
			isPressed = true;
		}
	}

	void HandleBtnUp(){
		if (isPressed) {
			LeanTween.moveLocalY (gameObject, initY, 0.1f);
			isPressed = false;
		}
	}

	void HandleButtonTap (int _id) {
		if (_id == gameObject.GetInstanceID () && !menu.IsInMenu) {
			Messenger.Broadcast <int> (EventManager.GUI.SELECT_VEHICLE.ToString (), index);
		}
	}
	#endregion private functions

}
