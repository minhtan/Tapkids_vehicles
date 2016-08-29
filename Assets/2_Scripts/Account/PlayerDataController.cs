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
	public override void Awake () {
		base.Awake ();

		Debug.Log (Application.persistentDataPath);
		if (TapkidsData.Load ()) {
			mPlayer = TapkidsData.GetPlayerById(currentPlayer);
		} else {
			// pre setup player vehicle
//			List <int> newCarList = new List<int> ();
			Dictionary <int, int> newCarList = new Dictionary<int, int> ();
			if (firstVehicle != null)
				newCarList.Add (firstVehicle.GetComponent <ArcadeCarController> ().vehicle.id, 0);

			bool[] tutStates = {false, false};
			mPlayer = new Player (0, 2000, 0, "Ambulance", newCarList, tutStates);

			TapkidsData.AddPlayer (mPlayer);
			TapkidsData.Save ();
		}
		foreach (KeyValuePair <int, int> d in mPlayer.unlockedVehicles) {
			unlockedIds.Add (d.Key);
		}
	}

	void OnEnable () {
	}

	void Start () {
		
	}

	void OnDisable () {
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
		if (!mPlayer.unlockedVehicles.ContainsKey (_unlockedVehicle.id)) 
			mPlayer.unlockedVehicles.Add (_unlockedVehicle.id, 0);
		if (!unlockedIds.Contains (_unlockedVehicle.id)) 
			unlockedIds.Add (_unlockedVehicle.id);

		UpdatePlayerCredit (_unlockedVehicle.costPoint * -1);

		Messenger.Broadcast <string, float> (EventManager.GUI.NOTIFY.ToString (), GameConstant.PurchaseSuccessful, 1f);

		TapkidsData.players [currentPlayer].unlockedVehicles = mPlayer.unlockedVehicles;
		TapkidsData.Save ();
	}

	public void UpdateCurrentVehicle (Vehicle _vehicle) {
		mPlayer.vehicleId = _vehicle.id;
		mPlayer.vehicleName = _vehicle.name;
		TapkidsData.players[currentPlayer] = mPlayer;
		TapkidsData.Save ();
	}
	public void UpdateVehicleMaterial (Vehicle _vehicle) {
		mPlayer.unlockedVehicles[_vehicle.id] = _vehicle.matId;
		TapkidsData.players[currentPlayer].unlockedVehicles = mPlayer.unlockedVehicles;
		TapkidsData.Save ();
	}

	public void UpdatePlayerTutorialState (int gameIndex, bool _state) {
		for (int i = 0; i < mPlayer.playedTuts.Length; i++) {
			if (i == gameIndex) {
				mPlayer.playedTuts[i] = _state;
			}
		}
		TapkidsData.players[currentPlayer].playedTuts = mPlayer.playedTuts;
		TapkidsData.Save ();
	}
	#endregion public functions

	#region private functions
	#endregion private functions
}
