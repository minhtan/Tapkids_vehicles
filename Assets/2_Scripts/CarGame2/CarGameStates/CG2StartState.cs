using UnityEngine;
using System.Collections;
using Prime31.StateKit;

public class CG2StartState : SKState<CarGameController2> {
	// send event start game: enable mobile input, 

	public override void begin ()
	{
		Debug.Log("Start State >>>");

		for (int i = 0; i < _context.mTransform.childCount; i++) 
			_context.mTransform.GetChild (i).gameObject.SetActive (true);

		Messenger.Broadcast (EventManager.GameState.STARTGAME.ToString());

	}

	public override void reason ()
	{

	}
	public override void update (float deltaTime)
	{


	}

	public override void end ()
	{
		Debug.Log("Start State <<<");
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
