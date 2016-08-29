using UnityEngine;
using System.Collections;
using Prime31.StateKit;

public class CG2InitState : SKState<CarGameController2> {

	#region private members
//	private	float countDownTimer = 3f;

	#endregion private members

	#region SKState
	public override void begin ()
	{
		Debug.Log("Init State >>>");

		if (PlayerDataController.Instance.mPlayer.playedTuts[1] == false) {
			Messenger.Broadcast (EventManager.GUI.TOGGLE_TUTORIAL.ToString ());
		} else {
			Messenger.Broadcast (EventManager.GUI.ACTIVATE_COUNT_DOWN.ToString ());
		}

		_context.wordGameDatas = DataUltility.ReadDataForWordGame ();
		_context.wordGameData = _context.RandomData ();

		_context.playableLetters = DataUltility.GetPlayableLetters (_context.wordGameData);
		_context.answers = DataUltility.GetAnswersList (_context.wordGameData);

		// broadcast event load car, letter, obstacle
		Messenger.Broadcast <string, string> (EventManager.GameState.INIT.ToString (), _context.RandomLetter ().ToString (), _context.wordGameData.letters);

//		
	}

	public override void reason ()
	{
//		if (countDownTimer <= 0f) {
//			_machine.changeState <CG2ARMapState> ();
//		}
	}
	public override void update (float deltaTime)
	{
//		if (countDownTimer > 0) {
//			countDownTimer -= Time.deltaTime;
//		}
	}

	public override void end ()
	{
		Debug.Log("Init State <<<");
	}
	#endregion SKState

	#region private functions
	#endregion private functions

}
