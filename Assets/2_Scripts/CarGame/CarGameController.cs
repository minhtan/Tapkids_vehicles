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
	public float gameTime = 10f;
	public List<string> collectedLetters;
	public Text gatherLetter;
	#endregion public members

	#region private members
	// mono
	Transform mTransform;
	private SKStateMachine <CarGameController> _machine;


	// word game data member
	private WordGameData wordGameData;		// contains given letters and, answers
	[HideInInspector]
	public string letter;					// founded image target

	// car game data member
//	string assetBundleName = "car_asset";
//	string carName = "Police";

	#endregion private members

	#region GET DATA
	// get letter from image target
	// get word data by given letter

	// get current select car
	// 
	void GetData () {
//		wordGameData = DataUltility.ReadDataForCarGame (letter);
	}
	#endregion GET DATA

	#region public functions

	#endregion public functions

	#region private functions
	void OnTargetTracking (bool _isFound, Transform _parent, string _letter) {
		if (_isFound) {
			if(letter != _letter){
				// reset create other game base on new letter
			}
			letter = _letter;
			mTransform.SetParent(_parent);

			_machine.changeState <CGInitState> ();
		} 
//		else {
//			// if lost target 
//			// we pause game for sure
//			if (_machine.currentState == ) {
//				_machine.changeState <CGPauseState> ();
//			}
//		}
	}

	void OnCollectLetter (string letter) {
		collectedLetters.Add (letter);
	}

	void OnGatherLetter () {
		if (collectedLetters.Count > 0) {
			gatherLetter.text = string.Join ("", collectedLetters.ToArray ());
		}
			
	}


	void GetWordData () {
//		wordGameData = DataUltility.ReadDataForCarGame (letter);
	}

	void Init () {
//		GetWordData ();
//		StartCoroutine (CreateCar ());
//		FsmVariables.GlobalVariables.GetFsmString ("givenWord").Value = wordGameData.letters;
	}



//	IEnumerator CreateCar () {
//		yield return StartCoroutine (AssetController.Instance.InstantiateGameObjectAsync (assetBundleName, carName, (bundle) => {
//			GameObject carGO = Instantiate (bundle);
//			carGO.transform.SetParent (transform);
//		}));
//	}



	#endregion private functions

	#region Mono
	void OnEnable () {
		CarGameEventController.TargetTracking += OnTargetTracking;
		CarGameEventController.CollectLetter += OnCollectLetter;
		CarGameEventController.GatherLetter += OnGatherLetter;
	}
	void OnDisable () {
		CarGameEventController.TargetTracking -= OnTargetTracking;
		CarGameEventController.CollectLetter -= OnCollectLetter;
		CarGameEventController.GatherLetter -= OnGatherLetter;
	}

	void Awake () {
		mTransform = this.transform;

		// setup finite state machine
		_machine = new SKStateMachine <CarGameController> (this, new CGWaitForTargetState ());
		_machine.addState (new CGInitState ());
		_machine.addState (new CGStartState ());
		_machine.addState (new CGPlayState ());
		_machine.addState (new CGPauseState ());
		_machine.addState (new CGGameOverState ());

		collectedLetters = new List <string> ();
	}

	void Start () {
		
	}
	void Update () {
		_machine.update (Time.deltaTime);
	}

	#endregion Mono
}
