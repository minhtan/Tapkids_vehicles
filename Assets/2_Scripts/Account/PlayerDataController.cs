using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;

public class PlayerDataController : UnitySingletonPersistent<PlayerDataController> {

	public Player mPlayer;
	public List<int> unlockedIds = new List<int> ();
	public GameObject firstVehicle;

	private int currentPlayer = 0;
	#region MONO
	void Awake () {
		base.Awake ();
		if (TapkidsData.Load ()) {
			mPlayer = TapkidsData.GetPlayerById(currentPlayer);
		} else {
			// pre setup player vehicle
			List <Vehicle> newCarList = new List<Vehicle> ();
			if (firstVehicle != null)
				newCarList.Add (firstVehicle.GetComponent <ArcadeCarController> ().vehicle);

			mPlayer = new Player (0, 100, 2, "Car", newCarList); 

			TapkidsData.AddPlayer (mPlayer);
			TapkidsData.Save ();
		}

		for (int i = 0; i < mPlayer.unlockedVehicles.Count; i++) {
			unlockedIds.Add (mPlayer.unlockedVehicles[i].id);
		}
	}

	void OnEnable () {
		Messenger.AddListener <Vehicle> (EventManager.GUI.UPDATE_VEHICLE.ToString (), HandleSelectVehicle);
//		Messenger.AddListener (EventManager.GUI.PURCHASEVEHICLE.ToString (), HandlePurchaseVehicle);
	}

	void Start () {
		
	}

	void OnDisable () {
//		Messenger.RemoveListener <Vehicle> (EventManager.GUI.UPDATE_VEHICLE.ToString (), HandleSelectVehicle);
//		Messenger.RemoveListener (EventManager.GUI.PURCHASEVEHICLE.ToString (), HandlePurchaseVehicle);
	}
	#endregion MONO

	#region public functions

	public void GetPlayers () {
	}

	public void SelectPlayer (int id) {
	}


	public void UpdatePlayerName (string _name) {
		mPlayer.name = _name;
		TapkidsData.players [currentPlayer].name = mPlayer.name;
		TapkidsData.Save ();
	}

	public void UpdatePlayerCredit (int _point) {
		mPlayer.credit += _point;

		Messenger.Broadcast <int> (EventManager.GUI.UPDATE_CREDIT.ToString (), PlayerDataController.Instance.mPlayer.credit);

		TapkidsData.players [currentPlayer].credit = mPlayer.credit;
		TapkidsData.Save ();
	}

	public void UnlockVehicle (Vehicle _unlockedVehicle) {
		if (!mPlayer.unlockedVehicles.Contains (_unlockedVehicle)) 
			mPlayer.unlockedVehicles.Add (_unlockedVehicle);
		if (!unlockedIds.Contains (_unlockedVehicle.id)) 
			unlockedIds.Add (_unlockedVehicle.id);

		UpdatePlayerCredit (_unlockedVehicle.costPoint * -1);

		Messenger.Broadcast <string, float> (EventManager.GUI.NOTIFY.ToString (), GameConstant.PurchaseSuccessful, 1f);

		TapkidsData.players [currentPlayer].unlockedVehicles = mPlayer.unlockedVehicles;
		TapkidsData.Save ();
	}

	public void UpdateVehicle (Vehicle _vehicle) {
//		if (mPlayer.unlockedVehicles.Contains (_vehicle)) {
		for (int i = 0; i < mPlayer.unlockedVehicles.Count; i++) {
			if (mPlayer.unlockedVehicles [i].id == _vehicle.id) {
				mPlayer.unlockedVehicles [i].matId = _vehicle.matId;
				break;
			}
		}
//		} else {
//			return;
//		}
		TapkidsData.players[currentPlayer].unlockedVehicles = mPlayer.unlockedVehicles;
		TapkidsData.Save ();
	}
	#endregion public functions

	#region private functions
	void HandleSelectVehicle (Vehicle _newVehicle) {
		// check unlocked car
		if (PlayerDataController.Instance.unlockedIds.Contains (_newVehicle.id)) {
			mPlayer.vehicleId = _newVehicle.id;
			mPlayer.vehicleName = _newVehicle.name;
		} else {
			// do nothing
		}
	}


	#endregion private functions
}
