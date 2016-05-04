using System.Collections.Generic;

public static class GameConstant  {
	public const string assetBundleName = "car_asset";

	public static readonly List<string> vehicles = new List<string> (new string[] {"Ambulance", "Bus", "Car", "DeliveryTruck", "ElectricBike", "FireTruck", "GarbageTruck", "Helicopter", 
		"IcecreamTruck", "JetSki", "Kayak", "Limousine", "Motorcycle", "NavySubmarine", "OutriggerCanoe", "PoliceCar", "Quadbike", "Rickshaw", 
		"SpaceShuttle", "Train", "UltralightCraft", "Van", "Windjammer", "Excavator", "Yacht", "Zeppelin"});

	public static readonly List<string> fourWheels = new List<string> (new string[] {"Ambulance", "Bus", "Car", "DeliveryTruck", "FireTruck", "GarbageTruck", 
		"IcecreamTruck", "Limousine", "PoliceCar", "Van", "Excavator"});
	
	public const string WinMessage = "You Win";
	public const string LoseMessage = "You Lose";

	public const string CorrectMessage = "Correct Word";
	public const string WrongMessage = "Wrong Word";

	public const string LetterScanMessage = "Please scan a letter card first";
	public const string MapScanMessage = "Please scan the map";

	public const string PurchaseSuccessful = "Your Purchase was successful";
	public const string PurchaseUnsuccessful = "Your Purchase was NOT successful";

}
