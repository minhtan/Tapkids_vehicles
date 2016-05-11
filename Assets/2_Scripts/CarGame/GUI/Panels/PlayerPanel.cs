using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerPanel : MonoBehaviour {

	#region public members
	#endregion public members

	#region private members
	Text mText;
	#endregion private members

	#region Mono
	void Start () {
		mText = GetComponentInChildren <Text> ();
		if(mText != null) {
//			mText.text = "Hi " + TapkidsData.GetPlayerById (PlayerDataController.Instance.mPlayer.id).name;
		}
	}
	#endregion Mono

	#region public functions
	#endregion public functions

	#region private functions
	#endregion private functions

}
