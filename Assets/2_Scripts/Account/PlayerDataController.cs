using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;

public class PlayerDataController : UnitySingletonPersistent<PlayerDataController> {

	public Player myPlayer2;
	string fileName = "player";
	string path = Application.persistentDataPath; // "Assets/Resources/Player";

	void Start () {
//		Vehicle[] vehicles= new Vehicle [2];
//
//		vehicles[0] = new Vehicle();
//		vehicles[0].name = "police";
//
//		vehicles[1] = new Vehicle();
//		vehicles[1].name = "ambulance";

		//
		Dictionary <int, Vehicle> unlockedVehicles = new Dictionary<int, Vehicle> ();
		unlockedVehicles.Add (1, new Vehicle ("police", 10));
		unlockedVehicles.Add (2, new Vehicle ("ambulance", 20));
		//

		Player myPlayer = new Player ();
		myPlayer.name = "fdj";
		myPlayer.currentVehicleIndex = 1;
		myPlayer.unlockedVehicles = unlockedVehicles;

		WriteJsonFile (JsonUtility.ToJson(myPlayer), fileName, path);
	}

	void WriteJsonFile (string data, string fileName, string directoryPath)
	{
		var sr = File.CreateText (directoryPath + "/" + fileName + ".json");
		sr.Write (data);
		sr.Close ();
	}

	string ReadJsonFile (string fileName, string directoryPath) {
		return File.ReadAllText (directoryPath + "/" + fileName + ".json");
	}

	void Update () {
		if (Input.GetKeyDown (KeyCode.Space)) {
			myPlayer2 = JsonUtility.FromJson <Player> (ReadJsonFile(fileName, path));
			Debug.Log (myPlayer2.name);
		}
	}
}
