using UnityEngine;
using System.Collections;
using Prime31.StateKit;

public class CG2GameOverState : SKState<CarGameController2> {

	//	private	float countDownTimer = 3f;

	public override void begin ()
	{
		Debug.Log("Game Over State >>>");
		// TODO: pass star count to gameover panel
		Messenger.Broadcast <int> (EventManager.GameState.GAMEOVER.ToString (), 0);
	}

	public override void reason ()
	{
		//		if (countDownTimer <= 0f) {
		//			_machine.changeState <CGStartState> ();
		//		}
	}
	public override void update (float deltaTime)
	{
		//		countDownTimer -= Time.deltaTime;
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
