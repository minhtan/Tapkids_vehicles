using UnityEngine;
using System.Collections;
using Prime31.StateKit;

public class CGInitState : SKState<CarGameController> {

	private	float countDownTimer = 3f;

	public override void begin ()
	{
		Debug.Log("Init State >>>");

		// reset game time
		_context.gameTime = 10f;
		// get word data]
		_context.wordGameData = DataUltility.ReadDataForCarGame (_context.letter);
		// send event init game environment
		_context.answers = DataUltility.GetAnswersList (_context.wordGameData);
		CarGameEventController.OnInitGame (_context.wordGameData.letters);

	}

	public override void reason ()
	{
		if (countDownTimer <= 0f) {
			_machine.changeState <CGStartState> ();
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
