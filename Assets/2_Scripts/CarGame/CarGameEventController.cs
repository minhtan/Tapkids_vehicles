using UnityEngine;

public static class CarGameEventController {

	#region public delegates
	public delegate void TargetTrackingEventHandler (bool isFound, Transform parent, string letter);

	// Game State 
	public delegate void InitGameEventHandler (string _letter);

	public delegate void StartGameEventHandler ();

	public delegate void PauseGameEventHandler (bool _isPaused);

	public delegate void ResetGameEventHandler ();

	public delegate void GameOverEventHandler ();

	// Car 
	public delegate void CollectLetterEventHandler (string _letter);

	public delegate void GatherLetterEventHandler ();

	// Word
	public delegate void ValidateWordEventHandler ();

	// GUI 
	public delegate void UpdateCollectedTextEventHandler (string _letter);

	public delegate void ToggleTutorialPanelEventHandler (bool _isToggled);

	public delegate void ToggleInGamePanelEventHandler (bool _isToggled);

	public delegate void ToggleGameOverPanelEventHandler (bool _isToggled);

	public delegate void SelectCarEventHandler (int index);
	// Others

	#endregion public delegates

	#region EventsisPaused
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

	public static event ToggleTutorialPanelEventHandler ToggleTutorialPanel;

	public static event ToggleInGamePanelEventHandler ToggleInGamePanel;

	public static event ToggleGameOverPanelEventHandler ToggleGameOverPanel;

	public static event SelectCarEventHandler SelectCar;

	// Others
	#endregion Events

	#region Event Invoker Methods

	public static void OnTargetTracking (bool _isFound, Transform _parent, string _letter) {
		var handler = TargetTracking;
		if (handler != null) {
			handler (_isFound, _parent, _letter);
		}
	}

	// Game State
	public static void OnInitGame (string _letter) {
		var handler = InitGame;
		if (handler != null) {
			handler (_letter);
		}
	}

	public static void OnStartGame () {
		var handler = StartGame;
		if (handler != null) {
			handler ();
		}
	}

	public static void OnPauseGame (bool _isPaused) {
		var handler = PauseGame;
		if (handler != null) {
			handler (_isPaused);
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
	public static void OnCollectLetter (string _letter) {
		var handler = CollectLetter;
		if (handler != null) {
			handler (_letter);
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
	public static void OnUpdateCollectedText (string _letter) {
		var handler = UpdateCollectedText;
		if (handler != null) {
			handler (_letter);
		}
	}

	public static void OnToggleTutorialPanel (bool _isToggled) {
		var handler = ToggleTutorialPanel;
		if (handler != null) {
			handler (_isToggled);
		}
	}

	public static void OnToggleInGamePanel (bool _isToggled) {
		var handler = ToggleInGamePanel;
		if (handler != null) {
			handler (_isToggled);
		}
	}

	public static void OnToggleGameOverPanel (bool _isToggled) {
		var handler = ToggleGameOverPanel;
		if (handler != null) {
			handler (_isToggled);
		}
	}

	public static void OnSelectCar (int _index) {
		var handler = SelectCar;
		if (handler != null) {
			handler (_index);
		}
	}
	#endregion Event Invoker Methods
}
