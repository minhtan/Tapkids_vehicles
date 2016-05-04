using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIVehicleDescText : MonoBehaviour {

	public Text vehicleDescText;

	void OnEnable () {
		Messenger.AddListener <Vehicle> (EventManager.GUI.UPDATEVEHICLE.ToString (), HandleUpdateVehicle);
	}

	void Start () {
		vehicleDescText = GetComponent <Text> ();
	}
	void OnDisable () {
		Messenger.RemoveListener <Vehicle> (EventManager.GUI.UPDATEVEHICLE.ToString (), HandleUpdateVehicle);
	}

	void HandleUpdateVehicle (Vehicle _vehicle) {
		vehicleDescText.text = _vehicle.desc;
	}

}
