using UnityEngine;
using System.Collections;
using Prime31.StateKit;

public class CGInitState : SKState<CarGameController> {

	private	float countDownTimer = 3f;
	private WordGameData wordData;

	public override void begin ()
	{
		Debug.Log("Init State >>>");

		// reset game time
		_context.gameTime = 10f;
		// get word data]
		wordData = DataUltility.ReadDataForCarGame (_context.letter);
		// send event init game environment
		CarGameEventController.OnInitGame (wordData.letters);

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
