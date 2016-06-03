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
		Messenger.AddListener <int> (EventManager.GUI.MENU_BTN_DOWN.ToString (), HandleBtnDown);
		Messenger.AddListener <int> (EventManager.GUI.MENU_BTN_UP.ToString (), HandleBtnUp);
		Messenger.AddListener <int> (EventManager.GUI.MENU_BTN_HOLD.ToString (), HandleBtnHold);
	}

	void Start () {
		menu = FindObjectOfType <MainMenuController3D> ();
		initY = transform.localPosition.y;
	}
	void OnDisable () {
		Messenger.RemoveListener <int> (EventManager.GUI.MENU_BTN_DOWN.ToString (), HandleBtnDown);
		Messenger.RemoveListener <int> (EventManager.GUI.MENU_BTN_UP.ToString (), HandleBtnUp);
		Messenger.RemoveListener <int> (EventManager.GUI.MENU_BTN_HOLD.ToString (), HandleBtnHold);
	}
	#endregion Mono

	#region public functions
	#endregion public functions

	#region private functions
	void HandleBtnDown(int _id){
		if (_id == gameObject.GetInstanceID () && !menu.IsInMenu ) {
			LeanTween.moveLocalY (gameObject, -2.5f*initY, 0.1f);
			isPressed = true;
		}
	}

	void HandleBtnUp(int _id){
		if (isPressed) {
			LeanTween.moveLocalY (gameObject, initY, 0.1f);

			if (_id == gameObject.GetInstanceID () && !menu.IsInMenu && !menu.IsCamRotating()) {
				Messenger.Broadcast <int> (EventManager.GUI.SELECT_VEHICLE.ToString (), index);
			}

			isPressed = false;
		}
	}

	void HandleBtnHold(int _id){
		if (isPressed) {
			if (_id == gameObject.GetInstanceID () && !menu.IsInMenu) {
				LeanTween.moveLocalY (gameObject, -2.5f*initY, 0.1f);
			} else {
				LeanTween.moveLocalY (gameObject, initY, 0.1f);
			}
		}
	}
	#endregion private functions

}
