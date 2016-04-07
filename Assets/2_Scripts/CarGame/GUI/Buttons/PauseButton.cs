﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PauseButton : MonoBehaviour {

	#region public members
	#endregion public members

	#region private members
	private Button pauseButton;
	#endregion private members

	#region Mono
	void Start () {
		pauseButton = GetComponent <Button> ();
		if(pauseButton != null) {
			pauseButton.onClick.AddListener (delegate {
				//CarGameEventController.OnPauseGame (true);
//				CarGameEventController.OnTogglePanel ("pause");
				Messenger.Broadcast<bool> ( EventManager.MenuEvent.ISPAUSE.ToString(), true);
			});
		}
	}
	#endregion Mono

	#region public functions
	#endregion public functions

	#region private functions
	#endregion private functions

}
