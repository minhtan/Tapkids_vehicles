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
	public int currentPoint;
	public int currentVehicleIndex;
	public Vehicle[] unlockedVehicles;


	public Player () {
	}

	public Player (int _id, string _name, int _currentPoint, int _currentVehicleIndex, Vehicle[] _unlockedVehicles) {
		this.id = _id;
		this.name = _name;
		this.currentPoint = _currentPoint;
		this.currentVehicleIndex = _currentVehicleIndex;
		this.unlockedVehicles = _unlockedVehicles;
	}
}

[System.Serializable]
public class Vehicle {
	public string name;
	public int maxSpeed;
	public int costPoint;
	// TODO: ... customize color index

	public Vehicle () {
	}

	public Vehicle (string _name, int _speed, int _costPoint) {
		this.name = _name;
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

	public static Vehicle GetVehicleById (int _id) {
		for (int i = 0; i < players.Count; i++) {
			if (players[i].id == _id) {
				return players[i].unlockedVehicles[players[i].currentVehicleIndex];
			}
		}
		return null;
	}

	public static List<Vehicle> GetUnlockedVehicles () {
		List<Vehicle> vehicles = new List<Vehicle> ();



		return vehicles;
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

	public static void UpdatePlayerById (int _id, Player _newPlayer) {
		for (int i = 0; i < players.Count; i++) {
			if (players[i].id == _id) {
				players[i] = _newPlayer;
			}
		}
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
