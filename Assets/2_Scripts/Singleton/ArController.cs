using UnityEngine;
using System.Collections;
using Vuforia;

public class ArController : UnitySingletonPersistent<ArController> {

	#region Vars

	#endregion

	#region Mono
	public override void Awake ()
	{
		base.Awake ();
	}

	void Start () {
	
	}

	void Update () {
	
	}
	#endregion

	public void SetArMaxStimTargets(int targetNums){
		VuforiaUnity.SetHint ( Vuforia.VuforiaUnity.VuforiaHint.HINT_MAX_SIMULTANEOUS_IMAGE_TARGETS,  targetNums );
	}

	public void ToggleAr(bool state){
		VuforiaBehaviour.Instance.enabled = state;
	}
}
