using UnityEngine;
using System.Collections;
using Vuforia;
using System.Collections.Generic;
using System.Linq;

public class ArController : UnitySingletonPersistent<ArController> {

	#region Vars
	#endregion

	#region Mono
	public override void Awake ()
	{
		base.Awake ();
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
}
