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
	[HideInInspector]
//	public float gameTime = 10f;	// there is no more game time

	public Text gatherLetterText;
	public Text successText;

	[HideInInspector]
	public	float countDownTimer = 3f;
	#endregion public members

	#region private members
	private List<string> collectedLetters;

	// mono
	private Transform mTransform;

	// SK STATE
	private SKStateMachine <CarGameController> _machine;

	// word game data member
	[HideInInspector]
	public WordGameData wordGameData;		// contains given letters and, answers
	[HideInInspector]
	public string letter;					// founded image target
	[HideInInspector]
	public List<string> answers;

	#endregion private members

	#region public functions
//	public void CountDown () {
////		string message = "GO";
//		if (countDownTimer <= 0) {
//			CancelInvoke ();
//			return;
////			Messenger.Broadcast <string> (EventManager.GUI.NOTIFY.ToString (), message);
//		} else {
//			Debug.Log ("count");
//			Messenger.Broadcast <string> (EventManager.GUI.NOTIFY.ToString (), countDownTimer.ToString());
//		}
//		countDownTimer--;
//	}
	IEnumerator CountDownCo () {
		for (int i = 0; i < 3; i++) {
			Messenger.Broadcast <string, float> (EventManager.GUI.NOTIFY.ToString (), countDownTimer.ToString(), 1f);
			countDownTimer--;
			yield return new WaitForSeconds (1f);
		}
//		string message = "GO";
//		if (countDownTimer <= 0) {
//			Messenger.Broadcast <string> (EventManager.GUI.NOTIFY.ToString (), message);
//		} 
		yield return null;
	}
	#endregion public functions

	#region private functions

	// NOTE: Car Game 1 flow 
	// scan letter and then scan a big map for better ar experience
	// 1. what if they scan map before scan letter ? warning player that they have not scanned letter
	// 2. notify player what letter they scanned 
	// 3. what if player scan new letter? 

	void OnLetterTracking (bool _isFound, string _letter) {
		if (_isFound) {	// FOUND LETTER
			if(letter.Length <= 0)
				letter = _letter;
			
			if (letter.Equals (_letter)) {	// deja vu
				_machine.changeState <CGInitState> ();
				Messenger.Broadcast <string, float> (EventManager.GUI.NOTIFY.ToString (), letter, 1f);
			} else {
				// DO NOTHING
			}
		} else {		// LOST LETTER
			// DO NOTHING
		}
	}

	void OnMapTracking (bool _isFound, Transform _parent) {
		if (letter.Length > 0) {
			if (_isFound) {	// FOUND MAP
				// check letter if null show warning

					mTransform.SetParent (_parent, true);
					for (int i = 0; i < mTransform.childCount; i++) 
						mTransform.GetChild (i).gameObject.SetActive (true);

					_machine.changeState <CGMapState> ();
			} else {		// LOST MAP
	//			for (int i = 0; i < mTransform.childCount; i++) 
	//				mTransform.GetChild (i).gameObject.SetActive (false);
	//			_machine.changeState <CGMapState> ();
			}
		} else {
//			Messenger.Broadcast <string, float> (EventManager.GUI.NOTIFY.ToString(), GameMessages.LetterScanMessage, 1f);
		}
	}

	void OnCollectLetter (string letter) {
//		collectedLetters.Add (letter);
//		gatherLetterText.text =  string.Join ("", collectedLetters.ToArray ());
	}

	void OnGatherLetter () {
		if (collectedLetters.Count > 0) {
			string gather = string.Join ("", collectedLetters.ToArray ());
			if(collectedLetters.ToArray ().Length > 0) {
				if (answers.Contains (gather)) {
					successText.text = "Correct";
					_machine.changeState <CGPauseState> ();
				} else {
					successText.text = "Wrong";
					collectedLetters.Clear ();
					gatherLetterText.text = "Result";
					// respawn letter

//					CarGameEventController.OnValidateWord();
				}
			}
		}
	}

	void OnResetGame () {
		successText.text = "";
		collectedLetters.Clear ();
		gatherLetterText.text = "Result";
	}
	#endregion private functions

	#region Mono
	void OnEnable () {
		
	}

	void OnDisable () {
		Messenger.RemoveListener <bool, string> (EventManager.AR.LETTERTRACKING.ToString(), OnLetterTracking);

		Messenger.RemoveListener <bool, Transform> (EventManager.AR.MAPTRACKING.ToString(), OnMapTracking);

	}

	void Awake () {
		Messenger.AddListener <bool, string> (EventManager.AR.LETTERTRACKING.ToString(), OnLetterTracking);

		Messenger.AddListener <bool, Transform> (EventManager.AR.MAPTRACKING.ToString(), OnMapTracking);

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
		collectedLetters = new List <string> ();
		StartCoroutine (CountDownCo ());
	}

	void Update () {
		_machine.update (Time.deltaTime);
	}

	#endregion Mono
}
