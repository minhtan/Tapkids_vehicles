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
		mPlayer = new Player ();

		TapkidsData.Load ();
		mPlayer = TapkidsData.GetPlayerById(0);

		if (mPlayer == null) {
			TapkidsData.players.Add (mPlayer);
			TapkidsData.Save();
		} else {
			currentPlayer = mPlayer.id;
		}

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

	public void UpdatePlayerCurrentVehicle (Vehicle _newVehicle) {
		mPlayer.currentVehicle = _newVehicle;
		TapkidsData.players [currentPlayer].currentVehicle = mPlayer.currentVehicle;
		TapkidsData.Save ();
	}

	public void UpdateUnlockedVehicle (Vehicle _unlockedVehicle) {
		mPlayer.unlockedVehicles.Add (_unlockedVehicle);
		TapkidsData.players [currentPlayer].unlockedVehicles = mPlayer.unlockedVehicles;
		TapkidsData.Save ();
	}
	#endregion public functions

	#region demo data

	#endregion demo data
}
