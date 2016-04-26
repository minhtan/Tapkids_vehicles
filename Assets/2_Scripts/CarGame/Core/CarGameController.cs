﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Prime31.StateKit;
using UnityEngine.UI;

/// <summary>
/// Car game controller. handles int game data, and game states
/// Game data contains user data, car data, and word given data
/// </summary>
public class CarGameController : MonoBehaviour {

	#region public members
	[HideInInspector]
	public string collectedLetters;
	[HideInInspector]
	public string letters;					// founded image target
	[HideInInspector]
	public List<string> vehicles = new List<string> (new string[] {"airplane", "bus", "car", "delivery truck", "electric bike", "fire truck", "garbage truck", "helicopter", 
		"ice-cream truck", "jet ski", "kayak", "limousine", "motorcycle", "navy submarine", "outrigger canoe", "police car", "quadbike", "rickshaw", 
		"space shuttle", "train", "ultralight craft", "van", "windjammer", "excavator", "yacht", "zeppelin"});
	private SKStateMachine <CarGameController> _machine;
	[HideInInspector]
	public Transform mTransform;
	#endregion public members

	#region private members
	#endregion private members

	#region Mono
	void Awake () {
		mTransform = this.transform;
	}

	void OnEnable () {
		Messenger.AddListener <bool, string> (EventManager.AR.IMAGETRACKING.ToString(), HandleVehicleTracking);

		Messenger.AddListener <bool, Transform> (EventManager.AR.MAPTRACKING.ToString(), HandleMapTracking);

		Messenger.AddListener <string> (EventManager.Vehicle.COLLECTLETTER.ToString (), HandleCollectLetter);

		Messenger.AddListener (EventManager.GUI.DROPBUTTON.ToString (), HandleDropLetter);

		Messenger.AddListener (EventManager.Vehicle.GATHERLETTER.ToString (), HandleGatherLetter);
	}
	void Start () {
		if (ArController.Instance != null) {
			ArController.Instance.ToggleAR (true);
			ArController.Instance.SetCenterMode (false);
			ArController.Instance.SetArMaxStimTargets (1);
		}

		// setup finite state machine
		_machine = new SKStateMachine <CarGameController> (this, new CGLetterState ());
		_machine.addState (new CGInitState ());
		_machine.addState (new CGMapState ());
		_machine.addState (new CGStartState ());
		_machine.addState (new CGPlayState ());
		_machine.addState (new CGPauseState ());
		_machine.addState (new CGGameOverState ());
		_machine.addState (new CGResetState ());


	}

	void Update () {
		_machine.update (Time.deltaTime);
	}

	void OnDisable () {
		Messenger.RemoveListener <bool, string> (EventManager.AR.IMAGETRACKING.ToString(), HandleVehicleTracking);

		Messenger.RemoveListener <bool, Transform> (EventManager.AR.MAPTRACKING.ToString(), HandleMapTracking);

		Messenger.RemoveListener <string> (EventManager.Vehicle.COLLECTLETTER.ToString (), HandleCollectLetter);

		Messenger.RemoveListener (EventManager.GUI.DROPBUTTON.ToString (), HandleDropLetter);

		Messenger.RemoveListener (EventManager.Vehicle.GATHERLETTER.ToString (), HandleGatherLetter);
	}

	void OnDestroy () {
		if (ArController.Instance != null)
			ArController.Instance.ToggleAR (false);
	}
	#endregion Mono

	#region public functions
	#endregion public functions

	#region private functions
	// handle ar events
	private void HandleVehicleTracking (bool _isFound, string _letters) {
		if (_isFound) {	// FOUND LETTER
				letters = _letters;
				_machine.changeState <CGInitState> ();
				Messenger.Broadcast <string, float> (EventManager.GUI.NOTIFY.ToString (), letters, 1f);
		} else {		// LOST LETTER
			// DO NOTHING
		}
	}

	private void HandleMapTracking (bool _isFound, Transform _parent) {
		if (_machine == null) return;

		if (_isFound) {	// FOUND MAP
			if (letters.Length > 0) {	// check given letters
				if (_machine.currentState.GetType () == typeof (CGMapState)) 
				{	
					_machine.changeState <CGStartState> ();
					mTransform.SetParent (_parent);
				}
			} else {
				Messenger.Broadcast <string, float> (EventManager.GUI.NOTIFY.ToString(), GameMessages.LetterScanMessage, 1f);
			}
		} else {		// LOST MAP
			if(_machine.currentState.GetType () == typeof (CGStartState))
				_machine.changeState <CGMapState> ();
		}
	}

	// handle car events
	private void HandleCollectLetter (string _letter) {
		Messenger.Broadcast <string> (EventManager.GUI.ADDLETTER.ToString (), _letter);

		collectedLetters = string.Concat (collectedLetters, _letter);
	}

	private void HandleDropLetter () {
		if (collectedLetters.Length > 0) {
			string letter = collectedLetters[collectedLetters.Length - 1].ToString ();
			Messenger.Broadcast <string> (EventManager.GUI.REMOVELETTER.ToString (), letter);

			collectedLetters = collectedLetters.Substring (0, collectedLetters.Length - 1);
		}
	}

	private void HandleGatherLetter () {
		if (collectedLetters.Length > 0) {
			if (vehicles.Contains (collectedLetters)) {
				_machine.changeState <CGGameOverState> ();
				Messenger.Broadcast <int> (EventManager.GameState.GAMEOVER.ToString (), 0);
			} else {
				Messenger.Broadcast <string, float> (EventManager.GUI.NOTIFY.ToString (), GameMessages.WrongMessage, 1f);
			}
		}
	}

	#endregion private functions


}
