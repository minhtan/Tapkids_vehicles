using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GaragaController : MonoBehaviour {

	// TODO: 
	// => need a list string of vehicles
	// 
	// pre setup 26 vehicles slot
	// get car model based on player unlocked list
	// 

	#region public members
	public int currentSelectedCar;
	#endregion public members

	#region private members
	private string[] demoGarage = new string[] {"police", "ambulance"};

	private GameObject[] vehicleDemos;

	private Vehicle[] vehicles;
	#endregion private members

	#region Mono
	void OnEnable () {
		CarGameEventController.SelectCar += OnSelectCar;
	}

	void OnDisable () {
		CarGameEventController.SelectCar -= OnSelectCar;
	}

	void Start () {
		// get player unlocked list

		// compare with avaiable vehicle list

		// then setup garage

		// demo setup garage
		vehicleDemos = new GameObject[transform.childCount];
		for (int i = 0; i < vehicleDemos.Length; i++) {
			vehicleDemos[i] = transform.GetChild (i).gameObject;
			if (PlayerDataController.Instance.mPlayer.currentVehicleIndex == i) 
				vehicleDemos[i].SetActive (true);
			else
				vehicleDemos[i].SetActive (false);
		}
	}
	#endregion Mono

	#region public functions
	#endregion public functions

	#region private functions
	private void OnSelectCar (int _index) {

		// handle car modle
		if (currentSelectedCar + _index >= 0 && currentSelectedCar + _index < vehicleDemos.Length)
		{
			vehicleDemos [currentSelectedCar].SetActive (false);
			currentSelectedCar += _index;
			vehicleDemos [currentSelectedCar].SetActive (true);
		}

		// TODO: update car stats in GUI
		// is this unlocked vehicle ?
	}

	// TODO: load vehicle only it is unlocked one
	private IEnumerator LoadVehicleFromAssetBundle (string _assetBundleName, string _vehicleName) {
		yield return StartCoroutine (AssetController.Instance.InstantiateGameObjectAsync (_assetBundleName, _vehicleName, (bundle) => {
			// GameObject vehicle = Instantiate (bundle) as GameObject;
			// add vehicle to garage vehicles list
		}));
	}
	#endregion private functions

}
