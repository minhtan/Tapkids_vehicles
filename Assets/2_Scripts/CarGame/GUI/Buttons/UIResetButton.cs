﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIResetButton : MonoBehaviour {

	#region public members
	#endregion public members

	#region private members
	private Button resetButton;
	#endregion private members

	#region Mono
	void Start () {
		resetButton = GetComponent <Button> ();
		if(resetButton != null) {
			resetButton.onClick.AddListener ( delegate {
//				CarGameEventController.OnResetGame ();
				Messenger.Broadcast (EventManager.GameState.RESET.ToString() );
				Messenger.Broadcast<bool> (EventManager.GameState.PAUSE.ToString (), false);
				AudioManager.Instance.PlayAudio (AudioKey.UNIQUE_KEY.BUTTON_CLICK);
				// car reset position
				// score reset 0
				// re init game
			});
		}
	}
	#endregion Mono

	#region public functions
	#endregion public functions

	#region private functions
	#endregion private functions

}
