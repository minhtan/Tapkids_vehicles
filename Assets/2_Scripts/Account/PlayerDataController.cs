using UnityEngine;
using System.Collections;
using System.IO;
using System;

public class PlayerDataController : MonoBehaviour {

	Player myPlayer2;
	string fileName = "player";
	string path = "Assets/Resources/Player";

	void Start () {
		Vehicle[] vehicles= new Vehicle [2];

		vehicles[0] = new Vehicle();
		vehicles[0].id = 1;
		vehicles[0].name = "police";

		vehicles[1] = new Vehicle();
		vehicles[1].id = 2;
		vehicles[1].name = "ambulance";

		Player myPlayer = new Player ();
		myPlayer.name = "fdj";
		myPlayer.unlockeds = vehicles;
		myPlayer.currentVehicle = 1;

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

	// get and set data for player: name, score, car
	// 
		
}
