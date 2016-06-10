using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;
public class GaragaController : MonoBehaviour {

	#region public members
	public Material lockedMaterial;
	public int currentSelectedIndex;
	public Transform disk;

	public Transform initPos;
	public Transform nextPos;
	public Transform prevPos;
	public Transform diskDown;
	public Transform diskUp;

	public GameObject locker;
	public GameObject purchaseButton;

	public AudioClip carMove;
	public AudioClip carUnlock;
	#endregion public members

	#region private members
	public List<GameObject> vehicles;
	private Transform mTransform;

	private int lastUnlockedIndex;

	private float forwardPos = 20f;
	private float backwardPos = -20f;

	private int curVehicleRotateId;

	MainMenuController3D menu;
	#endregion private members

	#region Mono
	void Awake () {
		mTransform = GetComponent <Transform> ();
	}

	void OnEnable () {
		Messenger.AddListener <int> (EventManager.GUI.SELECT_VEHICLE.ToString (), HandleSelectVehicle);
		Messenger.AddListener (EventManager.GUI.TO_MENU.ToString (), HandleExitGarage);
		Messenger.AddListener (EventManager.GUI.PURCHASE_VEHICLE.ToString (), HandlePurchaseVehicle);
		Messenger.AddListener <int> (EventManager.GUI.CHANGE_MATERIAL.ToString (), HandleChangeMaterial);
	}

	void OnDisable () {
		Messenger.RemoveListener <int> (EventManager.GUI.SELECT_VEHICLE.ToString (), HandleSelectVehicle);
		Messenger.RemoveListener (EventManager.GUI.TO_MENU.ToString (), HandleExitGarage);
		Messenger.RemoveListener (EventManager.GUI.PURCHASE_VEHICLE.ToString (), HandlePurchaseVehicle);
		Messenger.RemoveListener <int> (EventManager.GUI.CHANGE_MATERIAL.ToString (), HandleChangeMaterial);
	}

	void Start () {
		menu = FindObjectOfType <MainMenuController3D> ();
		vehicles = new List <GameObject> ();
		// get player unlocked list

		// compare with avaiable vehicle list

		// then setup garage
//		Invoke ("SetupGarage", 1f);
		SetupGarage ();
	}
	void SetupGarage () {
		for (int i = 0; i < GameConstant.fourWheels.Count; i++) {
			StartCoroutine (SetupVehicles (GameConstant.fourWheels[i], () => {
				Invoke ("SetupCurrentVehicle", 1f);
			}));
		}
	}

	void SetupCurrentVehicle () {
		for (int i = 0; i < vehicles.Count; i++) {
			int vehicleId = vehicles[i].GetComponent <ArcadeCarController> ().vehicle.id;
			if (vehicleId == PlayerDataController.Instance.mPlayer.vehicleId) {
				vehicles[vehicleId].SetActive (true);
				currentSelectedIndex = i;
				lastUnlockedIndex = currentSelectedIndex;

				vehicles[vehicleId].transform.localPosition = diskUp.position;
				vehicles[vehicleId].transform.localScale = vehicles[currentSelectedIndex].GetComponent <ArcadeCarController> ().vehicle.garageScale.ToVector3 ();
				curVehicleRotateId = LeanTween.rotateAroundLocal (vehicles[currentSelectedIndex], Vector3.up, 360f, 10f).setLoopClamp().id;

				Messenger.Broadcast <Vehicle> (EventManager.GUI.UPDATE_VEHICLE.ToString (), vehicles[currentSelectedIndex].GetComponent <ArcadeCarController> ().vehicle);
			} 
			else if (vehicleId == HandleCurrentIndex (PlayerDataController.Instance.mPlayer.vehicleId, 1)) {
				vehicles [vehicleId].transform.position = nextPos.position;
				vehicles [vehicleId].transform.rotation = nextPos.rotation;
				vehicles [vehicleId].SetActive (true);
			} 
			else if (vehicleId == HandleCurrentIndex (PlayerDataController.Instance.mPlayer.vehicleId, -1)) {
				vehicles [vehicleId].transform.position = prevPos.position;
				vehicles [vehicleId].transform.rotation = prevPos.rotation;
				vehicles [vehicleId].SetActive (true);
			}
		}
	}
	#endregion Mono

	#region public functions
	#endregion public functions

	#region private functions
	private IEnumerator SetupVehicles (string _vehicleName, System.Action _callback = null) {
		yield return new WaitForSeconds (0f);
		StartCoroutine (AssetController.Instance.InstantiateGameObjectAsync (GameConstant.assetBundleName, _vehicleName, (bundle) => {
			GameObject carGameObject = Instantiate (bundle, initPos.position, Quaternion.identity) as GameObject;
			int carId = carGameObject.GetComponent <ArcadeCarController> ().vehicle.id;
			vehicles.Add (carGameObject);
			carGameObject.GetComponent <Rigidbody> ().isKinematic = true;
			carGameObject.SetActive (false);
			carGameObject.transform.SetParent (mTransform, false);

			//			// apply lock shader
			if (!PlayerDataController.Instance.unlockedIds.Contains (carId)) {
				Renderer[] renderers = carGameObject.GetComponentsInChildren <Renderer> ();
				for (int j = 0; j < renderers.Length; j++) {
					renderers [j].material = lockedMaterial;
				}
			} 
			// apply current selected material
			if (PlayerDataController.Instance.mPlayer.unlockedVehicles.ContainsKey (carId)) {
				int _matID;
				if (PlayerDataController.Instance.mPlayer.unlockedVehicles.TryGetValue (carId, out _matID)) {
					carGameObject.GetComponent <ArcadeCarController> ().vehicle.matId = _matID;
					Renderer [] renderers = carGameObject.GetComponentsInChildren <Renderer> ();
					for (int j = 0; j < renderers.Length; j++) {
						renderers [j].material = carGameObject.GetComponent <ArcadeCarController> ().vehicle.carMats [_matID].mat;
					}
				}
			}
		}));

		yield return new WaitForSeconds (.5f);
		if (_vehicleName == GameConstant.fourWheels [GameConstant.fourWheels.Count - 1]) {
			_callback ();
		}
	}

	public BezierSpline spline1;
	public BezierSpline spline2;
	public BezierSpline spline3;
	public BezierSpline spline4;
	bool valid = true;	// handle tween

	private void HandleSelectVehicle (int _index) {
		if (valid == false) return;
		purchaseButton.SetActive (false);
		menu.SetTweenLock (true);
		valid = false;
		AudioManager.Instance.PlayTemp (carMove);

		if (_index == 1)
			NextOne ();
		else
			PrevOne ();
	}

	private int HandleCurrentIndex (int index, int increment) {
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

	private void NextOne () {
		//
		vehicles [HandleCurrentIndex (currentSelectedIndex, 2)].SetActive (true);
		StartCoroutine (MoveVehicle (vehicles [HandleCurrentIndex (currentSelectedIndex, 2)].transform, spline1));

		//
		LeanTween.scale (vehicles [HandleCurrentIndex (currentSelectedIndex, 1)], Vector3.zero, .5f).setEase (LeanTweenType.easeInQuint);
		StartCoroutine (MoveVehicle (vehicles [HandleCurrentIndex (currentSelectedIndex, 1)].transform, spline2));

		HandleElevator (vehicles [currentSelectedIndex], false);
		//
		StartCoroutine (MoveVehicle (vehicles [HandleCurrentIndex (currentSelectedIndex, -1)].transform, spline4, () => { 
			vehicles [HandleCurrentIndex (currentSelectedIndex, -1)].SetActive (false);
			NextTwo(); 
		}));
	}

	private void NextTwo () {
		//
		LeanTween.scale (vehicles [currentSelectedIndex], Vector3.one, .5f).setEase (LeanTweenType.easeOutQuint);
		StartCoroutine (MoveVehicle (vehicles [currentSelectedIndex].transform, spline3, () => {
		}));

		//
		HandleElevator (vehicles [HandleCurrentIndex (currentSelectedIndex, 1)], true, () => {
			currentSelectedIndex += 1;
			currentSelectedIndex = HandleCurrentIndex (currentSelectedIndex, 0);
			if (PlayerDataController.Instance.unlockedIds.Contains (vehicles [currentSelectedIndex].GetComponent <ArcadeCarController> ().vehicle.id)) {
				lastUnlockedIndex = currentSelectedIndex;
				purchaseButton.SetActive (false);
			} else {
				purchaseButton.SetActive (true);
			}
			valid = true;
			Messenger.Broadcast <Vehicle> (EventManager.GUI.UPDATE_VEHICLE.ToString (), vehicles [currentSelectedIndex].GetComponent <ArcadeCarController> ().vehicle);
			curVehicleRotateId = LeanTween.rotateAroundLocal (vehicles [currentSelectedIndex], Vector3.up, 360f, 10f).setLoopClamp().id;
			menu.SetTweenLock (false);
		});
	}

	private void PrevOne () {
		vehicles [HandleCurrentIndex (currentSelectedIndex, -2)].SetActive (true);
		StartCoroutine (MoveVehicle (vehicles [HandleCurrentIndex (currentSelectedIndex, -2)].transform, spline4, () => { 
			PrevTwo(); 
		}, false));

		LeanTween.scale (vehicles [HandleCurrentIndex (currentSelectedIndex, -1)], Vector3.zero, .5f).setEase (LeanTweenType.easeInQuint);
		StartCoroutine (MoveVehicle (vehicles [HandleCurrentIndex (currentSelectedIndex, -1)].transform, spline3, () => {
		}, false));
		HandleElevator (vehicles [currentSelectedIndex], false);

		StartCoroutine (MoveVehicle (vehicles [HandleCurrentIndex (currentSelectedIndex, 1)].transform, spline1, () => {
			vehicles [HandleCurrentIndex (currentSelectedIndex, 1)].SetActive (false);
		}, false));
	}

	private void PrevTwo () {
		LeanTween.scale (vehicles [currentSelectedIndex], Vector3.one, .5f);
		StartCoroutine (MoveVehicle (vehicles [currentSelectedIndex].transform, spline2, () => {
		}, false));

		HandleElevator (vehicles [HandleCurrentIndex (currentSelectedIndex, -1)], true, () => {
			currentSelectedIndex -= 1;
			currentSelectedIndex = HandleCurrentIndex (currentSelectedIndex, 0);
			if (PlayerDataController.Instance.unlockedIds.Contains (vehicles [currentSelectedIndex].GetComponent <ArcadeCarController> ().vehicle.id)) {
				lastUnlockedIndex = currentSelectedIndex;
				purchaseButton.SetActive (false);
			} else {

				purchaseButton.SetActive (true);
			}
			valid = true;
			Messenger.Broadcast <Vehicle> (EventManager.GUI.UPDATE_VEHICLE.ToString (), vehicles [currentSelectedIndex].GetComponent <ArcadeCarController> ().vehicle);
			curVehicleRotateId = LeanTween.rotateAroundLocal (vehicles [currentSelectedIndex], Vector3.up, 360f, 10f).setLoopClamp().id;
			menu.SetTweenLock (false);
		});
	}

	// move vehicle
	IEnumerator MoveVehicle (Transform _trans, BezierSpline _spline, System.Action _callback = null, bool _isGoingForward = true) {
		float progress = _isGoingForward ? 0 : 1;
		float _duration_ = .5f;
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

	private void HandleExitGarage () {
		if (menu.IsInMenu) return;
		purchaseButton.SetActive (false);

		// check if current select car is not unlocked 
		if (!PlayerDataController.Instance.unlockedIds.Contains (vehicles [currentSelectedIndex].GetComponent <ArcadeCarController> ().vehicle.id)) {
			
			menu.SetTweenLock (true);
			if (currentSelectedIndex > lastUnlockedIndex && currentSelectedIndex - lastUnlockedIndex <= 1) {
				PrevOne ();
			} else if (lastUnlockedIndex >  currentSelectedIndex && lastUnlockedIndex - currentSelectedIndex <= 1) {
				NextOne ();
			} else if (currentSelectedIndex == vehicles.Count - 1 && lastUnlockedIndex == 0) {
				NextOne ();
			} else if ( lastUnlockedIndex == vehicles.Count - 1 && currentSelectedIndex == 0) {
				PrevOne ();
			}else {
				ClearVehicle ();
			}
			locker.SetActive (false);
		} else {
			PlayerDataController.Instance.UpdateCurrentVehicle (vehicles [currentSelectedIndex].GetComponent <ArcadeCarController> ().vehicle);
		}
	}

	private void ClearVehicle () {
		AudioManager.Instance.PlayTemp (carMove);
		HandleElevator (vehicles [currentSelectedIndex], false, () => {
			StartCoroutine (MoveVehicle (vehicles [HandleCurrentIndex (currentSelectedIndex, -1)].transform, spline4, () => {
				vehicles [HandleCurrentIndex (currentSelectedIndex, -1)].SetActive (false);
			}));

			LeanTween.scale (vehicles [currentSelectedIndex], Vector3.one, .5f).setEase (LeanTweenType.easeOutQuint);
			StartCoroutine (MoveVehicle (vehicles [currentSelectedIndex].transform, spline3, () => {
				StartCoroutine (MoveVehicle (vehicles [currentSelectedIndex].transform, spline4, () => {
					vehicles [currentSelectedIndex].SetActive (false);
				}));
			}));
			LeanTween.scale (vehicles [HandleCurrentIndex (currentSelectedIndex, 1)], Vector3.zero, .5f).setEase (LeanTweenType.easeInQuint);
			StartCoroutine (MoveVehicle (vehicles [HandleCurrentIndex (currentSelectedIndex, 1)].transform, spline2, () => {
				LeanTween.scale (vehicles [HandleCurrentIndex (currentSelectedIndex, 1)], Vector3.one, .5f).setEase (LeanTweenType.easeOutQuint);
				StartCoroutine (MoveVehicle (vehicles [HandleCurrentIndex (currentSelectedIndex, 1)].transform, spline3, () => {
					StartCoroutine (MoveVehicle (vehicles [HandleCurrentIndex (currentSelectedIndex, 1)].transform, spline4, () => {
						vehicles [HandleCurrentIndex (currentSelectedIndex, 1)].SetActive (false);
						StartCoroutine (ReSetupVehicles ());
					}));
				}));
			}));
		});
	}

	private IEnumerator ReSetupVehicles () {
		vehicles [HandleCurrentIndex (lastUnlockedIndex, -1)].SetActive (true);
		AudioManager.Instance.PlayTemp (carMove);
		StartCoroutine (MoveVehicle (vehicles [HandleCurrentIndex (lastUnlockedIndex, -1)].transform, spline1, () => {
			LeanTween.scale (vehicles [HandleCurrentIndex (lastUnlockedIndex, -1)], Vector3.zero, .5f).setEase (LeanTweenType.easeInQuint);
			StartCoroutine (MoveVehicle (vehicles [HandleCurrentIndex (lastUnlockedIndex, -1)].transform, spline2, () => {
				LeanTween.scale (vehicles [HandleCurrentIndex (lastUnlockedIndex, -1)], Vector3.one, .5f).setEase (LeanTweenType.easeOutQuint);
				StartCoroutine (MoveVehicle (vehicles [HandleCurrentIndex (lastUnlockedIndex, -1)].transform, spline3, () => { menu.SetTweenLock (false); }));
			}));
		}));

		yield return new WaitForSeconds (.25f);
		vehicles [HandleCurrentIndex (lastUnlockedIndex, 0)].SetActive (true);
		StartCoroutine (MoveVehicle (vehicles [HandleCurrentIndex (lastUnlockedIndex, 0)].transform, spline1, () => {
			LeanTween.scale (vehicles [HandleCurrentIndex (lastUnlockedIndex, 0)], Vector3.zero, .5f).setEase (LeanTweenType.easeInQuint);
			StartCoroutine (MoveVehicle (vehicles [HandleCurrentIndex (lastUnlockedIndex, 0)].transform, spline2, () => {
				HandleElevator (vehicles [HandleCurrentIndex (lastUnlockedIndex, 0)], true, () => {
					currentSelectedIndex = HandleCurrentIndex (lastUnlockedIndex, 0);
					Messenger.Broadcast <Vehicle> (EventManager.GUI.UPDATE_VEHICLE.ToString (), vehicles [currentSelectedIndex].GetComponent <ArcadeCarController> ().vehicle);
					curVehicleRotateId = LeanTween.rotateAroundLocal (vehicles [HandleCurrentIndex (currentSelectedIndex, 0)], Vector3.up, 360f, 10f).setLoopClamp().id;
				});
			}));
		}));

		yield return new WaitForSeconds (.25f);
		vehicles [HandleCurrentIndex (lastUnlockedIndex, 1)].SetActive (true);
		StartCoroutine (MoveVehicle (vehicles [HandleCurrentIndex (lastUnlockedIndex, 1)].transform, spline1));
	}

	private void HandleElevator (GameObject _go, bool _isGoingUp, System.Action _callback = null) {
		float scaleFacetor = _go.GetComponent <ArcadeCarController> ().vehicle.garageScale;
		bool isUnlocked = PlayerDataController.Instance.mPlayer.unlockedVehicles.ContainsKey (_go.GetComponent<ArcadeCarController> ().vehicle.id);
		float _duration = .25f;
		if (_isGoingUp) {
			LeanTween.scale (_go, isUnlocked ? scaleFacetor.ToVector3() : (scaleFacetor * .7f).ToVector3 (), _duration).setEase (LeanTweenType.easeOutQuint).setOnComplete ( () => {
				HandleLocker (_go);
			});
			LeanTween.move (_go, diskUp.position, _duration).setEase (LeanTweenType.easeOutQuint);	
			LeanTween.move (disk.gameObject, diskUp.position , _duration).setEase (LeanTweenType.easeOutQuint).setOnComplete (_callback);
		} else {
			locker.SetActive (false);
			LeanTween.scale (_go, Vector3.zero, _duration).setEase (LeanTweenType.easeInQuint).setOnComplete ( () => {
				LeanTween.cancel (curVehicleRotateId);
			});
			LeanTween.move (_go, diskDown.position, _duration).setEase (LeanTweenType.easeInQuint).setOnComplete (_callback);	
			LeanTween.move (disk.gameObject, diskDown.position, _duration).setEase (LeanTweenType.easeInQuint);
		}
	}

	private void HandleLocker (GameObject _go) {
		if (!PlayerDataController.Instance.mPlayer.unlockedVehicles.ContainsKey (_go.GetComponent<ArcadeCarController> ().vehicle.id)) {
			locker.SetActive (true);
		} else {
			locker.SetActive (false);
		}
		///
		BoxCollider body = _go.transform.Find ("Colliders").GetComponent <BoxCollider> ();
		float offset = 5.5f;
		Vector3 lockerPos = new Vector3 (body.bounds.center.x, body.bounds.size.y + offset, body.bounds.center.z);
		locker.transform.position = lockerPos;
	}

	private void HandlePurchaseVehicle () {
		// check cost
		if (PlayerDataController.Instance.mPlayer.credit >= vehicles[currentSelectedIndex].GetComponent <ArcadeCarController> ().vehicle.costPoint) {
			PlayerDataController.Instance.UnlockVehicle (vehicles[currentSelectedIndex].GetComponent <ArcadeCarController> ().vehicle);
			lastUnlockedIndex = currentSelectedIndex;

			// apply material
			int _matID;
			if (PlayerDataController.Instance.mPlayer.unlockedVehicles.TryGetValue (vehicles[currentSelectedIndex].GetComponent <ArcadeCarController> ().vehicle.id, out _matID)) {
				Renderer[] renderers = vehicles[currentSelectedIndex].GetComponentsInChildren <Renderer> ();
				for (int j = 0; j < renderers.Length; j++) {
					renderers [j].material =  vehicles[currentSelectedIndex].GetComponent <ArcadeCarController> ().vehicle.carMats [_matID].mat;
				}
			}

			// handle fx, sfx, gui 
			AudioManager.Instance.PlayTemp (carUnlock);
			LeanTween.scale (vehicles [currentSelectedIndex], vehicles [currentSelectedIndex].GetComponent <ArcadeCarController> ().vehicle.garageScale.ToVector3 (), .5f).setEase (LeanTweenType.easeOutBack);
			locker.SetActive (false);
			purchaseButton.SetActive (false);
			StartCoroutine (ShowPurchaseMessage ());

			// broadcast event > update gui
			Messenger.Broadcast <Vehicle> (EventManager.GUI.UPDATE_VEHICLE.ToString (), vehicles[currentSelectedIndex].GetComponent <ArcadeCarController> ().vehicle);

		} else {
			GUIController.Instance.OpenDialog ("Khong du tien!!!")
				.AddButton ("Ok", UIDialogButton.Anchor.BOTTOM_CENTER);
		}
	}

	public GameObject purchaseMessage;
	IEnumerator ShowPurchaseMessage(){
		purchaseMessage.SetActive (true);
		yield return new WaitForSeconds (2f);
		purchaseMessage.SetActive (false);
	}

	void HandleChangeMaterial (int _matId) {
		if (!PlayerDataController.Instance.unlockedIds.Contains (vehicles [currentSelectedIndex].GetComponent <ArcadeCarController> ().vehicle.id)) return;

		// apply mat
		Renderer[] renderers = vehicles [currentSelectedIndex].GetComponentsInChildren<Renderer> ();
		for (int i = 0; i < renderers.Length; i++) {
			renderers[i].material = vehicles [currentSelectedIndex].GetComponent <ArcadeCarController> ().vehicle.carMats[_matId].mat;
		}

		// update temp data
		vehicles [currentSelectedIndex].GetComponent <ArcadeCarController> ().vehicle.matId = _matId;

		// update backend data
		PlayerDataController.Instance.UpdateVehicleMaterial (vehicles [currentSelectedIndex].GetComponent <ArcadeCarController> ().vehicle);

		// broadcast event > update gui
		Messenger.Broadcast <Vehicle> (EventManager.GUI.UPDATE_VEHICLE.ToString (), vehicles [currentSelectedIndex].GetComponent <ArcadeCarController> ().vehicle);
	}
	#endregion private functions

}
