using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;

public class PlayerDataController : UnitySingletonPersistent<PlayerDataController> {

	public Player mPlayer;
	string fileName = "player";

	#region MONO
	void Awake () {
//		mPlayer = JsonUtility.FromJson <Player> (ReadJsonFile(fileName, Application.persistentDataPath));
		AddDemoData ();

		mPlayer = TapkidsData.GetPlayerById(0);
	}
	#endregion MONO

	#region public functions

	#endregion public functions

	#region JSON functions
	private void WriteJsonFile (string fileName, string data)
	{
		var sr = File.CreateText (Application.persistentDataPath + "/" + fileName + ".json");
		sr.Write (data);
		sr.Close ();
	}

	private string ReadJsonFile (string fileName, string directoryPath) {
		return File.ReadAllText (Application.persistentDataPath + "/" + fileName + ".json");
	}
	#endregion JSON functions

	#region demo data
	private void AddDemoData () {
		Vehicle[] unlockedVehicles= new Vehicle [2];

		unlockedVehicles[0] = new Vehicle();
		unlockedVehicles[0].name = "police";
		unlockedVehicles[0].maxSpeed = 100;
		unlockedVehicles[0].costPoint = 10;

		unlockedVehicles[1] = new Vehicle();
		unlockedVehicles[1].name = "ambulance";
		unlockedVehicles[0].maxSpeed = 200;
		unlockedVehicles[1].costPoint = 20;

// 		ultility json has not supported dictionary
//		Dictionary <int, Vehicle> unlockedVehicles = new Dictionary<int, Vehicle> ();
//		unlockedVehicles.Add (1, new Vehicle ("police", 10));
//		unlockedVehicles.Add (2, new Vehicle ("ambulance", 20));

		// demo player data
		Player demoPlayer1 = new Player (0, "fdj1", 10, 0, unlockedVehicles);
//		Player demoPlayer2 = new Player (1, "fdj2", 20, 1, unlockedVehicles);

//		List <Player> players = new List <Player> ();
//		players.Add(demoPlayer1);

//		WriteJsonFile (fileName, JsonUtility.ToJson(demoPlayer1));
		TapkidsData.players.Add (demoPlayer1);
		TapkidsData.Save();

	}
	#endregion demo data
}
