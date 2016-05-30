using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Lean;

public class MainMenuController3D : MonoBehaviour {

	bool isInMenu = true;
	public bool IsInMenu {
		get {
			return isInMenu;
		}
	}

	Vector3 menuPos;
	public Vector3 garagePos;
	float menuTweenTime = 0.2f;
	float drag;

	// Use this for initialization
	void Start () {
		Messenger.Broadcast <bool> (EventManager.GUI.TOGGLE_MENU_BTN.ToString (), false);
		SetFieldOfView ();
		menuPos = transform.localRotation.eulerAngles;
	}

	void SetFieldOfView(){
		Camera cam = gameObject.GetComponentInChildren<Camera> ();
		float ratio = cam.aspect;
		if (ratio > 1.7f) {
			//16:9
		} else if (ratio >= 1.5f) {
			//3:2
			cam.fieldOfView = 70f;
		} else if (ratio > 1.3f) {
			//4:3
			cam.fieldOfView = 77f;
		} else {
			//others
			Debug.Log ("default");
		}
	}
	
	void OnEnable(){
		LeanTouch.OnFingerSwipe += OnFingerSwipe;
		LeanTouch.OnFingerTap += OnTap;
		LeanTouch.OnFingerDown += OnFingerDown;
		LeanTouch.OnFingerDrag += OnDrag;
		LeanTouch.OnFingerUp += OnFingerUp;
	}

	void OnDisable(){
		LeanTouch.OnFingerSwipe -= OnFingerSwipe;
		LeanTouch.OnFingerTap -= OnTap;
		LeanTouch.OnFingerDown -= OnFingerDown;
		LeanTouch.OnFingerDrag -= OnDrag;
		LeanTouch.OnFingerUp -= OnFingerUp;
	}

	void OnFingerDown(LeanFinger fg){
		drag = 0;
	}

	void OnDrag(LeanFinger fg){
		drag += fg.DeltaScreenPosition.x;
		LeanTween.rotateAroundLocal (gameObject, Vector3.up, fg.DeltaScreenPosition.x * 0.03f, 0);
	}

	void OnFingerUp(LeanFinger fg){
		if(drag <= -Screen.width/2){
			ToMenu (true);
		}else if(drag >= Screen.width/2){
			ToGarage (true);
		}else{
			SnapBack();
		}
	}

	void SnapBack(){
		if (isInMenu) {
			ToMenu (true);
		} else {
			ToGarage (true);
		}
	}

	void OnTap(LeanFinger fg){
		RaycastHit hitInfo;
		Ray ray = fg.GetRay ();

		if (Physics.Raycast (ray, out hitInfo) && !LeanTouch.GuiInUse) {
			Messenger.Broadcast<int> (EventManager.GUI.MENU_BTN_TAP.ToString (), hitInfo.collider.gameObject.GetInstanceID ());
		}
	}

	void OnFingerSwipe(LeanFinger finger){
		var swipe = finger.SwipeDelta;

		//swipe left
		if (swipe.x < -Mathf.Abs (swipe.y)) {
			ToMenu ();
		}

		//swipe right
		if (swipe.x > Mathf.Abs (swipe.y)) {
			ToGarage ();
		}
	}
		
	void ToMenu(bool ovrd = false){
		if (!isInMenu || ovrd) {
			LeanTween.rotateLocal (gameObject, menuPos, menuTweenTime);
			Messenger.Broadcast (EventManager.GUI.TO_MENU.ToString());
			isInMenu = true;
		}
	}

	void ToGarage(bool ovrd = false){
		if (isInMenu || ovrd) {
			LeanTween.rotateLocal (gameObject, garagePos, menuTweenTime);
			Messenger.Broadcast (EventManager.GUI.TO_GARAGE.ToString());
			isInMenu = false;
		}
	}
}
