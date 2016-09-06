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
	Light light;

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
		light = GetComponentInChildren<Light> ();
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
	}

	public void SetArMaxStimTargets(int targetNums){
		VuforiaUnity.SetHint (Vuforia.VuforiaUnity.VuforiaHint.HINT_MAX_SIMULTANEOUS_IMAGE_TARGETS, targetNums);
	}

	public void ToggleTracker(bool state){
		if (state) {
			TrackerManager.Instance.GetTracker<ObjectTracker> ().Start ();
		} else {
			TrackerManager.Instance.GetTracker<ObjectTracker> ().Stop ();
		}
	}

	public void ToggleAR(bool state, bool toggleQR = false, bool toggleVuforia = true){
		if (toggleVuforia || state) {
			VuforiaBehaviour.Instance.enabled = state;
		}

		camera.enabled = state;

		if (toggleQR && state) {
			GetComponent<VuforiaScanner> ().enabled = state;
		} else {
			GetComponent<VuforiaScanner> ().enabled = false;
		}
	}

	public void ToggleLight(bool state){
		if(this.light != null && state != this.light.enabled){
			this.light.enabled = state;
		}	
	}

	IEnumerator StartQRScanner(bool state){
		while(!isVuforiaReady){
			yield return null;
		}

		GetComponent<VuforiaScanner> ().enabled = state;
	}

	public void SetCenterMode(bool isCameraCenter){
		if (isCameraCenter) {
			VuforiaBehaviour.Instance.SetWorldCenterMode (VuforiaBehaviour.WorldCenterMode.CAMERA);
		} else {
			VuforiaBehaviour.Instance.SetWorldCenterMode (VuforiaBehaviour.WorldCenterMode.FIRST_TARGET);
		}
	}
}
