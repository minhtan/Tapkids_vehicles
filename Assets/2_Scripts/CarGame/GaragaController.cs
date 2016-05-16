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
	public int currentSelectedIndex;
	#endregion public members

	#region private members
	private List<GameObject> vehicles;
	private Transform mTransform;
	private Shader lockedShader;

	private int lastUnlockedIndex;

	private float forwardPos = 20f;
	private float backwardPos = -20f;
	#endregion private members

	#region Mono
	void Awake () {
		mTransform = GetComponent <Transform> ();
	}
	void OnEnable () {
		Messenger.AddListener <int> (EventManager.GUI.SELECT_VEHICLE.ToString (), HandleSelectCar);
//		Messenger.AddListener (EventManager.GUI.ENTER_GARAGE.ToString (), HandleEnterGarage);
		Messenger.AddListener (EventManager.GUI.EXIT_GARAGE.ToString (), HandleExitGarage);
		Messenger.AddListener (EventManager.GUI.PURCHASE_VEHICLE.ToString (), HandlePurchaseVehicle);
		Messenger.AddListener <int> (EventManager.GUI.CHANGE_MATERIAL.ToString (), HandleChangeMaterial);
	}

	void OnDisable () {
		Messenger.RemoveListener <int> (EventManager.GUI.SELECT_VEHICLE.ToString (), HandleSelectCar);
//		Messenger.RemoveListener (EventManager.GUI.ENTER_GARAGE.ToString (), HandleEnterGarage);
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
					carGameObject.GetComponent <ArcadeCarController> ().vehicle.matId = PlayerDataController.Instance.mPlayer.unlockedVehicles [i].matId;
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
				currentSelectedIndex = vehicles.IndexOf (carGameObject);
				lastUnlockedIndex = currentSelectedIndex;
				carGameObject.transform.localPosition = Vector3.zero;
				LeanTween.rotateAroundLocal (carGameObject, Vector3.up, 360f, 10f).setLoopClamp();

				Debug.Log (carGameObject.GetComponent <ArcadeCarController> ().vehicle.matId);

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
	int HandleCurrentIndex (int index) {
		if (index >= vehicles.Count) {
			index = 0;
		} else if (index < 0) {
			index = vehicles.Count - 1;
		} else {
			// nothing
		}
		return index;

	}
	private void HandleSelectCar (int _index) {
		// handle car modle
		if (valid == false) return;

		//		if (currentSelectedIndex + _index >= 0 && currentSelectedIndex + _index < vehicles.Count)
//		{
		valid = false;

		LeanTween.cancelAll ();

		LeanTween.moveLocalZ (vehicles [currentSelectedIndex], forwardPos, .5f).setEase(LeanTweenType.linear).setOnComplete ( () => {
			vehicles [currentSelectedIndex].transform.localPosition = new Vector3 (0f, 0f, backwardPos);
			vehicles [currentSelectedIndex].transform.localRotation = Quaternion.identity;
			vehicles [currentSelectedIndex].SetActive (false);

			currentSelectedIndex += _index;
			currentSelectedIndex = HandleCurrentIndex (currentSelectedIndex);

			if (PlayerDataController.Instance.unlockedIds.Contains (vehicles [currentSelectedIndex].GetComponent <ArcadeCarController> ().vehicle.id))
				lastUnlockedIndex = currentSelectedIndex;

			vehicles [currentSelectedIndex].SetActive (true);
			// update car 
			Messenger.Broadcast <Vehicle> (EventManager.GUI.UPDATE_VEHICLE.ToString (), vehicles[currentSelectedIndex].GetComponent <ArcadeCarController> ().vehicle);

			LeanTween.moveLocalZ (vehicles [currentSelectedIndex], 0f, .5f).setEase (LeanTweenType.linear).setOnComplete ( () => { 
				valid = true;
				LeanTween.rotateAroundLocal (vehicles [currentSelectedIndex], Vector3.up, 360f, 10f).setLoopClamp();
			});
		});
//		LeanTween.pause (id);
//		}
	}

//	private void HandleEnterGarage () {
//		lastUnlockedIndex = currentSelectedIndex;
//	}
	private void HandleExitGarage () {
		// check if current select car is not unlocked 
		if (!PlayerDataController.Instance.unlockedIds.Contains (vehicles [currentSelectedIndex].GetComponent <ArcadeCarController> ().vehicle.id)) {
			LeanTween.moveLocalZ (vehicles [currentSelectedIndex], forwardPos, .5f).setEase(LeanTweenType.linear).setOnComplete ( () => {
				vehicles [currentSelectedIndex].transform.localPosition = new Vector3 (0f, 0f, backwardPos);
				vehicles [currentSelectedIndex].SetActive (false);

				vehicles [lastUnlockedIndex].SetActive (true);
				// update car 
				Messenger.Broadcast <Vehicle> (EventManager.GUI.UPDATE_VEHICLE.ToString (), vehicles[lastUnlockedIndex].GetComponent <ArcadeCarController> ().vehicle);

				LeanTween.moveLocalZ (vehicles [lastUnlockedIndex], 0f, .5f).setEase (LeanTweenType.linear);
				currentSelectedIndex = lastUnlockedIndex;
			});
		}
	}

	private void HandlePurchaseVehicle () {
		if (PlayerDataController.Instance.mPlayer.credit >= vehicles[currentSelectedIndex].GetComponent <ArcadeCarController> ().vehicle.costPoint) {
			PlayerDataController.Instance.UnlockVehicle (vehicles[currentSelectedIndex].GetComponent <ArcadeCarController> ().vehicle);

			// apply standard shader to unlocked car
			Renderer[] renderers = vehicles[currentSelectedIndex].GetComponentsInChildren <Renderer> ();
			for (int j = 0; j < renderers.Length; j++) {
				renderers [j].material.shader = Shader.Find ("Standard");
			}

			Messenger.Broadcast <Vehicle> (EventManager.GUI.UPDATE_VEHICLE.ToString (), vehicles[currentSelectedIndex].GetComponent <ArcadeCarController> ().vehicle);
		} else {
			// notify player
			Messenger.Broadcast <string, float> (EventManager.GUI.NOTIFY.ToString (), GameConstant.PurchaseUnsuccessful, 1f);
		}
	}

	void HandleChangeMaterial (int _matId) {
		// update player data
		Renderer[] renderers = vehicles [currentSelectedIndex].GetComponentsInChildren<Renderer> ();
		for (int i = 0; i < renderers.Length; i++) {
			renderers[i].material = vehicles [currentSelectedIndex].GetComponent <ArcadeCarController> ().mats[_matId];
		}
		vehicles [currentSelectedIndex].GetComponent <ArcadeCarController> ().vehicle.matId = _matId;

		PlayerDataController.Instance.UpdateVehicle (vehicles [currentSelectedIndex].GetComponent <ArcadeCarController> ().vehicle);
	}
	#endregion private functions

}
