using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;

public class PlayerDataController : UnitySingletonPersistent<PlayerDataController> {

	public Player mPlayer;

	private int currentPlayer;
	#region MONO
	void Awake () {
		base.Awake ();
		if (TapkidsData.Load ()) {
			mPlayer = TapkidsData.GetPlayerById(0);
		} else {
			List <int> newCarList = new List<int> ();
			newCarList.Add (0);
//			newCarList.Add (1);
//			newCarList.Add (2);
				
			mPlayer = new Player (0, 100, 1, newCarList); 

			TapkidsData.AddPlayer (mPlayer);
			TapkidsData.Save ();
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
		mPlayer.currentCredit += _point;
		TapkidsData.players [currentPlayer].currentCredit = mPlayer.currentCredit;
		TapkidsData.Save ();
	}

	public void UpdatePlayerCurrentVehicle (int _newVehicle) {
		mPlayer.currentVehicle = _newVehicle;
		TapkidsData.players [currentPlayer].currentVehicle = mPlayer.currentVehicle;
		TapkidsData.Save ();
	}

	public void UpdateUnlockedVehicle (Vehicle _unlockedVehicle) {
		mPlayer.unlockedVehicles.Add (_unlockedVehicle.id);
		Messenger.Broadcast <string, float> (EventManager.GUI.NOTIFY.ToString (), GameConstant.PurchaseSuccessful, 1f);

		UpdatePlayerCredit (_unlockedVehicle.costPoint * -1);
		TapkidsData.players [currentPlayer].unlockedVehicles = mPlayer.unlockedVehicles;
		TapkidsData.Save ();
	}
	#endregion public functions

	#region private functions
	void HandleSelectVehicle (Vehicle _newVehicle) {
		// check unlocked car
		if (mPlayer.unlockedVehicles.Contains (_newVehicle.id)) {
			mPlayer.currentVehicle = _newVehicle.id;
		} else {
			// do nothing
		}
	}


	#endregion private functions
}
