using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MaterialToggle : MonoBehaviour {

	public int matId;

	private Toggle mToggle;

	void OnEnable () {
		Messenger.AddListener <Vehicle> (EventManager.GUI.UPDATE_VEHICLE.ToString (), HandleUpdateVehicle);
	}

	void Start () {
		mToggle = GetComponent <Toggle> ();
		if (mToggle != null) {
			mToggle.onValueChanged.AddListener (delegate {
				if (mToggle.isOn)
					Messenger.Broadcast <int> (EventManager.GUI.CHANGE_MATERIAL.ToString (), matId);
			});
		}
	}

	void OnDisable () {
		Messenger.RemoveListener <Vehicle> (EventManager.GUI.UPDATE_VEHICLE.ToString (), HandleUpdateVehicle);
	}

	void HandleUpdateVehicle (Vehicle _vehicle) {
		if (_vehicle.matId == matId)
			mToggle.isOn = true;
	}
}
