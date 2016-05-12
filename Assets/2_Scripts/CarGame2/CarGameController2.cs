using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using Prime31.StateKit;

public class CarGameController2 : MonoBehaviour {

	#region public members
	#endregion public members

	#region private members
	private SKStateMachine <CarGameController2> _machine;
	[HideInInspector]
	public Transform mTransform;

	[HideInInspector]
	public string collectedLetters;
	[HideInInspector]
	public string letters;
	[HideInInspector]
	public WordGameData wordGameData;

	#endregion private members

	#region MONO
	void Awake () {
		mTransform = this.transform;
	}

	void OnEnable () {
		Messenger.AddListener <bool, Transform> (EventManager.AR.MAP_TRACKING.ToString(), HandleMapTracking);

		Messenger.AddListener <string> (EventManager.Vehicle.COLLECT_LETTER.ToString (), HandleCollectLetter);

		Messenger.AddListener (EventManager.GUI.DROPBUTTON.ToString (), HandleDropLetter);

		Messenger.AddListener (EventManager.Vehicle.GATHER_LETTER.ToString (), HandleGatherLetter);
	}

	void Start () {
		RandomWord();

		if (ArController.Instance != null) {
			ArController.Instance.ToggleAR (true);
			ArController.Instance.SetCenterMode (false);
			ArController.Instance.SetArMaxStimTargets (1);
		}

		if (GUIController.Instance != null) {
			Messenger.Broadcast <bool> (EventManager.GUI.TOGGLE_MENU_BTN.ToString (), true);
		}

		_machine = new SKStateMachine <CarGameController2> (this, new CG2InitState ());
		_machine.addState (new CG2ARMapState ());
		_machine.addState (new CG2StartState ());
		_machine.addState (new CG2PlayState ());
		_machine.addState (new CG2PauseState ());
		_machine.addState (new CG2GameOverState ());
		_machine.addState (new CG2ResetState ());
	}

	public void RandomWord()
	{
		wordGameData = DataUltility.ReadDataForCarGame ();
		UnityEngine.Random.seed = Environment.TickCount;
		letters = wordGameData.wordlist [UnityEngine.Random.Range (0, wordGameData.wordlist.Length)];
	}

	void Update () {
		_machine.update (Time.deltaTime);

//		if (Input.GetKeyDown (KeyCode.Space)) {
//			RandomData ();
//		}
	}

	void OnDisable () {
		Messenger.RemoveListener <bool, Transform> (EventManager.AR.MAP_TRACKING.ToString(), HandleMapTracking);

		Messenger.RemoveListener <string> (EventManager.Vehicle.COLLECT_LETTER.ToString (), HandleCollectLetter);

		Messenger.RemoveListener (EventManager.GUI.DROPBUTTON.ToString (), HandleDropLetter);

		Messenger.RemoveListener (EventManager.Vehicle.GATHER_LETTER.ToString (), HandleGatherLetter);
	}

	void OnDestroy () {
		if (ArController.Instance != null)
			ArController.Instance.ToggleAR (false);
	}
	#endregion MONO

	#region private functions
	void HandleMapTracking (bool _isFound, Transform _parent) {
		if (_machine == null) return;

		if (_isFound) {	// FOUND MAP
			if (_machine.currentState.GetType () == typeof (CG2ARMapState)) {
				_machine.changeState <CG2StartState> ();
				mTransform.SetParent (_parent);
			} else {
				// DO NOTHING
			}
		} else {		// LOST MAP
			if(_machine.currentState.GetType () == typeof (CG2StartState)) {
				_machine.changeState <CG2ARMapState> ();
			} else {
				// DO NOTHING
			}
		}
	}

	// handle car events
	void HandleCollectLetter (string _letter) {

	}
	void HandleDropLetter () {

	}
	void HandleGatherLetter () {

	}
	#endregion private function
}
