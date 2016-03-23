using System.Collections.Generic;

[System.Serializable]
public class Player {
	public int id;
	public string name;
	public int currentPoint;
	public int currentVehicleIndex;
	public Dictionary <int, Vehicle> unlockedVehicles;


	public Player () {
	}

	public Player (int id, string name, int currentPoint, int currentVehicleIndex, Dictionary <int, Vehicle> unlockedVehicles) {
		this.id = id;
		this.name = name;
		this.currentPoint = currentPoint;
		this.currentVehicleIndex = currentVehicleIndex;
		this.unlockedVehicles = unlockedVehicles;
	}
}

[System.Serializable]
public class Vehicle {
	public string name;
	public int costPoint;
	// TODO: ... customize color index

	public Vehicle () {
	}

	public Vehicle (string name, int costPoint) {
		this.name = name;
		this.costPoint = costPoint;
	}
}