using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIResumeButton : MonoBehaviour {

	#region public members
	#endregion public members

	#region private members
	private Button mButton;
	#endregion private members

	#region Mono
	void Start () {
		mButton = GetComponent <Button> ();
		if (mButton != null) {
			mButton.onClick.AddListener (delegate {
//				CarGameEventController.OnPauseGame (false);	
//				CarGameEventController.OnTogglePanel ("ingame");	
				Messenger.Broadcast<bool> ( EventManager.GameState.PAUSEGAME.ToString(), false);
			});
		}
	}
	#endregion Mono

	#region public functions
	#endregion public functions

	#region private functions
	#endregion private functions

}
