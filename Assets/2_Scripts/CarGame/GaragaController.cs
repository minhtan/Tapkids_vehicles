﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;
public class GaragaController : MonoBehaviour {

	// TODO: 
	// => need a list string of vehicles
	// 
	// pre setup 26 vehicles slot
	// get car model based on player unlocked list
	// 

	#region public members
	public Material lockedMaterial;
	public int currentSelectedIndex;
	public Transform disk;

	public Transform initPos;
	public Transform nextPos;
	public Transform prevPos;
	public Transform diskDown;
	public Transform diskUp;
	#endregion public members

	#region private members
	public List<GameObject> vehicles;
	private Transform mTransform;

	private int lastUnlockedIndex;

	private float forwardPos = 20f;
	private float backwardPos = -20f;

	private int curVehicleRotateId;
	#endregion private members

	#region Mono
	void Awake () {
		mTransform = GetComponent <Transform> ();
	}
	void OnEnable () {
		Messenger.AddListener <int> (EventManager.GUI.SELECT_VEHICLE.ToString (), HandleSelectVehicle);
		Messenger.AddListener (EventManager.GUI.TO_MENU.ToString (), HandleExitGarage);
//		Messenger.AddListener (EventManager.GUI.TO_GARAGE.ToString (), HandleEnterGarage);
		Messenger.AddListener (EventManager.GUI.PURCHASE_VEHICLE.ToString (), HandlePurchaseVehicle);
		Messenger.AddListener <int> (EventManager.GUI.CHANGE_MATERIAL.ToString (), HandleChangeMaterial);
	}

	void OnDisable () {
		Messenger.RemoveListener <int> (EventManager.GUI.SELECT_VEHICLE.ToString (), HandleSelectVehicle);
		Messenger.RemoveListener (EventManager.GUI.TO_MENU.ToString (), HandleExitGarage);
//		Messenger.AddListener (EventManager.GUI.TO_GARAGE.ToString (), HandleEnterGarage);
		Messenger.RemoveListener (EventManager.GUI.PURCHASE_VEHICLE.ToString (), HandlePurchaseVehicle);
		Messenger.RemoveListener <int> (EventManager.GUI.CHANGE_MATERIAL.ToString (), HandleChangeMaterial);
	}

	void Start () {
		vehicles = new List <GameObject> ();
		// get player unlocked list

		// compare with avaiable vehicle list

		// then setup garage
		for (int i = 0; i < GameConstant.fourWheels.Count; i++) {
			StartCoroutine (SetupCar (GameConstant.fourWheels[i], () => {
				vehicles = vehicles.OrderBy (x => x.GetComponent <ArcadeCarController> ().vehicle.id).ToList();
			}));
		}
	}

	private IEnumerator SetupCar (string _vehicleName, System.Action _callback) {
		yield return new WaitForSeconds (1f);
		StartCoroutine (AssetController.Instance.InstantiateGameObjectAsync (GameConstant.assetBundleName, _vehicleName, (bundle) => {
			GameObject carGameObject = Instantiate (bundle, initPos.position, Quaternion.identity) as GameObject;
			vehicles.Add (carGameObject);
			carGameObject.GetComponent <Rigidbody> ().isKinematic = true;
			carGameObject.SetActive (false);
			// apply lock shader
			if (!PlayerDataController.Instance.unlockedIds.Contains (carGameObject.GetComponent <ArcadeCarController> ().vehicle.id)) {
				Renderer[] renderers = carGameObject.GetComponentsInChildren <Renderer> ();
				for (int j = 0; j < renderers.Length; j++) {
					renderers [j].material = lockedMaterial;
				}
			} 

			// apply material matching with player data
			for (int i = 0; i < PlayerDataController.Instance.mPlayer.unlockedVehicles.Count; i++) {
				if (carGameObject.GetComponent <ArcadeCarController> ().vehicle.id == PlayerDataController.Instance.mPlayer.unlockedVehicles [i].id ) {
					carGameObject.GetComponent <ArcadeCarController> ().vehicle.matId = PlayerDataController.Instance.mPlayer.unlockedVehicles [i].matId;
					Renderer [] renderers = carGameObject.GetComponentsInChildren <Renderer> ();
					for (int j = 0; j < renderers.Length; j++) {
						renderers [j].material = carGameObject.GetComponent <ArcadeCarController> ().carMats [PlayerDataController.Instance.mPlayer.unlockedVehicles [i].matId].mat;
					}
					break;
				}
			}
			carGameObject.transform.SetParent (mTransform, false);
			// update player current car 
			if (PlayerDataController.Instance.mPlayer.vehicleId == carGameObject.GetComponent <ArcadeCarController> ().vehicle.id) {
				carGameObject.SetActive (true);
				currentSelectedIndex = HandleCurrentIndex (vehicles.IndexOf (carGameObject) , 0);
				lastUnlockedIndex = currentSelectedIndex;

				vehicles [HandleCurrentIndex (currentSelectedIndex, -1)].transform.position = prevPos.position;
				vehicles [HandleCurrentIndex (currentSelectedIndex, -1)].transform.rotation = prevPos.rotation;
				vehicles [HandleCurrentIndex (currentSelectedIndex, -1)].SetActive (true);

				carGameObject.transform.localPosition = diskUp.position;

				carGameObject.transform.localScale = new Vector3 (carGameObject.GetComponent <ArcadeCarController> ().garageScale,carGameObject.GetComponent <ArcadeCarController> ().garageScale,carGameObject.GetComponent <ArcadeCarController> ().garageScale);
				curVehicleRotateId = LeanTween.rotateAroundLocal (carGameObject, Vector3.up, 360f, 10f).setLoopClamp().id;

				Debug.Log (carGameObject.GetComponent <ArcadeCarController> ().vehicle.matId);

				Messenger.Broadcast <Vehicle> (EventManager.GUI.UPDATE_VEHICLE.ToString (), carGameObject.GetComponent <ArcadeCarController> ().vehicle);
			} else if (currentSelectedIndex != 0) {
				vehicles [HandleCurrentIndex (currentSelectedIndex, 1)].transform.position = nextPos.position;
				vehicles [HandleCurrentIndex (currentSelectedIndex, 1)].transform.rotation = nextPos.rotation;
				vehicles [HandleCurrentIndex (currentSelectedIndex, 1)].SetActive (true);
			} 
			if (_vehicleName == GameConstant.fourWheels [GameConstant.fourWheels.Count - 1]) {
				_callback ();
			}
		}));
	}

	void SetCarPosition () {

	}

	#endregion Mono

	#region public functions
	#endregion public functions


	#region private functions
	int HandleCurrentIndex (int index, int increment) {
		int offset = 0;
		if ((index + increment) >= vehicles.Count) {
			offset = (index + increment) - vehicles.Count;
			index = 0;	
		} else if ((index + increment) < 0) {
			offset = index + increment;
			index = vehicles.Count;
		} else {
			// nothing
			offset = increment;
		}
		index += offset;
		return index;

	}

	public BezierSpline spline1;
	public BezierSpline spline2;
	public BezierSpline spline3;
	public BezierSpline spline4;
	bool valid = true;	// handle tween
	private void HandleSelectVehicle (int _index) {
		if (valid == false) return;
		valid = false;
		if (_index == 1)
			NextOne ();
		else
			PrevOne ();
	}

	void NextOne () {
		vehicles [HandleCurrentIndex (currentSelectedIndex, 2)].SetActive (true);
		StartCoroutine (MoveVehicle (vehicles [HandleCurrentIndex (currentSelectedIndex, 2)].transform, spline1, 1f, () => {
		}));

		LeanTween.scale (vehicles [HandleCurrentIndex (currentSelectedIndex, 1)], Vector3.zero, 1f).setEase (LeanTweenType.easeInQuint);
		StartCoroutine (MoveVehicle (vehicles [HandleCurrentIndex (currentSelectedIndex, 1)].transform, spline2, 1f, null));

		HandleElevator (vehicles [currentSelectedIndex], false, null);

		StartCoroutine (MoveVehicle (vehicles [HandleCurrentIndex (currentSelectedIndex, -1)].transform, spline4, 1f, () => { 
			vehicles [HandleCurrentIndex (currentSelectedIndex, -1)].SetActive (false);
			NextTwo(); 
		}));
	}

	void NextTwo () {
		LeanTween.scale (vehicles [currentSelectedIndex], Vector3.one, 1f).setEase (LeanTweenType.easeOutQuint);
		StartCoroutine (MoveVehicle (vehicles [currentSelectedIndex].transform, spline3, 1f, () => {
		}));

		HandleElevator (vehicles [HandleCurrentIndex (currentSelectedIndex, 1)], true, () => {
			
			currentSelectedIndex += 1;
			currentSelectedIndex = HandleCurrentIndex (currentSelectedIndex, 0);
			if (PlayerDataController.Instance.unlockedIds.Contains (vehicles [currentSelectedIndex].GetComponent <ArcadeCarController> ().vehicle.id))
				lastUnlockedIndex = currentSelectedIndex;
			valid = true;

			curVehicleRotateId = LeanTween.rotateAroundLocal (vehicles [currentSelectedIndex], Vector3.up, 360f, 10f).setLoopClamp().id;
		});
	}

	void PrevOne () {
		vehicles [HandleCurrentIndex (currentSelectedIndex, -2)].SetActive (true);
		StartCoroutine (MoveVehicle (vehicles [HandleCurrentIndex (currentSelectedIndex, -2)].transform, spline4, 1f,  () => { 
			PrevTwo(); 
		}, false));

		LeanTween.scale (vehicles [HandleCurrentIndex (currentSelectedIndex, -1)], Vector3.zero, 1f).setEase (LeanTweenType.easeInQuint);
		StartCoroutine (MoveVehicle (vehicles [HandleCurrentIndex (currentSelectedIndex, -1)].transform, spline3, 1f, () => {
		}, false));
		HandleElevator (vehicles [currentSelectedIndex], false, null);

		StartCoroutine (MoveVehicle (vehicles [HandleCurrentIndex (currentSelectedIndex, 1)].transform, spline1, 1f, () => {
			vehicles [HandleCurrentIndex (currentSelectedIndex, 1)].SetActive (false);
		}, false));
	}

	void PrevTwo () {
		LeanTween.scale (vehicles [currentSelectedIndex], Vector3.one, 1f);
		StartCoroutine (MoveVehicle (vehicles [currentSelectedIndex].transform, spline2, 1f, () => {
		}, false));

		HandleElevator (vehicles [HandleCurrentIndex (currentSelectedIndex, -1)], true, () => {
			currentSelectedIndex -= 1;
			currentSelectedIndex = HandleCurrentIndex (currentSelectedIndex, 0);
			if (PlayerDataController.Instance.unlockedIds.Contains (vehicles [currentSelectedIndex].GetComponent <ArcadeCarController> ().vehicle.id))
				lastUnlockedIndex = currentSelectedIndex;
			valid = true;
			curVehicleRotateId = LeanTween.rotateAroundLocal (vehicles [currentSelectedIndex], Vector3.up, 360f, 10f).setLoopClamp().id;
		});
	}

	// move vehicle
	IEnumerator MoveVehicle (Transform _trans, BezierSpline _spline, float _duration, System.Action _callback, bool _isGoingForward = true) {
		float progress = _isGoingForward ? 0 : 1;
		float _duration_ = 1f;
		while (true) {
			if (_isGoingForward) {
				progress += Time.deltaTime / _duration_;
				if (progress > 1f) {
					if (_callback != null)
						_callback ();
					yield break;
				}
			}else{
				progress -= Time.deltaTime / _duration_;
				if (progress < 0f) {
					if (_callback != null)
						_callback ();
					yield break;
				}
			}

			Vector3 position = _spline.GetPoint(progress);
			_trans.localPosition = position;
			_trans.LookAt(position + _spline.GetDirection(progress));
			yield return null;
		}
	}


	MainMenuController3D menu;
	private void HandleExitGarage () {
		if (menu == null){
			menu = FindObjectOfType <MainMenuController3D> ();
		}
		if (menu.IsInMenu) return;

		Debug.Log ("exit"+menu.IsInMenu);
		// check if current select car is not unlocked 
		if (!PlayerDataController.Instance.unlockedIds.Contains (vehicles [currentSelectedIndex].GetComponent <ArcadeCarController> ().vehicle.id)) {
			ClearVehicle ();
		}
	}

	void ClearVehicle () {
		LeanTween.move (vehicles [currentSelectedIndex], diskDown.position, .5f).setEase (LeanTweenType.easeInCirc).setOnComplete (() => {
			StartCoroutine (MoveVehicle (vehicles [currentSelectedIndex].transform, spline3, 1f, () => {
				StartCoroutine (MoveVehicle (vehicles [currentSelectedIndex].transform, spline4, 3f,  () => {
					vehicles [currentSelectedIndex].SetActive (false);
				}));
			}));

			StartCoroutine (MoveVehicle (vehicles [HandleCurrentIndex (currentSelectedIndex, -1)].transform, spline4, 3f, () => {
				vehicles [HandleCurrentIndex (currentSelectedIndex, -1)].SetActive (false);
			}));

			LeanTween.scale (vehicles [HandleCurrentIndex (currentSelectedIndex, 1)], Vector3.zero, 1f);
			StartCoroutine (MoveVehicle (vehicles [HandleCurrentIndex (currentSelectedIndex, 1)].transform, spline2, 1f, () => {
				LeanTween.scale (vehicles [HandleCurrentIndex (currentSelectedIndex, 1)], Vector3.one, 1f).setEase (LeanTweenType.easeOutQuint);
				StartCoroutine (MoveVehicle (vehicles [HandleCurrentIndex (currentSelectedIndex, 1)].transform, spline3, 1f, () => {
					StartCoroutine (MoveVehicle (vehicles [HandleCurrentIndex (currentSelectedIndex, 1)].transform, spline4, 3f, () => {
						vehicles [HandleCurrentIndex (currentSelectedIndex, 1)].SetActive (false);
						StartCoroutine (ReSetupVehicles ());
					}));
				}));
			}));
		});
		LeanTween.move (disk.gameObject, diskDown.position, .5f).setEase (LeanTweenType.easeInCirc);
		LeanTween.scale (vehicles [HandleCurrentIndex (currentSelectedIndex, 1)], Vector3.zero, 1f).setEase (LeanTweenType.easeInCirc);
	}

	IEnumerator ReSetupVehicles () {
		vehicles [lastUnlockedIndex - 1].SetActive (true);
		StartCoroutine (MoveVehicle (vehicles [lastUnlockedIndex - 1].transform, spline1, 1f, () => {
			LeanTween.scale (vehicles [lastUnlockedIndex - 1], Vector3.zero, 1f).setEase (LeanTweenType.easeInQuint);
			StartCoroutine (MoveVehicle (vehicles [lastUnlockedIndex - 1].transform, spline2, 1f, () => {
				LeanTween.scale (vehicles [lastUnlockedIndex - 1], Vector3.one, 1f).setEase (LeanTweenType.easeOutQuint);
				StartCoroutine (MoveVehicle (vehicles [lastUnlockedIndex - 1].transform, spline3, 1f, null));
			}));
		}));

		yield return new WaitForSeconds (.5f);
		vehicles [lastUnlockedIndex].SetActive (true);
		StartCoroutine (MoveVehicle (vehicles [lastUnlockedIndex].transform, spline1, 1f, () => {
			LeanTween.scale (vehicles [lastUnlockedIndex], Vector3.zero, 1f).setEase (LeanTweenType.easeInQuint);
			StartCoroutine (MoveVehicle (vehicles [lastUnlockedIndex].transform, spline2, 1f, () => {
				LeanTween.move (disk.gameObject, diskUp.position, .5f).setEase (LeanTweenType.easeInCirc);

				LeanTween.scale (vehicles [lastUnlockedIndex], Vector3.one, .5f).setEase (LeanTweenType.easeInCirc);
				LeanTween.move (vehicles [lastUnlockedIndex], diskUp.position, .5f).setEase (LeanTweenType.easeInCirc);


			}));
		}));

		yield return new WaitForSeconds (.5f);
		vehicles [lastUnlockedIndex + 1].SetActive (true);
		StartCoroutine (MoveVehicle (vehicles [lastUnlockedIndex + 1].transform, spline1, 1f, () => {
			currentSelectedIndex = lastUnlockedIndex;
		}));
	}

	void HandleElevator (GameObject _go, bool _isGoingUp, System.Action _callback) {
		float scaleFacetor = _go.GetComponent <ArcadeCarController> ().garageScale;
		if (_isGoingUp) {
			LeanTween.scale (_go, new Vector3 (scaleFacetor, scaleFacetor, scaleFacetor), .5f).setEase (LeanTweenType.easeOutQuint);
			LeanTween.move (_go, diskUp.position, .5f).setEase (LeanTweenType.easeOutQuint);	
			LeanTween.move (disk.gameObject, diskUp.position , .5f).setEase (LeanTweenType.easeOutQuint).setOnComplete (_callback);
		} else {
			LeanTween.scale (_go, Vector3.zero, .5f).setEase (LeanTweenType.easeInQuint).setOnComplete ( () => {
				LeanTween.cancel (curVehicleRotateId);
			});
			LeanTween.move (_go, diskDown.position, .5f).setEase (LeanTweenType.easeInQuint);	
			LeanTween.move (disk.gameObject, diskDown.position, .5f).setEase (LeanTweenType.easeInQuint).setOnComplete (_callback);
		}
	}

	// TODO: refactor spline codes

	void HandleSpline1 (GameObject _go, System.Action _calback, bool _isGoingForward = true) {
		if (_isGoingForward) {
			_go.SetActive (true);
			StartCoroutine (MoveVehicle (_go.transform, spline1, 2f, _calback, _isGoingForward));
		} else {
			StartCoroutine (MoveVehicle (_go.transform, spline1, 2f, () => { _go.SetActive (false); }, _isGoingForward));
		}
	}

	void HandleSpline2 (GameObject _go, System.Action _calback, bool _isGoingForward = true) {
		if (_isGoingForward) {
			LeanTween.scale (_go, Vector3.zero, 1f).setEase (LeanTweenType.easeInQuint);
			StartCoroutine (MoveVehicle (_go.transform, spline2, 1f, null, _isGoingForward));
		} else {
			LeanTween.scale (_go, Vector3.one, 1f).setEase (LeanTweenType.easeInQuint);
			StartCoroutine (MoveVehicle (_go.transform, spline2, 1f, null, _isGoingForward));
		}
	}

	void HandleSpline3 (GameObject _go, System.Action _calback, bool _isGoingForward = true) {
		if (_isGoingForward) {
			LeanTween.scale (_go, Vector3.one, 1f).setEase (LeanTweenType.easeInQuint);
			StartCoroutine (MoveVehicle (_go.transform, spline3, 1f, null, _isGoingForward));
		} else {
			LeanTween.scale (_go, Vector3.zero, 1f).setEase (LeanTweenType.easeInQuint);
			StartCoroutine (MoveVehicle (_go.transform, spline3, 1f, null, _isGoingForward));
		}
	}

	void HandleSpline4(GameObject _go, System.Action _calback, bool _isGoingForward = true) {
		if (_isGoingForward) {
			StartCoroutine (MoveVehicle (_go.transform, spline4, 3f, () => { _go.SetActive (false); }, _isGoingForward));
		} else {
			_go.SetActive (true);
			StartCoroutine (MoveVehicle (_go.transform, spline4, 3f, null, _isGoingForward));
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
			renderers[i].material = vehicles [currentSelectedIndex].GetComponent <ArcadeCarController> ().carMats[_matId].mat;
		}
		vehicles [currentSelectedIndex].GetComponent <ArcadeCarController> ().vehicle.matId = _matId;

		PlayerDataController.Instance.UpdateVehicle (vehicles [currentSelectedIndex].GetComponent <ArcadeCarController> ().vehicle);
	}
	#endregion private functions

}
