using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CarPanel : MonoBehaviour {

	#region public members
	public Text carNameText;
	#endregion public members

	#region private members
	#endregion private members

	#region Mono
	void OnEnable () {
		Messenger.AddListener <Vehicle> (EventManager.GUI.UPDATECAR.ToString (), HandleUpdateCar);
	}

	void Awake () {

	}

	void Start () {

	}

	void OnDisable () {
		Messenger.AddListener <Vehicle> (EventManager.GUI.UPDATECAR.ToString (), HandleUpdateCar);
	}
	#endregion Mono

	#region public functions
	#endregion public functions

	#region private functions
	private void HandleUpdateCar (Vehicle _vehicle) {
		Debug.Log (_vehicle.name);
		carNameText.text = _vehicle.name;
	}
	#endregion private functions

}
