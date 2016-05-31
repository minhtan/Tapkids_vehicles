using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Lean;

public class MainMenuController3D : MonoBehaviour {

	bool isTweenLocked = false;
	bool isUILocked = false;
	bool isInMenu = true;
	public bool IsInMenu {
		get {
			return isInMenu;
		}
	}

	public Vector3 garagePos;
	Vector3 menuPos;
	float menuTweenTime = 0.2f;
	float drag;

	public Transform menuBtns;
	public Transform garageBtns;
	float totalD;

	// Use this for initialization
	void Start () {
		Messenger.Broadcast <bool> (EventManager.GUI.TOGGLE_MENU_BTN.ToString (), false);
		SetFieldOfView ();
		menuPos = transform.localRotation.eulerAngles;
		totalD = garagePos.y - menuPos.y;
	}

	void Update(){
		ResizingBtns ();
	}

	void ResizingBtns(){
		float currentD = garagePos.y - transform.localRotation.eulerAngles.y;
		float menuBtnScale = Mathf.Clamp01( currentD / totalD );

		menuBtns.localScale = menuBtnScale.ToVector3 ();
		garageBtns.localScale = (1f - menuBtnScale).ToVector3 ();
	}

	public void SetTweenLock(bool state){
		isTweenLocked = state;
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
		LeanTouch.OnFingerDown += OnFingerDown;
		LeanTouch.OnFingerTap += OnTap;
		LeanTouch.OnFingerDrag += OnDrag;
		LeanTouch.OnFingerUp += OnFingerUp;
		LeanTouch.OnFingerHeldDown += OnFingerHeldDown;
	}

	void OnDisable(){
		LeanTouch.OnFingerSwipe -= OnFingerSwipe;
		LeanTouch.OnFingerDown -= OnFingerDown;
		LeanTouch.OnFingerTap -= OnTap;
		LeanTouch.OnFingerDrag -= OnDrag;
		LeanTouch.OnFingerUp -= OnFingerUp;
		LeanTouch.OnFingerHeldDown -= OnFingerHeldDown;
	}

	void OnFingerDown(LeanFinger fg){
		isUILocked = LeanTouch.GuiInUse;
		if(isUILocked){
			return;
		}

		RaycastHit hitInfo;
		Ray ray = fg.GetRay ();

		if (Physics.Raycast (ray, out hitInfo)) {
			Messenger.Broadcast<int> (EventManager.GUI.MENU_BTN_DOWN.ToString (), hitInfo.collider.gameObject.GetInstanceID ());
		}

		drag = 0;
	}

	void OnFingerUp(LeanFinger fg){
		Messenger.Broadcast (EventManager.GUI.MENU_BTN_UP.ToString ());
		if(drag <= -Screen.width/2){
			ToMenu (true);
		}else if(drag >= Screen.width/2){
			ToGarage (true);
		}else{
			SnapBack();
		}
	}

	void OnFingerHeldDown(LeanFinger fg){
		
	}

	void OnDrag(LeanFinger fg){
		if(isUILocked || isTweenLocked){
			return;
		}
		drag += fg.DeltaScreenPosition.x;
		LeanTween.rotateAroundLocal (gameObject, Vector3.up, fg.DeltaScreenPosition.x * 0.03f, 0);
	}

	void OnTap(LeanFinger fg){
		if(!isUILocked){
			RaycastHit hitInfo;
			Ray ray = fg.GetRay ();

			if (Physics.Raycast (ray, out hitInfo)) {
				Messenger.Broadcast<int> (EventManager.GUI.MENU_BTN_TAP.ToString (), hitInfo.collider.gameObject.GetInstanceID ());
			}
		}
	}

	void SnapBack(){
		if (isInMenu) {
			ToMenu (true);
		} else {
			ToGarage (true);
		}
	}

	void OnFingerSwipe(LeanFinger finger){
		if(isUILocked || isTweenLocked){
			return;
		}

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
