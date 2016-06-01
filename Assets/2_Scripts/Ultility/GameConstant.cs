using System.Collections.Generic;

public static class GameConstant  {
	public const string assetBundleName = "car_asset";

	public static readonly List<string> vehicles = new List<string> (new string[] {"ambulance", "bus", "car", "deliverytruck", "electricbike", "firetruck", "garbagetruck", "helicopter", 
		"icecreamtruck", "jetski", "kayak", "limousine", "motorcycle", "navysubmarine", "outriggercanoe", "policecar", "quadbike", "rickshaw", 
		"spaceshuttle", "train", "ultralightcraft", "van", "windjammer", "excavator", "yacht", "zeppelin"});

	public static readonly List<string> fourWheels = new List<string> (new string[] {"ambulance", "bus", "car", "deliverytruck", "firetruck", "garbagetruck", 
		"icecreamtruck", "limousine", "policecar", "van", "excavator"});
	
	public const string WinMessage = "You Win";
	public const string LoseMessage = "You Lose";

	public const string CorrectMessage = "Correct Word";
	public const string WrongMessage = "Wrong Word";

	public const string LetterScanMessage = "Please scan a letter card first";
	public const string MapScanMessage = "Please scan the map";

	public const string PurchaseSuccessful = "Your Purchase was successful";
	public const string PurchaseUnsuccessful = "Your Purchase was NOT successful";

	public const int ISLAND_ID = 2; //1 for animal, 2 for vehicle
	public const string UNLOCKED = "unlock";
	public enum unlockStatus{
		VALID,
		INVALID
	}

}
