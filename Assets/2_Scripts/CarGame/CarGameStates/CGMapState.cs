using UnityEngine;
using System.Collections;
using Prime31.StateKit;

public class CGMapState : SKState<CarGameController> {

	private float countDownTimer;

	public override void begin ()
	{
		Debug.Log("Wait Map State >>>");
		// reset countdown
		countDownTimer = 3f;
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
		Debug.Log("Wait Map State <<<");
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
