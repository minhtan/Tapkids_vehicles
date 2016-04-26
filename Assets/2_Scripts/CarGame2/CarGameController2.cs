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
		Messenger.AddListener <bool, Transform> (EventManager.AR.MAPTRACKING.ToString(), HandleMapTracking);

		Messenger.AddListener <string> (EventManager.Vehicle.COLLECTLETTER.ToString (), HandleCollectLetter);

		Messenger.AddListener (EventManager.GUI.DROPBUTTON.ToString (), HandleDropLetter);

		Messenger.AddListener (EventManager.Vehicle.GATHERLETTER.ToString (), HandleGatherLetter);

		mTransform = this.transform;

		_machine = new SKStateMachine <CarGameController2> (this, new CG2InitState ());
		_machine.addState (new CG2MapState ());
		_machine.addState (new CG2StartState ());
		_machine.addState (new CG2PlayState ());
		_machine.addState (new CG2PauseState ());
		_machine.addState (new CG2GameOverState ());
		_machine.addState (new CG2ResetState ());
	}

	void Start () {
		// TODO: get word from database
//		wordGameData = DataUltility.ReadDataForCarGame ();
//		RandomData();
	}

	public void RandomData()
	{
		UnityEngine.Random.seed = Environment.TickCount;
		letters = wordGameData.wordlist [UnityEngine.Random.Range (0, wordGameData.wordlist.Length)];
//		Debug.Log (letters);
	}

	void Update () {
		_machine.update (Time.deltaTime);

//		if (Input.GetKeyDown (KeyCode.Space)) {
//			RandomData ();
//		}
	}

	void OnDisable () {
		Messenger.RemoveListener <bool, Transform> (EventManager.AR.MAPTRACKING.ToString(), HandleMapTracking);

		Messenger.RemoveListener <string> (EventManager.Vehicle.COLLECTLETTER.ToString (), HandleCollectLetter);

		Messenger.RemoveListener (EventManager.GUI.DROPBUTTON.ToString (), HandleDropLetter);

		Messenger.RemoveListener (EventManager.Vehicle.GATHERLETTER.ToString (), HandleGatherLetter);
	}

	void OnDestroy () {
		if (ArController.Instance != null)
			ArController.Instance.ToggleAR (false);
	}
	#endregion MONO

	#region private functions

	void HandleMapTracking (bool _isFound, Transform _parent) {
		if (_isFound) {	// FOUND MAP
			if (_machine.currentState.GetType () == typeof (CG2MapState)) {
				_machine.changeState <CG2StartState> ();
				mTransform.SetParent (_parent);
			} else {
				// DO NOTHING
			}
		} else {		// LOST MAP
			if(_machine.currentState.GetType () == typeof (CG2StartState)) {
				_machine.changeState <CG2MapState> ();
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
