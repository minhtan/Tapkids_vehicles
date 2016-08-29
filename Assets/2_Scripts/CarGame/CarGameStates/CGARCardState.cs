using UnityEngine;
using System.Collections;
using Prime31.StateKit;

public class CGARCardState : SKState<CarGameController> {

	public override void begin ()
	{
		Debug.Log("Wait Letter State >>>");
		if (PlayerDataController.Instance.mPlayer.playedTuts[0] == false) {
			Messenger.Broadcast (EventManager.GUI.TOGGLE_TUTORIAL.ToString ());
		}
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
