using UnityEngine;
using System.Collections;
using Prime31.StateKit;

public class CGInitState : SKState<CarGameController> {

	#region private members
	private	float countDownTimer = 3f;

	#endregion private members

	#region SKState
	public override void begin ()
	{
		Debug.Log("Init State >>>");

		// reset game time
//		_context.gameTime = 10f;

		// get word data
//		_context.wordGameData = DataUltility.ReadDataForCarGame (_context.letters);
		// send event init game environment
//		_context.answers = DataUltility.GetAnswersList (_context.wordGameData);

		if (_context.givenLetters.Length > 0) {
			Messenger.Broadcast <string, string> (EventManager.GameState.INIT.ToString (), _context.givenLetters[0].ToString(), _context.givenLetters);
		}
	}

	public override void reason ()
	{
		if (countDownTimer <= 0f) {
			_machine.changeState <CGARMapState> ();
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
