﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Lean;

public class MainMenuController3D : MonoBehaviour {

	float minDrag = 10f;
	float holdTime;
	int swingTweenID;
	bool isMenuTweening = false;
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

	void OnEnable(){
		LeanTouch.OnFingerDown += OnFingerDown;
		LeanTouch.OnFingerTap += OnTap;
		LeanTouch.OnFingerDrag += OnDrag;
		LeanTouch.OnFingerUp += OnFingerUp;
	}

	void OnDisable(){
		LeanTouch.OnFingerDown -= OnFingerDown;
		LeanTouch.OnFingerTap -= OnTap;
		LeanTouch.OnFingerDrag -= OnDrag;
		LeanTouch.OnFingerUp -= OnFingerUp;
	}

	void Start () {
		Messenger.Broadcast <bool> (EventManager.GUI.TOGGLE_MENU_BTN.ToString (), false);
		Messenger.Broadcast <bool> (EventManager.GUI.TOGGLE_PLAYER_PNL.ToString (), true);
		Messenger.Broadcast <bool> (EventManager.GUI.TOGGLE_SFX_BTN.ToString (), true);

		SetFieldOfView ();
		menuPos = transform.localRotation.eulerAngles;
		totalD = garagePos.y - menuPos.y;
		SwingCam (-1f);
	}

	void Update(){
		ResizingBtns ();
	}

	void OnDestroy () {
		Messenger.Broadcast <bool> (EventManager.GUI.TOGGLE_SFX_BTN.ToString (), false);
		Messenger.Broadcast <bool> (EventManager.GUI.TOGGLE_PLAYER_PNL.ToString (), false);
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

	void SwingCam(float angle){
		Vector3 v = new Vector3(
			transform.localRotation.eulerAngles.x,
			transform.localRotation.eulerAngles.y + angle,
			transform.localRotation.eulerAngles.z
		);

		swingTweenID = LeanTween.rotateLocal (gameObject, v, 3f).setEase (LeanTweenType.easeInOutQuad).setLoopPingPong().uniqueId;
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
		holdTime = Time.time;
	}

	void OnFingerUp(LeanFinger fg){
		//here do raycast things
		if(isUILocked){
			return;
		}

		RaycastHit hitInfo;
		Ray ray = fg.GetRay ();
		if (Physics.Raycast (ray, out hitInfo)) {
			Messenger.Broadcast<int> (EventManager.GUI.MENU_BTN_UP.ToString (), hitInfo.collider.gameObject.GetInstanceID ());
		}

		//here do tween things
		if(isTweenLocked){
			return;
		}

		holdTime = Time.time - holdTime;
		if(holdTime < 0.5f){
			Swiped (drag);
		}else{
			Held ();
		}
	}

	void Swiped(float drag){
		if (Mathf.Abs (drag) < minDrag) {
			return;
		}

		LeanTween.cancel (swingTweenID);
		if(drag < 0){
			ToMenu (true);
		}else{
			ToGarage (true);
		}
	}

	void Held(){
		float anglePercentage = (transform.localRotation.eulerAngles.y - menuPos.y) / (garagePos.y - menuPos.y);
		if (anglePercentage > 0.5f) {
			ToGarage (true);
		} else {
			ToMenu (true);
		}
	}

	void OnDrag(LeanFinger fg){
		//here do raycast things
		if (isUILocked) {
			return;
		}

		RaycastHit hitInfo;
		Ray ray = fg.GetRay ();
		if (Physics.Raycast (ray, out hitInfo)) {
			Messenger.Broadcast<int> (EventManager.GUI.MENU_BTN_HOLD.ToString (), hitInfo.collider.gameObject.GetInstanceID ());
		} else {
			Messenger.Broadcast<int> (EventManager.GUI.MENU_BTN_HOLD.ToString (), 0);
		}

		//here do tween things
		if(isTweenLocked){
			return;
		}

		drag += fg.DeltaScreenPosition.x;
		if(Mathf.Abs(drag) > minDrag){
			LeanTween.cancel (swingTweenID);
			LeanTween.rotateAroundLocal (gameObject, Vector3.up, fg.DeltaScreenPosition.x * 0.03f, 0);
		}
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
		
	void ToMenu(bool ovrd = false){
		if (isMenuTweening)
			return;
		
		if (!isInMenu || ovrd) {
			isMenuTweening = true;
			LeanTween.rotateLocal (gameObject, menuPos, menuTweenTime).setOnComplete(() => {
				SwingCam(-1f);
				isMenuTweening = false;
			});
			Messenger.Broadcast (EventManager.GUI.TO_MENU.ToString());
			isInMenu = true;
		}
	}

	void ToGarage(bool ovrd = false){
		if (isMenuTweening)
			return;

		if (isInMenu || ovrd) {
			isMenuTweening = true;
			LeanTween.rotateLocal (gameObject, garagePos, menuTweenTime).setOnComplete(() => {
				SwingCam(1f);
				isMenuTweening = false;
			});
			Messenger.Broadcast (EventManager.GUI.TO_GARAGE.ToString());
			isInMenu = false;
		}
	}
}
