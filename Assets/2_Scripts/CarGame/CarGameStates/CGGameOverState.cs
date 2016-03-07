using UnityEngine;
using System.Collections;
using Prime31.StateKit;

public class CGGameOverState : SKState<CarGameController> {

	public override void begin ()
	{
		Debug.Log("Game Over State >>>");
	}

	public override void reason ()
	{
	}
	public override void update (float deltaTime)
	{
	}

	public override void end ()
	{
		Debug.Log("Game Over State <<<");
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
