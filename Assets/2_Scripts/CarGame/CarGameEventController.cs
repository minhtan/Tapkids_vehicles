using UnityEngine;

public static class CarGameEventController {

	#region public delegates
	public delegate void TargetTrackingEventHandler (bool isFound, Transform parent, string letter);

	// Game State 
	public delegate void InitGameEventHandler (string letter);

	public delegate void StartGameEventHandler ();

	public delegate void PauseGameEventHandler ();

	public delegate void GameOverEventHandler ();

	// Car 
	public delegate void CollectLetterEventHandler (string letter);

	public delegate void GatherLetterEventHandler ();


	#endregion public delegates

	#region Events
	public static event TargetTrackingEventHandler TargetTracking;

	// Game State
	public static event InitGameEventHandler InitGame;

	public static event StartGameEventHandler StartGame;

	public static event PauseGameEventHandler PauseGame;

	public static event PauseGameEventHandler GameOver;

	// Car
	public static event CollectLetterEventHandler CollectLetter;

	public static event GatherLetterEventHandler GatherLetter;

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

	#endregion  Event Invoker Methods
}
