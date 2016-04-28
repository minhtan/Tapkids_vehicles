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
//			mText.text = "Name: " + PlayerDataController.Instance.mPlayer.currentVehicle.name + "\n" 
//				+ "Max Speed: " + PlayerDataController.Instance.mPlayer.currentVehicle.maxSpeed + "\n" 
//				+ "Cost: " + PlayerDataController.Instance.mPlayer.currentVehicle.costPoint;
		}
	}
	#endregion Mono

	#region public functions
	#endregion public functions

	#region private functions
	#endregion private functions

}
