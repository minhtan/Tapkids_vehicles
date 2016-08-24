using UnityEngine;
using System.Collections;
using Prime31.StateKit;

public class CGARCardState : SKState<CarGameController> {

	public override void begin ()
	{
		Debug.Log("Wait Letter State >>>");

//		if (PlayerPrefs.HasKey (GameConstant.hasPlayedTutorial)) {
//			if (PlayerPrefs.GetInt (GameConstant.hasPlayedTutorial) == 1) {
//				Messenger.Broadcast <bool> (EventManager.GUI.TOGGLE_TUTORIAL.ToString (), true);
//			} else {
//				Messenger.Broadcast <bool> (EventManager.GUI.TOGGLE_TUTORIAL.ToString (), false);
//			}
//		} else {
//			Messenger.Broadcast <bool> (EventManager.GUI.TOGGLE_TUTORIAL.ToString (), true);
//		}

		if (PlayerDataController.Instance.mPlayer.playedTuts[0] == true) {
			Messenger.Broadcast <bool> (EventManager.GUI.TOGGLE_TUTORIAL.ToString (), false);
		} else {
			Messenger.Broadcast <bool> (EventManager.GUI.TOGGLE_TUTORIAL.ToString (), true);
		}
	}

	public override void reason ()
	{
	}
	public override void update (float deltaTime)
	{
	}

	public override void end ()
	{
		Debug.Log("Wait Letter State <<<");
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
