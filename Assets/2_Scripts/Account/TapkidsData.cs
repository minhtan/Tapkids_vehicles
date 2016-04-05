using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System;

[System.Serializable]
public class Player {
	public int id;
	public string name;
	public int currentCredit;
	public Vehicle currentVehicle;
//	public Vehicle[] unlockedVehicles;
	public List <Vehicle> unlockedVehicles;

	public Player () {
	}

	public Player (int _id, string _name, int _currentCredit, Vehicle _currentVehicle, List <Vehicle> _unlockedVehicles) {
		this.id = _id;
		this.name = _name;
		this.currentCredit = _currentCredit;
		this.currentVehicle = _currentVehicle;
		this.unlockedVehicles = _unlockedVehicles;
	}
}

[System.Serializable]
public class Vehicle {
	public int id;
	public string name;
	public int maxSpeed;
	public int costPoint;
	// TODO: ... customize color index

	public Vehicle () {
	}

	public Vehicle (int _id, string _name, int _speed, int _costPoint) {
		this.id = _id;
		this.name = _name;
		this.maxSpeed = _speed;
		this.costPoint = _costPoint;
	}
}

public static class TapkidsData {
	public static List<Player> players = new List<Player>();

	#region text file functions
	public static void AddPlayer (Player _player) {
		players.Add (_player);
	}

	public static Player GetPlayerById (int _id) {
		for (int i = 0; i < players.Count; i++) {
			if (players[i].id == _id) {
				return players[i];
			}
		}
		return null;
	}

	// update score, current car, unlocked car ....
	public static void UpdatePlayerById (int _id, Player _newPlayer) {
		for (int i = 0; i < players.Count; i++) {
			if (players[i].id == _id) {
				players[i] = _newPlayer;
			}
		}
	}

	public static bool RemovePlayerById (int _id) {
		for (int i = 0; i < players.Count; i++) {
			if (players[i].id == _id) {
				players.RemoveAt(i);
				return true;
			}
		}
		return false;
	}



	public static bool RemoveAllPlayer () {
		players.Clear ();
		return true;
	}

	public static void Save() {
		BinaryFormatter bf = new BinaryFormatter();
		FileStream file = File.Create (Application.persistentDataPath + "/player.tapkids");
		bf.Serialize(file, players);
		file.Close();
	}

	public static void Load() {
		if(File.Exists(Application.persistentDataPath + "/player.tapkids")) {
			BinaryFormatter bf = new BinaryFormatter();
			FileStream file = File.Open(Application.persistentDataPath + "/player.tapkids", FileMode.Open);
			players = (List<Player>)bf.Deserialize(file);
			file.Close();
		}
	}
	#endregion text file functions
}
