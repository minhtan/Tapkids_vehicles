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
	public int credit;
	public int vehicleId;
	public string vehicleName;
	public List <int> unlockedVehicles;

	public Player () {
	}

	public Player (int _id, int _currentCredit, int _currentVehicleId, string _currentVehicleName, List <int> _unlockedVehicles) {
		this.id = _id;
		this.credit = _currentCredit;
		this.vehicleId = _currentVehicleId;
		this.vehicleName = _currentVehicleName;
		this.unlockedVehicles = _unlockedVehicles;
	}

	public Player (int _id, string _name, int _currentCredit, int _currentVehicleId, string _currentVehicleName, List <int> _unlockedVehicles) {
		this.id = _id;
		this.name = _name;
		this.credit = _currentCredit;
		this.vehicleId = _currentVehicleId;
		this.vehicleName = _currentVehicleName;
		this.unlockedVehicles = _unlockedVehicles;
	}
}

[System.Serializable]
public class Vehicle {
	public int id;
	public string name;
	public string desc;
	public int maxSpeed;
	public int costPoint;
	// TODO: ... customize color index

	public Vehicle () {
	}

	public Vehicle (string _name) {
		this.name = _name;
	}

	public Vehicle (int _id, string _name, string _desc, int _speed, int _costPoint) {
		this.id = _id;
		this.name = _name;
		this.desc = _desc;
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

	public static bool Load() {
		if(File.Exists(Application.persistentDataPath + "/player.tapkids")) {
			BinaryFormatter bf = new BinaryFormatter();
			FileStream file = File.Open(Application.persistentDataPath + "/player.tapkids", FileMode.Open);
			players = (List<Player>)bf.Deserialize(file);
			file.Close(); 
			return true;
		} else {
			return false;
		}
	}
	#endregion text file functions
}
