using UnityEngine;
using System.Collections;
using Vuforia;
using System.Collections.Generic;
using System.Linq;

public class ArController : UnitySingletonPersistent<ArController> {

	#region Vars
	ObjectTracker tracker;
	public bool isVuforiaReady = false;
	#endregion

	#region Mono
	public override void Awake ()
	{
		base.Awake ();
		VuforiaBehaviour.Instance.RegisterVuforiaInitializedCallback (OnVuforiaInitialized);
	}

	void Update () {
	}
	#endregion

	void OnVuforiaInitialized(){
		tracker = TrackerManager.Instance.GetTracker<ObjectTracker> ();
		isVuforiaReady = true;
	}

	public void SetArMaxStimTargets(int targetNums){
		VuforiaUnity.SetHint ( Vuforia.VuforiaUnity.VuforiaHint.HINT_MAX_SIMULTANEOUS_IMAGE_TARGETS,  targetNums );
	}

	public void ToggleAR(bool state){
		VuforiaBehaviour.Instance.enabled = state;
	}

	public void LoadAndActiveDataSet(string name){
		DataSet dataSet = tracker.CreateDataSet ();
		dataSet.Load (name);
		tracker.ActivateDataSet (dataSet);
	}

	public void UnloadAllDataSet(){
		tracker.DestroyAllDataSets (true);
	}
}
