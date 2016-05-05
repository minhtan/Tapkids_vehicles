using UnityEngine;
using System.Collections;
using Prime31.StateKit;

public class CGARMapState : SKState<CarGameController> {

//	private float countDownTimer;

	public override void begin ()
	{
		Debug.Log("Map State >>>");

		Messenger.Broadcast <bool> (EventManager.GameState.START.ToString (), false);
//		for (int i = 0; i < _context.mTransform.childCount; i++) 
//			_context.mTransform.GetChild (i).gameObject.SetActive (false);
		
		// reset countdown
//		countDownTimer = 3f;
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
