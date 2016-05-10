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
	private List<GameObject> vehicles;
	private Transform mTransform;
	private Shader lockedShader;
	#endregion private members

	#region Mono
	void Awake () {
		mTransform = GetComponent <Transform> ();
	}
	void OnEnable () {
		Messenger.AddListener <int> (EventManager.GUI.SELECT_VEHICLE.ToString (), HandleSelectCar);
		Messenger.AddListener (EventManager.GUI.ENTER_GARAGE.ToString (), HandleEnterGarage);
		Messenger.AddListener (EventManager.GUI.EXIT_GARAGE.ToString (), HandleExitGarage);
		Messenger.AddListener (EventManager.GUI.PURCHASE_VEHICLE.ToString (), HandlePurchaseVehicle);
		Messenger.AddListener <int> (EventManager.GUI.CHANGE_MATERIAL.ToString (), HandleChangeMaterial);
	}

	void OnDisable () {
		Messenger.RemoveListener <int> (EventManager.GUI.SELECT_VEHICLE.ToString (), HandleSelectCar);
		Messenger.RemoveListener (EventManager.GUI.ENTER_GARAGE.ToString (), HandleEnterGarage);
		Messenger.RemoveListener (EventManager.GUI.EXIT_GARAGE.ToString (), HandleExitGarage);
		Messenger.RemoveListener (EventManager.GUI.PURCHASE_VEHICLE.ToString (), HandlePurchaseVehicle);
		Messenger.RemoveListener <int> (EventManager.GUI.CHANGE_MATERIAL.ToString (), HandleChangeMaterial);
	}

	void Start () {
		vehicles = new List <GameObject> ();
		lockedShader = Shader.Find ("Custom/Unlit/Color");
		// get player unlocked list

		// compare with avaiable vehicle list

		// then setup garage
		for (int i = 0; i < GameConstant.fourWheels.Count; i++) {
			StartCoroutine (SetupCar (GameConstant.fourWheels[i]));
		}
	}
	private IEnumerator SetupCar (string vehicleName) {
		yield return new WaitForSeconds (1f);
		StartCoroutine (AssetController.Instance.InstantiateGameObjectAsync (GameConstant.assetBundleName, vehicleName, (bundle) => {
			GameObject carGameObject = Instantiate (bundle) as GameObject;
			vehicles.Add (carGameObject);
			carGameObject.GetComponent <Rigidbody> ().isKinematic = true;

			// apply lock shader
			if (!PlayerDataController.Instance.unlockedIds.Contains (carGameObject.GetComponent <ArcadeCarController> ().vehicle.id)) {
				Renderer[] renderers = carGameObject.GetComponentsInChildren <Renderer> ();
				for (int j = 0; j < renderers.Length; j++) {
					renderers [j].material.shader = lockedShader;
				}
			} 

			// apply material matching with player data
			for (int i = 0; i < PlayerDataController.Instance.mPlayer.unlockedVehicles.Count; i++) {
				if (carGameObject.GetComponent <ArcadeCarController> ().vehicle.id == PlayerDataController.Instance.mPlayer.unlockedVehicles [i].id ) {
					Renderer [] renderers = carGameObject.GetComponentsInChildren <Renderer> ();
					for (int j = 0; j < renderers.Length; j++) {
						renderers [j].material = carGameObject.GetComponent <ArcadeCarController> ().mats [PlayerDataController.Instance.mPlayer.unlockedVehicles [i].matId];
					}
					break;
				}
			}
			carGameObject.transform.SetParent (mTransform, false);
			// update player current car 
			if (PlayerDataController.Instance.mPlayer.vehicleId == carGameObject.GetComponent <ArcadeCarController> ().vehicle.id) {
				currentSelectedCar = vehicles.IndexOf (carGameObject);
				carGameObject.transform.localPosition = Vector3.zero;
				Messenger.Broadcast <Vehicle> (EventManager.GUI.UPDATE_VEHICLE.ToString (), carGameObject.GetComponent <ArcadeCarController> ().vehicle);

			} else {
				carGameObject.transform.localPosition = new Vector3 (0f, 0f, -10f);
				carGameObject.SetActive (false);
			}
		}));
	}

	#endregion Mono

	#region public functions
	#endregion public functions


	#region private functions
	bool valid = true;
	private void HandleSelectCar (int _index) {
		// handle car modle
		if (valid == false) return;

		if (currentSelectedCar + _index >= 0 && currentSelectedCar + _index < vehicles.Count)
		{
			valid = false;
//			vehicles [currentSelectedCar].SetActive (false);
			LeanTween.moveLocalZ (vehicles [currentSelectedCar], 20.0f, 1f).setEase(LeanTweenType.easeInBack).setOnComplete ( () => {
				vehicles [currentSelectedCar].transform.localPosition = new Vector3 (0f, 0f, -20f);
				vehicles [currentSelectedCar].SetActive (false);

				currentSelectedCar = Mathf.Clamp (currentSelectedCar + _index, 0, vehicles.Count- 1);

				vehicles [currentSelectedCar].SetActive (true);
				// update car 
				Messenger.Broadcast <Vehicle> (EventManager.GUI.UPDATE_VEHICLE.ToString (), vehicles[currentSelectedCar].GetComponent <ArcadeCarController> ().vehicle);

				LeanTween.moveLocalZ (vehicles [currentSelectedCar], 0f, 1f).setEase (LeanTweenType.easeOutBack).setOnComplete ( () => { 
					valid = true;
				});
			});
		}
	}

	private void HandleEnterGarage () {
		// get player current select car

		// setup selected car
	}
	private void HandleExitGarage () {
		// get player current select car

		// setup selected car
	}

	private void HandlePurchaseVehicle () {
		if (PlayerDataController.Instance.mPlayer.credit >= vehicles[currentSelectedCar].GetComponent <ArcadeCarController> ().vehicle.costPoint) {
			PlayerDataController.Instance.UnlockVehicle (vehicles[currentSelectedCar].GetComponent <ArcadeCarController> ().vehicle);

			// apply standard shader to unlocked car
			Renderer[] renderers = vehicles[currentSelectedCar].GetComponentsInChildren <Renderer> ();
			for (int j = 0; j < renderers.Length; j++) {
				renderers [j].material.shader = Shader.Find ("Standard");
			}

			Messenger.Broadcast <Vehicle> (EventManager.GUI.UPDATE_VEHICLE.ToString (), vehicles[currentSelectedCar].GetComponent <ArcadeCarController> ().vehicle);
		} else {
			// notify player
			Messenger.Broadcast <string, float> (EventManager.GUI.NOTIFY.ToString (), GameConstant.PurchaseUnsuccessful, 1f);
		}
	}

	void HandleChangeMaterial (int _matId) {
		// update player data
		Renderer[] renderers = vehicles [currentSelectedCar].GetComponentsInChildren<Renderer> ();
		for (int i = 0; i < renderers.Length; i++) {
			renderers[i].material = vehicles [currentSelectedCar].GetComponent <ArcadeCarController> ().mats[_matId];
		}
		vehicles [currentSelectedCar].GetComponent <ArcadeCarController> ().vehicle.matId = _matId;
		PlayerDataController.Instance.UpdateVehicle (vehicles [currentSelectedCar].GetComponent <ArcadeCarController> ().vehicle);
	}
	#endregion private functions

}
