using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;

public class PlayerDataController : UnitySingletonPersistent<PlayerDataController> {

	public Player mPlayer;
	public List<int> unlockedIds = new List<int> ();

	private int currentPlayer = 0;
	#region MONO
	void Awake () {
		base.Awake ();
		if (TapkidsData.Load ()) {
			mPlayer = TapkidsData.GetPlayerById(currentPlayer);
		} else {
			// pre setup player vehicle
			Vehicle firstVehicle = new Vehicle (2, "Car", "", 0, 10);
			List <Vehicle> newCarList = new List<Vehicle> ();
			newCarList.Add (firstVehicle);

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
		Messenger.RemoveListener <Vehicle> (EventManager.GUI.UPDATE_VEHICLE.ToString (), HandleSelectVehicle);
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
		TapkidsData.players [currentPlayer].credit = mPlayer.credit;
		TapkidsData.Save ();
	}

	public void UpdatePlayerCurrentVehicle (int _newVehicle) {
		mPlayer.vehicleId = _newVehicle;
		TapkidsData.players [currentPlayer].vehicleId = mPlayer.vehicleId;
		TapkidsData.Save ();
	}

	public void UpdateUnlockedVehicle (Vehicle _unlockedVehicle) {
		mPlayer.unlockedVehicles.Add (_unlockedVehicle);
		unlockedIds.Add (_unlockedVehicle.id);
		Messenger.Broadcast <string, float> (EventManager.GUI.NOTIFY.ToString (), GameConstant.PurchaseSuccessful, 1f);

		UpdatePlayerCredit (_unlockedVehicle.costPoint * -1);
		TapkidsData.players [currentPlayer].unlockedVehicles = mPlayer.unlockedVehicles;
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
