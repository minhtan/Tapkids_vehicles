using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CarPanel : MonoBehaviour {

	#region public members
	#endregion public members

	#region private members
	private Text mText;
	#endregion private members

	#region Mono
	void Start () {
		mText = GetComponentInChildren <Text> ();
		if (mText != null) {
			mText.text = "Name: " + TapkidsData.GetVehicleById (PlayerDataController.Instance.mPlayer.id).name + "\n" 
				+ "Max Speed: " + TapkidsData.GetVehicleById (PlayerDataController.Instance.mPlayer.id).maxSpeed + "\n" 
				+ "Cost: " + TapkidsData.GetVehicleById (PlayerDataController.Instance.mPlayer.id).costPoint;
		}
	}
	#endregion Mono

	#region public functions
	#endregion public functions

	#region private functions
	#endregion private functions

}
