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
		Messenger.AddListener (EventManager.GUI.PURCHASE_VEHICLE.ToString (), HandlePurchaseVehicle);
	}

	void OnDisable () {
		Messenger.RemoveListener <int> (EventManager.GUI.SELECT_VEHICLE.ToString (), HandleSelectCar);
		Messenger.RemoveListener (EventManager.GUI.ENTER_GARAGE.ToString (), HandleEnterGarage);
		Messenger.RemoveListener (EventManager.GUI.PURCHASE_VEHICLE.ToString (), HandlePurchaseVehicle);
	}

	void Start () {
		vehicles = new List <GameObject> ();
		lockedShader = Shader.Find ("Unlit/Color");
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
			Destroy (carGameObject.GetComponent <Rigidbody> ());

			if (!PlayerDataController.Instance.mPlayer.unlockedVehicles.Contains (carGameObject.GetComponent <ArcadeCarController> ().vehicle.id)) {
				Renderer[] renderers = carGameObject.GetComponentsInChildren <Renderer> ();
				for (int j = 0; j < renderers.Length; j++) {
					renderers [j].material.shader = lockedShader;
				}
			}

			carGameObject.transform.localPosition = Vector3.zero;
			carGameObject.transform.SetParent (mTransform, false);
			if (vehicles.Count == 1) {
				Messenger.Broadcast <Vehicle> (EventManager.GUI.UPDATE_VEHICLE.ToString (), carGameObject.GetComponent <ArcadeCarController> ().vehicle);
			}
			if (vehicles.Count > 1) {
				carGameObject.transform.localPosition = new Vector3 (0f, 0f, -10f);
				carGameObject.SetActive (false);

			}
		}));
	}
	void Update () {
//		if (Input.GetKeyDown (KeyCode.Space)) {
//			Debug.Log (AssetController.Instance.GetTotalLoadedAssetBundle ().ToString ());
//		}
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

		// TODO: update car stats in GUI
		// is this unlocked vehicle ?
	}

	private void HandleEnterGarage () {
		// get player current select car

		// setup selected car
	}

	private void HandlePurchaseVehicle () {
		if (PlayerDataController.Instance.mPlayer.currentCredit >= vehicles[currentSelectedCar].GetComponent <ArcadeCarController> ().vehicle.costPoint) {
			PlayerDataController.Instance.UpdateUnlockedVehicle (vehicles[currentSelectedCar].GetComponent <ArcadeCarController> ().vehicle);
			Messenger.Broadcast <Vehicle> (EventManager.GUI.UPDATE_VEHICLE.ToString (), vehicles[currentSelectedCar].GetComponent <ArcadeCarController> ().vehicle);
		} else {
			// notify player
			Messenger.Broadcast <string, float> (EventManager.GUI.NOTIFY.ToString (), GameConstant.PurchaseUnsuccessful, 1f);
		}
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
