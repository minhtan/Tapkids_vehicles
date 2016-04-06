using UnityEngine;
using System.Collections;
using Prime31.StateKit;

public class CGWaitForTargetState : SKState<CarGameController> {

	public override void begin ()
	{
		Debug.Log("Wait State >>>");

		// TODO: setting ar camera
//		ArController.Instance.ToggleAR (true);
//		ArController.Instance.SetCenterMode (false);
//		ArController.Instance.SetArMaxStimTargets (1);
	}

	public override void reason ()
	{
	}
	public override void update (float deltaTime)
	{
	}

	public override void end ()
	{
		Debug.Log("Wait State <<<");
	}

	#region public members
	#endregion public members

	#region private members
	#endregion private members

	#region Mono
	#endregion Mono

	#region public functions
	#endregion public functions

	#region private functions
	#endregion private functions

}
