
[System.Serializable]
public class Player {
	public string name;
	public Vehicle[] unlockeds;
	public int currentVehicle;
}

[System.Serializable]

public class Vehicle {
	public int id;
	public string name;
}