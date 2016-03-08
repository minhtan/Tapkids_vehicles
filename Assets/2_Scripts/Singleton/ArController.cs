using UnityEngine;
using System.Collections;
using Vuforia;
using System.Collections.Generic;
using System.Linq;

public class ArController : UnitySingletonPersistent<ArController> {

	#region Vars
	ObjectTracker tracker;
	IEnumerable<DataSet> dataSets;
	#endregion

	#region Mono
	public override void Awake ()
	{
		base.Awake ();
	}

	void Start () {
//		yield return new WaitForSeconds (1.0f);

	}

	void Update () {
	
	}
	#endregion

	public void SetArMaxStimTargets(int targetNums){
		VuforiaUnity.SetHint ( Vuforia.VuforiaUnity.VuforiaHint.HINT_MAX_SIMULTANEOUS_IMAGE_TARGETS,  targetNums );
	}

	public void ToggleAR(bool state){
		VuforiaBehaviour.Instance.enabled = state;
	}

	public void SetActiveDataSet(int index, bool isActive){
		tracker = TrackerManager.Instance.GetTracker<ObjectTracker> ();
		dataSets = tracker.GetDataSets ();
		if(dataSets.Count() <= 0 || dataSets.Count() < index - 1){
			return;
		}

		tracker.Stop ();
		if (isActive) {
			tracker.ActivateDataSet (dataSets.ElementAt (index));
		} else {
			tracker.DeactivateDataSet (dataSets.ElementAt (index));
		}
		tracker.Start ();
	}

	public void DeactiveAllDataSet(){
		tracker = TrackerManager.Instance.GetTracker<ObjectTracker> ();
		dataSets = tracker.GetDataSets ();
		using (IEnumerator<DataSet> dataSetsEnum = dataSets.GetEnumerator())
		{
			while (dataSetsEnum.MoveNext())
			{
				tracker.DeactivateDataSet (dataSetsEnum.Current);
			}
		}
	}
}
