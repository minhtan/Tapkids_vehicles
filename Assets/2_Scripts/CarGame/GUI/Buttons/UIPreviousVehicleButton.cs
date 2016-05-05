using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIPreviousVehicleButton : MonoBehaviour {

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
//				CarGameEventController.OnSelectCar (-1);
				Messenger.Broadcast <int> (EventManager.GUI.SELECT_VEHICLE.ToString (), -1);
			});
		}
	}
	#endregion Mono

	#region public functions
	#endregion public functions

	#region private functions
	#endregion private functions

}
