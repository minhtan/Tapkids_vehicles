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

	public const string SFX_STATE = "SFX_STATE";

	public const string LostLetterStep1 = "Scan Any Vehicle Card";
	public const string LostLetterStep2 = "See Your Suggest And Press Next";
	public const string LostLetterStep3 = "Now Scan Big Map And Ride To Collect The Letters";

	public const string RoamingLetterStep1 = "Now Scan Big Map And Ride To Gather The Letters";
	public const string RoamingLetterStep2 = "Press The Button To Drop The Collected Letters";
	public const string RoamingLetterStep3 = "Press The Button To Check The Collected Letters";

//	public const string hasPlayedTutorial = "hasPlayedTutorial";
	public enum unlockStatus{
		VALID,
		INVALID
	}

	public enum cars {
		Ambulance,
		Bus,
		Car,
		Deliverytruck,
		Firetruck,
		Garbagetruck,
		Icecreamtruck,
		Limousine,
		Policecar,
		Van,
		Excavator
	}
}
