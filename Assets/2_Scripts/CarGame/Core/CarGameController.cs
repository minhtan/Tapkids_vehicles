using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using  HutongGames.PlayMaker;
using Prime31.StateKit;
using UnityEngine.UI;

/// <summary>
/// Car game controller. handles int game data, and game states
/// Game data contains user data, car data, and word given data
/// </summary>
public class CarGameController : MonoBehaviour {

	#region public members
//	[HideInInspector]
//	public float gameTime = 10f;	// there is no more game time

	public Text gatherLetterText;
	public Text successText;

	[HideInInspector]
	public	float countDownTimer = 3f;
	#endregion public members

	#region private members
	[HideInInspector]
//	public List <string> collectedLetters;
	public string collectedLetters;

	// mono
	[HideInInspector]
	public Transform mTransform;

	// SK STATE
	private SKStateMachine <CarGameController> _machine;

	// word game data member
	[HideInInspector]
	public WordGameData wordGameData;		// contains given letters and, answers
	[HideInInspector]
	public string letters;					// founded image target
	[HideInInspector]
//	public List<string> answers;
	public string answer;
	[HideInInspector]
	public List<string> vehicles = new List<string> (new string[] {"airplane", "bus", "car", "delivery truck", "electric bike", "fire truck", "garbage truck", "helicopter", 
		"ice-cream truck", "jet ski", "kayak", "limousine", "motorcycle", "navy submarine", "outrigger canoe", "police car", "quadbike", "rickshaw", 
		"space shuttle", "train", "ultralight craft", "van", "windjammer", "excavator", "yacht", "zeppelin"});
	
	#endregion private members

	#region public functions
	IEnumerator CountDownCo () {
		countDownTimer = 3f;
		for (int i = 0; i < 3; i++) {
			Messenger.Broadcast <string, float> (EventManager.GUI.NOTIFY.ToString (), countDownTimer.ToString(), 1f);
			countDownTimer--;
			yield return new WaitForSeconds (1f);
		}
		Messenger.Broadcast <string, float> (EventManager.GUI.NOTIFY.ToString (), "Go", 1f);
		yield return null;
	}
	#endregion public functions

	#region private functions

	// NOTE: Car Game 1 flow 
	// scan letter and then scan a big map for better ar experience
	// 1. what if they scan map before scan letter ? warning player that they have not scanned letter
	// 2. notify player what letter they scanned 
	// 3. what if player scan new letter? 

	void HandleImageTracking (bool _isFound, string _letters) {
//		if (_machine.currentState.GetType () != typeof (CGLetterState)) return;

		if (_isFound) {	// FOUND LETTER
//			if (letters.Length <= 0) {
				letters = _letters;
				_machine.changeState <CGInitState> ();
				Messenger.Broadcast <string, float> (EventManager.GUI.NOTIFY.ToString (), letters, 1f);
//			}
		} else {		// LOST LETTER
			// DO NOTHING
		}
	}

	void HandleMapTracking (bool _isFound, Transform _parent) {
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

	void HandleCollectLetter (string _letter) {
		collectedLetters = string.Concat (collectedLetters, _letter);
		Messenger.Broadcast <string> (EventManager.GUI.UPDATECOLLECTEDLETTER.ToString (), collectedLetters);
	}

	void HandleDropLetter () {
		if (collectedLetters.Length > 0) {
			collectedLetters = collectedLetters.Substring (0, collectedLetters.Length - 1);
			Messenger.Broadcast <string> (EventManager.GUI.UPDATECOLLECTEDLETTER.ToString (), collectedLetters);
		}
	}

	void HandleGatherLetter () {
		if (collectedLetters.Length > 0) {
			// TODO: need a vehicle list data to compare
			if (vehicles.Contains (collectedLetters)) {
				_machine.changeState <CGGameOverState> ();

				Messenger.Broadcast <string, float> (EventManager.GUI.NOTIFY.ToString (), GameMessages.CorrectMessage, 1f);
			} else {
				Messenger.Broadcast <string, float> (EventManager.GUI.NOTIFY.ToString (), GameMessages.WrongMessage, 2f);
			}
		}
//		if (collectedLetters.Count > 0) {
//			string gather = string.Join ("", collectedLetters.ToArray ());
//			if(collectedLetters.ToArray ().Length > 0) {
//				if (answers.Contains (gather)) {
//					successText.text = "Correct";
//					_machine.changeState <CGPauseState> ();
//				} else {
//					successText.text = "Wrong";
//					collectedLetters.Clear ();
//					gatherLetterText.text = "Result";
//					// respawn letter
//					CarGameEventController.OnValidateWord();
//				}
//		}
	}

	void OnResetGame () {
		successText.text = "";
//		collectedLetters.Clear ();
		gatherLetterText.text = "Result";
	}
	#endregion private functions

	#region Mono
	void OnEnable () {
		
	}

	void OnDisable () {
		Messenger.RemoveListener <bool, string> (EventManager.AR.IMAGETRACKING.ToString(), HandleImageTracking);

		Messenger.RemoveListener <bool, Transform> (EventManager.AR.MAPTRACKING.ToString(), HandleMapTracking);

		Messenger.RemoveListener <string> (EventManager.Vehicle.COLLECTLETTER.ToString (), HandleCollectLetter);

		Messenger.RemoveListener (EventManager.Vehicle.DROPLETTER.ToString (), HandleDropLetter);

		Messenger.RemoveListener (EventManager.Vehicle.GATHERLETTER.ToString (), HandleGatherLetter);
	}

	void Awake () {
		Messenger.AddListener <bool, string> (EventManager.AR.IMAGETRACKING.ToString(), HandleImageTracking);

		Messenger.AddListener <bool, Transform> (EventManager.AR.MAPTRACKING.ToString(), HandleMapTracking);

		Messenger.AddListener <string> (EventManager.Vehicle.COLLECTLETTER.ToString (), HandleCollectLetter);

		Messenger.AddListener (EventManager.Vehicle.DROPLETTER.ToString (), HandleDropLetter);

		Messenger.AddListener (EventManager.Vehicle.GATHERLETTER.ToString (), HandleGatherLetter);

		mTransform = this.transform;

		// setup finite state machine
		_machine = new SKStateMachine <CarGameController> (this, new CGLetterState ());
		_machine.addState (new CGInitState ());
		_machine.addState (new CGMapState ());
		_machine.addState (new CGStartState ());
		_machine.addState (new CGPlayState ());
		_machine.addState (new CGPauseState ());
		_machine.addState (new CGGameOverState ());
		_machine.addState (new CGResetGameState ());
	}

	void Start () {
//		collectedLetters = new List <string> ();
//		StartCoroutine (CountDownCo ());
	}

	void Update () {
		_machine.update (Time.deltaTime);
	}

	void OnDestroy () {
		ArController.Instance.ToggleAR (false);
	}
	#endregion Mono
}
