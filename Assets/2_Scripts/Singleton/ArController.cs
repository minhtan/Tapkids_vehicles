using UnityEngine;
using System.Collections;
using Vuforia;
using System.Collections.Generic;
using System.Linq;
using Lean;

public class ArController : UnitySingletonPersistent<ArController> {

	#region Vars
	bool isFocusing;
	bool isVuforiaReady = false;
	Camera camera;

	public bool IsVuforiaReady {
		get {
			return isVuforiaReady;
		}
	}
	#endregion

	#region Mono
	public override void Awake ()
	{
		base.Awake ();
		VuforiaBehaviour.Instance.RegisterVuforiaStartedCallback (OnVuforiaStarted);
		camera = GetComponentInChildren<Camera> ();
	}

	void Update(){
		#if UNITY_ANDROID || UNITY_EDITOR
		if(isFocusing){
//			CameraDevice.Instance.SetFocusMode(CameraDevice.FocusMode.FOCUS_MODE_CONTINUOUSAUTO);
		}
		#endif
	}

	void OnEnable(){
		LeanTouch.OnFingerTap += OnFingerTap;
		SceneController.OnEndLoading += OnEndLoading;
	}

	void OnDisable(){
		LeanTouch.OnFingerTap -= OnFingerTap;
		SceneController.OnEndLoading -= OnEndLoading;
	}
	#endregion

	void OnFingerTap(LeanFinger fg){
		if(!LeanTouch.GuiInUse && VuforiaBehaviour.Instance.enabled == true){
			isFocusing = !isFocusing;
		}
	}

	public void OnEndLoading(){
		isFocusing = false;
	}

	void OnVuforiaStarted(){
		ArController.Instance.ToggleAR (false);
		isVuforiaReady = true;
//		SceneController.Instance.LoadingSceneAsync (SceneController.SceneID.MENU);
	}

	public void SetArMaxStimTargets(int targetNums){
		VuforiaUnity.SetHint (Vuforia.VuforiaUnity.VuforiaHint.HINT_MAX_SIMULTANEOUS_IMAGE_TARGETS, targetNums);
	}

	public void ToggleAR(bool state, bool isQR = false){
		VuforiaBehaviour.Instance.enabled = state;
		camera.enabled = state;

		if (isQR && state) {
			GetComponent<VuforiaScanner> ().enabled = state;
		} else {
			GetComponent<VuforiaScanner> ().enabled = false;
		}
	}

	public void SetCenterMode(bool isCameraCenter){
		if (isCameraCenter) {
			VuforiaBehaviour.Instance.SetWorldCenterMode (VuforiaBehaviour.WorldCenterMode.CAMERA);
		} else {
			VuforiaBehaviour.Instance.SetWorldCenterMode (VuforiaBehaviour.WorldCenterMode.FIRST_TARGET);
		}
	}
}
