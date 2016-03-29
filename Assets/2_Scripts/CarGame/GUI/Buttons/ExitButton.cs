using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ExitButton : MonoBehaviour {

	#region public members
	#endregion public members

	#region private members
	private Button mButton;
	private int menuSceneId = 2;

	#endregion private members

	#region Mono
	void Start () {
		mButton = GetComponent <Button> ();
		if(mButton != null) {
			mButton.onClick.AddListener ( delegate {
				// back to menu
				SceneController.Instance.LoadingSceneAsync ((SceneController.SceneID) menuSceneId);
			});
		}
	}
	#endregion Mono

	#region public functions
	#endregion public functions

	#region private functions
	#endregion private functions

}
