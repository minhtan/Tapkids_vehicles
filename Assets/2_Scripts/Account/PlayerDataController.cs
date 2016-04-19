using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;

public class PlayerDataController : UnitySingletonPersistent<PlayerDataController> {

	public Player mPlayer;

	#region MONO
	void Awake () {
		AddDemoData ();

		mPlayer = TapkidsData.GetPlayerById(0);
	}
	#endregion MONO

	#region public functions
	public void UpdatePlayerName (string _name) {
		mPlayer.name = _name;
	}

	public void UpdatePlayerCredit (int _point) {
		mPlayer.currentCredit += _point;
	}

	public void UpdatePlayerCurrentVehicle (Vehicle _newVehicle) {
		mPlayer.currentVehicle = _newVehicle;
	}

	public void UpdateUnlockedCar (Vehicle _unlockedVehicle) {
		mPlayer.unlockedVehicles.Add (_unlockedVehicle);
	}


	#endregion public functions



	#region demo data
	private void AddDemoData () {
		Vehicle police = new  Vehicle (0, "police", 100, 10);
		Vehicle ambulance = new Vehicle (1, "ambulance", 200, 20);
		List <Vehicle> unlockedVehicles= new List <Vehicle> ();

		unlockedVehicles.Add (police);
		unlockedVehicles.Add (ambulance);


		// demo player data
		Player demoPlayer1 = new Player (0, "fdj1", 10, police, unlockedVehicles);
//		Player demoPlayer2 = new Player (1, "fdj2", 20, 1, unlockedVehicles);

		TapkidsData.players.Add (demoPlayer1);
		TapkidsData.Save();

	}
	#endregion demo data
}
