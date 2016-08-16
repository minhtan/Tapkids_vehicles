using UnityEngine;
using System.Collections;
using Prime31.StateKit;

public class CGARCardState : SKState<CarGameController> {

	public override void begin ()
	{
		Debug.Log("Wait Letter State >>>");

//		Messenger.Broadcast <string, float> (EventManager.GUI.NOTIFY.ToString(), GameConstant.LetterScanMessage, 3f);
	}

	public override void reason ()
	{
	}
	public override void update (float deltaTime)
	{
	}

	public override void end ()
	{
		Debug.Log("Wait Letter State <<<");
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
