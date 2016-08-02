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
	public List<WordGameData> wordGameDatas;
	[HideInInspector]
	public WordGameData wordGameData;
	[HideInInspector]
	public List<string> playableLetters = new List<string>();
	[HideInInspector]
	public List<string> answers;

	private int lastRandomIndex = -1;
	#endregion private members

	#region MONO
	void Awake () {
		mTransform = this.transform;
	}

	void OnEnable () {
		Messenger.AddListener <bool, Transform> (EventManager.AR.MAP_IMAGE_TRACKING.ToString(), HandleMapTracking);

		Messenger.AddListener <string> (EventManager.Vehicle.COLLECT_LETTER.ToString (), HandleCollectLetter);

		Messenger.AddListener (EventManager.GUI.DROPBUTTON.ToString (), HandleDropLetter);

		Messenger.AddListener (EventManager.Vehicle.GATHER_LETTER.ToString (), HandleGatherLetter);
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

		_machine = new SKStateMachine <CarGameController2> (this, new CG2InitState ());
		_machine.addState (new CG2ARMapState ());
		_machine.addState (new CG2StartState ());
		_machine.addState (new CG2PlayState ());
		_machine.addState (new CG2PauseState ());
		_machine.addState (new CG2GameOverState ());
		_machine.addState (new CG2ResetState ());
	}

	public WordGameData RandomData()
	{
		if (wordGameDatas.Count < 0) return null; 

		UnityEngine.Random.seed = Environment.TickCount;
		int rd;
		do {
			rd = UnityEngine.Random.Range (0, wordGameDatas.Count);
		} while (rd == lastRandomIndex);
		lastRandomIndex = rd;
		return wordGameDatas[rd];
	}

	public char RandomLetter () {
		System.Random _random = new System.Random();
		// This method returns a random lowercase letter.
		// ... Between 'a' and 'z' inclusize.
		int num = _random.Next(0, 26); // Zero to 25
		char ch = (char) ('a' + num);
		return ch;
	}

	void Update () {
		_machine.update (Time.deltaTime);
	}

	void OnDisable () {
		Messenger.RemoveListener <bool, Transform> (EventManager.AR.MAP_IMAGE_TRACKING.ToString(), HandleMapTracking);

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
			
			Renderer[] rendererComponents = GetComponentsInChildren<Renderer>(true);

			// Enable rendering:
			foreach (Renderer component in rendererComponents)
			{
				component.enabled = true;
			}

			if (_machine.currentState.GetType () == typeof (CG2ARMapState)) {
				_machine.changeState <CG2StartState> ();
				mTransform.SetParent (_parent);
			} else {
				// DO NOTHING
			}
		} else {		// LOST MAP
			
			Renderer[] rendererComponents = GetComponentsInChildren<Renderer>(true);

			// Disable rendering:
			foreach (Renderer component in rendererComponents)
			{
				component.enabled = false;
			}

			if(_machine.currentState.GetType () == typeof (CG2StartState)) {
				_machine.changeState <CG2ARMapState> ();
			} else {
				// DO NOTHING
			}
		}
	}

	// handle car events
	void HandleCollectLetter (string _letter) {
		collectedLetters = string.Concat (collectedLetters, _letter);
		AudioManager.Instance.PlayAudio (AudioKey.UNIQUE_KEY.CARGAME_COLLECT_LETTER);
	}
	void HandleDropLetter () {
		if (collectedLetters.Length > 0) {
			string letter = collectedLetters[collectedLetters.Length - 1].ToString ();
			Messenger.Broadcast <string> (EventManager.GUI.REMOVE_LETTER.ToString (), letter);

			collectedLetters = collectedLetters.Substring (0, collectedLetters.Length - 1);
			AudioManager.Instance.PlayAudio (AudioKey.UNIQUE_KEY.CARGAME_DROP_LETTER);
		}
	}

	void HandleGatherLetter () {
		if (answers.Contains (collectedLetters)) {
			AudioManager.Instance.PlayAudio (AudioKey.UNIQUE_KEY.CORRECT_WORD);

			Messenger.Broadcast (EventManager.GUI.CORRECTWORD.ToString ());

			answers.Remove (collectedLetters);
			collectedLetters = string.Empty;

			// check if there is no word left, trigger gameover state
			if (answers.Count == 0) {
				_machine.changeState <CG2GameOverState> ();
			}
		} else {
			AudioManager.Instance.PlayAudio (AudioKey.UNIQUE_KEY.INCORRECT_WORD);
		}
	}
	#endregion private function
}
