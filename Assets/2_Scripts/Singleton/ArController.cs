using UnityEngine;
using System.Collections;
using Vuforia;
using System.Collections.Generic;
using System.Linq;

public class ArController : UnitySingletonPersistent<ArController> {

	#region Vars
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
	#endregion

	void OnVuforiaStarted(){
		ArController.Instance.ToggleAR (false);
		isVuforiaReady = true;
//		SceneController.Instance.LoadingSceneAsync (SceneController.SceneID.MENU);
	}

	public void SetArMaxStimTargets(int targetNums){
		VuforiaUnity.SetHint (Vuforia.VuforiaUnity.VuforiaHint.HINT_MAX_SIMULTANEOUS_IMAGE_TARGETS, targetNums);
	}

	public void ToggleAR(bool state){
		VuforiaBehaviour.Instance.enabled = state;
		camera.enabled = state;
	}

	public void SetCenterMode(bool isCameraCenter){
		if (isCameraCenter) {
			VuforiaBehaviour.Instance.SetWorldCenterMode (VuforiaBehaviour.WorldCenterMode.CAMERA);
		} else {
			VuforiaBehaviour.Instance.SetWorldCenterMode (VuforiaBehaviour.WorldCenterMode.FIRST_TARGET);
		}
	}
}
