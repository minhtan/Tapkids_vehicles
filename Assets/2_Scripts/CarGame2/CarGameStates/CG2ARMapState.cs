using UnityEngine;
using System.Collections;
using Prime31.StateKit;

public class CG2ARMapState : SKState<CarGameController2> {

	//	private float countDownTimer;

	public override void begin ()
	{
		Debug.Log("Map State >>>");

		Messenger.Broadcast <bool> (EventManager.GameState.START.ToString(), false);
	}


	public override void reason ()
	{
	}
	public override void update (float deltaTime)
	{
	}

	public override void end ()
	{
		Debug.Log("Map State <<<");
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
