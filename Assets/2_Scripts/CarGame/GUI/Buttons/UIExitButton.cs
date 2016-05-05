using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIExitButton : MonoBehaviour {

	#region public members
	#endregion public members

	#region private members
	private Button mButton;

	#endregion private members

	#region Mono
	void Start () {
		mButton = GetComponent <Button> ();
		if(mButton != null) {
			mButton.onClick.AddListener ( delegate {

				// TODO: save player data proccess

				// back to menu
				SceneController.Instance.LoadingSceneAsync (SceneController.SceneID.MENU);
				Messenger.Broadcast<bool>(EventManager.GameState.PAUSE.ToString(), false);
				Messenger.Broadcast(EventManager.GameState.EXIT_TO_MENU.ToString());
			});
		}
	}
	#endregion Mono

	#region public functions
	#endregion public functions

	#region private functions
	#endregion private functions

}
