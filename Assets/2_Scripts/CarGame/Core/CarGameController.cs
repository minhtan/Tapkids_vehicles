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
	public string givenLetters;					// founded image target

	private SKStateMachine <CarGameController> _machine;
	[HideInInspector]
	public Transform mTransform;
	#endregion public members

	#region private members

	private bool countDownFinished = false;
	#endregion private members

	#region Mono
	void Awake () {
		mTransform = this.transform;
	}

	void OnEnable () {
		Messenger.AddListener <bool, string> (EventManager.AR.VEHICLE_IMAGE_TRACKING.ToString(), HandleARCardTracking);

		Messenger.AddListener <bool, Transform> (EventManager.AR.MAP_IMAGE_TRACKING.ToString(), HandleARMapTracking);

		Messenger.AddListener <string> (EventManager.Vehicle.COLLECT_LETTER.ToString (), HandleCollectLetter);

		Messenger.AddListener (EventManager.GUI.DROPBUTTON.ToString (), HandleDropLetter);

		Messenger.AddListener (EventManager.GUI.FINISH_COUNTDOWN.ToString (), HandleCountDown);
	}

	void Start () {
		if (ArController.Instance != null) {
			ArController.Instance.ToggleAR (true);
			ArController.Instance.SetCenterMode (false);
			ArController.Instance.SetArMaxStimTargets (1);
		}

//		if (GUIController.Instance != null) {
		Messenger.Broadcast <bool> (EventManager.GUI.TOGGLE_MENU_BTN.ToString (), true);
//		}

		// setup finite state machine
		_machine = new SKStateMachine <CarGameController> (this, new CGARCardState ());
		_machine.addState (new CGInitState ());
		_machine.addState (new CGARMapState ());
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
		Messenger.RemoveListener <bool, string> (EventManager.AR.VEHICLE_IMAGE_TRACKING.ToString(), HandleARCardTracking);

		Messenger.RemoveListener <bool, Transform> (EventManager.AR.MAP_IMAGE_TRACKING.ToString(), HandleARMapTracking);

		Messenger.RemoveListener <string> (EventManager.Vehicle.COLLECT_LETTER.ToString (), HandleCollectLetter);

		Messenger.RemoveListener (EventManager.GUI.DROPBUTTON.ToString (), HandleDropLetter);

		Messenger.RemoveListener (EventManager.GUI.FINISH_COUNTDOWN.ToString (), HandleCountDown);
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
	private void HandleARCardTracking (bool _isFound, string _givenLetters) {
		if (_isFound && _machine.currentState.GetType () == typeof (CGARCardState)) {	// FOUND LETTER
			givenLetters = _givenLetters;
			_machine.changeState <CGInitState> ();
			AudioManager.Instance.PlayAudio (AudioKey.UNIQUE_KEY.SCAN_LETTER);
			Messenger.Broadcast <string> (EventManager.GUI.SHOWSUGGESTION.ToString (), _givenLetters);
		} else {		// LOST LETTER
			// DO NOTHING
		}
	}

	private void HandleARMapTracking (bool _isFound, Transform _parent) {
		if (_machine == null) return;


		if (_isFound) {	// FOUND MAP
			if (givenLetters.Length > 0) {	// check given letters
				if (_machine.currentState.GetType () == typeof (CGARMapState) && countDownFinished) 
				{	
					AudioManager.Instance.PlayAudio (AudioKey.UNIQUE_KEY.SCAN_MAP);

					Renderer[] rendererComponents = GetComponentsInChildren<Renderer>(true);

					// Enable rendering:
					foreach (Renderer component in rendererComponents)
					{
						component.enabled = true;
					}
					_machine.changeState <CGStartState> ();

					mTransform.SetParent (_parent);
					Messenger.Broadcast <bool> (EventManager.GUI.TOGGLE_GAME_PNL.ToString (), true);
				}
			} else {
				GUIController.Instance.OpenDialog ("Please scan a vehicle!!!")
					.AddButton ("Ok", UIDialogButton.Anchor.BOTTOM_CENTER, 0, 32);
			}
		} else {		// LOST MAP
			Renderer[] rendererComponents = GetComponentsInChildren<Renderer>(true);

			// Disable rendering:
			foreach (Renderer component in rendererComponents)
			{
				component.enabled = false;
			}

			if(_machine.currentState.GetType () == typeof (CGStartState))
				_machine.changeState <CGARMapState> ();
			
			Messenger.Broadcast <bool> (EventManager.GUI.TOGGLE_GAME_PNL.ToString (), false);
		}
	}

	// handle car events
	private void HandleCollectLetter (string _letter) {
		collectedLetters = string.Concat (collectedLetters, _letter);
		AudioManager.Instance.PlayAudio (AudioKey.UNIQUE_KEY.CARGAME_COLLECT_LETTER);

		// handle trigger gameover
		if (givenLetters.Equals (collectedLetters)) {
			AudioManager.Instance.PlayAudio (AudioKey.UNIQUE_KEY.CORRECT_WORD);
			PlayerDataController.Instance.UpdatePlayerCredit(givenLetters.Length * 5);
			_machine.changeState <CGGameOverState> ();
		} else if (givenLetters.Length == collectedLetters.Length && !givenLetters.Equals (collectedLetters)) {
//			Messenger.Broadcast <string, float> (EventManager.GUI.NOTIFY.ToString (), GameConstant.WrongMessage, 1f);
			AudioManager.Instance.PlayAudio (AudioKey.UNIQUE_KEY.INCORRECT_WORD);
		}
	}

	private void HandleDropLetter () {
		if (collectedLetters.Length > 0) {
			string letter = collectedLetters[collectedLetters.Length - 1].ToString ();
			Messenger.Broadcast <string> (EventManager.GUI.REMOVE_LETTER.ToString (), letter);
			collectedLetters = collectedLetters.Substring (0, collectedLetters.Length - 1);
			AudioManager.Instance.PlayAudio (AudioKey.UNIQUE_KEY.CARGAME_DROP_LETTER);
		}
	}

	private void HandleCountDown () {
		countDownFinished = true;
	}
	#endregion private functions
}
