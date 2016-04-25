using UnityEngine;
using System.Collections;
using Prime31.StateKit;

public class CG2InitState : SKState<CarGameController2> {

	#region private members
	private	float countDownTimer = 3f;

	#endregion private members

	#region SKState
	public override void begin ()
	{
		Debug.Log("Init State >>>");

		// broadcast event load car, letter, obstacle
		Messenger.Broadcast <string> (EventManager.GameState.INITGAME.ToString (), _context.letters);
	}

	public override void reason ()
	{
		if (countDownTimer <= 0f) {
			_machine.changeState <CG2MapState> ();
		}
	}
	public override void update (float deltaTime)
	{
		countDownTimer -= Time.deltaTime;
	}

	public override void end ()
	{
		Debug.Log("Init State <<<");
	}
	#endregion SKState

	#region private functions
	#endregion private functions

}
