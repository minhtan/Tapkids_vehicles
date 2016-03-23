using UnityEngine;

public static class CarGameEventController {

	#region public delegates
	public delegate void TargetTrackingEventHandler (bool isFound, Transform parent, string letter);

	// Game State 
	public delegate void InitGameEventHandler (string letter);

	public delegate void StartGameEventHandler ();

	public delegate void PauseGameEventHandler ();

	public delegate void ResetGameEventHandler ();

	public delegate void GameOverEventHandler ();

	// Car 
	public delegate void CollectLetterEventHandler (string letter);

	public delegate void GatherLetterEventHandler ();

	// Word
	public delegate void ValidateWordEventHandler ();

	// GUI 
	public delegate void UpdateCollectedTextEventHandler (string letter);

	// Others
	public delegate void CreateCarEventHandler () ;

	#endregion public delegates

	#region Events
	public static event TargetTrackingEventHandler TargetTracking;

	// Game State
	public static event InitGameEventHandler InitGame;

	public static event StartGameEventHandler StartGame;

	public static event PauseGameEventHandler PauseGame;

	public static event ResetGameEventHandler ResetGame;

	public static event GameOverEventHandler GameOver;

	// Car
	public static event CollectLetterEventHandler CollectLetter;

	public static event GatherLetterEventHandler GatherLetter;

	// Word
	public static event ValidateWordEventHandler ValidateWord;

	// GUI
	public static event UpdateCollectedTextEventHandler UpdateCollectedText;


	// Others
	public static event CreateCarEventHandler CreateCar;
	#endregion Events

	#region Event Invoker Methods

	public static void OnTargetTracking (bool isFound, Transform parent, string letter) {
		var handler = TargetTracking;
		if (handler != null) {
			handler (isFound, parent, letter);
		}
	}

	// Game State
	public static void OnInitGame (string letter) {
		var handler = InitGame;
		if (handler != null) {
			handler (letter);
		}
	}

	public static void OnStartGame () {
		var handler = StartGame;
		if (handler != null) {
			handler ();
		}
	}

	public static void OnPauseGame () {
		var handler = PauseGame;
		if (handler != null) {
			handler ();
		}
	}

	public static void OnResetGame () {
		var handler = ResetGame;
		if (handler != null) {
			handler ();
		}
	}

	public static void OnGameOver () {
		var handler = GameOver;
		if (handler != null) {
			handler ();
		}
	}

	// Car
	public static void OnCollectLetter (string letter) {
		var handler = CollectLetter;
		if (handler != null) {
			handler (letter);
		}
	}

	public static void OnGatherLetter () {
		var handler = GatherLetter;
		if (handler != null) {
			handler ();
		}
	}

	// Word
	public static void OnValidateWord () {
		var handler = ValidateWord;
		if (handler != null) {
			handler ();
		}
	}

	// GUI
	public static void OnUpdateCollectedText (string letter) {
		var handler = UpdateCollectedText;
		if (handler != null) {
			handler (letter);
		}
	}
	// Others
	public static void OnCreateCar () {
		var handler = CreateCar;
		if (handler != null) {
			handler ();
		}
	}


	#endregion  Event Invoker Methods
}
