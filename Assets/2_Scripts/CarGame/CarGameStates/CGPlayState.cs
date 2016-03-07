using UnityEngine;
using System.Collections;

using Prime31.StateKit;

public class CGPlayState : SKState<CarGameController> {

	// handle game time 
	float time = 10f;

	public override void begin ()
	{
		Debug.Log("Play State >>>");
	}

	public override void reason ()
	{
		if (time <= 0) {
			_machine.changeState <CGGameOverState> ();
		}
	}
	public override void update (float deltaTime)
	{
		time -= Time.deltaTime;
	}

	public override void end ()
	{
		Debug.Log("Play State <<<");
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
