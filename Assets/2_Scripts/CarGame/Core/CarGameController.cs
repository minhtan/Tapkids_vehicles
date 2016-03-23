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
	public float gameTime = 10f;

	public Text gatherLetterText;
	public Text successText;
	public GameObject carPrefab;
	#endregion public members

	#region private members
	private string assetBundleName = "car_asset";
	private Vehicle currentVehicle;

	private List<string> collectedLetters;

	// mono
	Transform mTransform;
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

	#endregion public functions

	#region private functions
	void OnTargetTracking (bool _isFound, Transform _parent, string _letter) {
		if (_isFound) {
			if(letter != _letter){
				// reset create other game base on new letter
			}
			letter = _letter;
			mTransform.SetParent(_parent, true);
			_machine.changeState <CGInitState> ();
		}
		else {
			// TODO: WHAT IF TARGETLOST
		}
	}

	void OnCollectLetter (string letter) {
		collectedLetters.Add (letter);
		gatherLetterText.text =  string.Join ("", collectedLetters.ToArray ());
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

					CarGameEventController.OnValidateWord();
				}
			}
		}
	}

	void OnResetGame () {
		successText.text = "";
		collectedLetters.Clear ();
		gatherLetterText.text = "Result";

	}

	// create car from asset bundle
	void OnCreatCar () {
		GameObject carGO = Instantiate (carPrefab) as GameObject;
		carGO.transform.SetParent (transform);

		//TODO: pending for real asset bundle server
		// StartCoroutine (CreateCarCo ());
	}
	
	IEnumerator CreateCarCo () {
		yield return StartCoroutine (AssetController.Instance.InstantiateGameObjectAsync (assetBundleName, currentVehicle.name, (bundle) => {
			GameObject carGO = Instantiate (bundle) as GameObject;
			carGO.transform.SetParent (transform);
		}));
	}
	#endregion private functions

	#region Mono
	void OnEnable () {
		CarGameEventController.TargetTracking += OnTargetTracking;
		CarGameEventController.ResetGame += OnResetGame;
		CarGameEventController.CollectLetter += OnCollectLetter;
		CarGameEventController.GatherLetter += OnGatherLetter;

		CarGameEventController.CreateCar += OnCreatCar;
	}
	void OnDisable () {
		CarGameEventController.TargetTracking -= OnTargetTracking;
		CarGameEventController.ResetGame += OnResetGame;
		CarGameEventController.CollectLetter -= OnCollectLetter;
		CarGameEventController.GatherLetter -= OnGatherLetter;CarGameEventController.CreateCar -= OnCreatCar;
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
		_machine.addState (new CGResetGameState ());
	}

	void Start () {
		collectedLetters = new List <string> ();
		PlayerDataController.Instance.myPlayer2.unlockedVehicles.TryGetValue (PlayerDataController.Instance.myPlayer2.currentVehicleIndex, out currentVehicle);
	}

	void Update () {
		_machine.update (Time.deltaTime);
	}

	#endregion Mono
}
