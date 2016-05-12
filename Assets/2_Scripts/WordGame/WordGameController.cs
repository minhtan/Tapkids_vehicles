using UnityEngine;
using System;
using Vuforia;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class WordGameController : MonoBehaviour {
    void Awake()
    {
        fsm = gameObject.GetComponent<PlayMakerFSM>();
    }

	void OnEnable(){
		Messenger.AddListener<bool>( EventManager.GameState.PAUSE.ToString(), ToggleGamePause );
		Messenger.AddListener( EventManager.GameState.RESET.ToString(), _ResetGame );
	}

	void OnDestroy(){
//		Messenger.Cleanup ();
		if(ArController.Instance != null){
			ArController.Instance.ToggleAR (false);
		}
	}
	#region Vars
	//Core
	private Dictionary<string, FSMTrackable> letterToImgTarget = new Dictionary<string, FSMTrackable>();
	private Dictionary<string, Transform> letterToPosition = new Dictionary<string, Transform> ();
	private List<string> playableLetters = new List<string>();
	private List<string> answers;
	private List<string> foundAnswers = new List<string>();
    private PlayMakerFSM fsm;
    public int gameTimeInSeconds;
	public int timeToShowHint;

	//Data
	private List<WordGameData> dataList;
	private WordGameData data;
	private int lastRandomIndex = -1;

	//Score
	private int minWordLength;
	private int currentScore;
	public int letterPoint = 5;
	public int scoreStep = 1;

	//GUI
	bool isTimerRed = false;
	float timeToShowCard = 0.5f;
	public GameObject pnl_answer;
	public GameObject pref_answerText;
	public Text txt_timer;
	public GameObject btn_Start;
	public GameObject pnl_TargetsTofind;
	public GameObject pref_LetterToFind;
	public GameObject pnl_LetterToHint;

	public Text txt_ResultWords;
	public Text txt_ResultScore;
	public Text txt_FoundWords;
	#endregion

	#region Data funcs
	void GetDataList(){
		dataList = DataUltility.ReadDataForWordGame ();
	}

    void RandomData()
    {
        UnityEngine.Random.seed = Environment.TickCount;
		int rd;
		do {
			rd = Mathf.RoundToInt (UnityEngine.Random.Range (0, dataList.Count));
		} while(rd == lastRandomIndex);
		lastRandomIndex = rd;
		data = dataList[rd];
    }
	#endregion

	#region UI funcs
	void _UpdateStartUI(){
		HidePnlHint ();
		ToggleTimerColor (true);
		_ToggleIngameMenu (false);
		ClearAnswersText ();
		btn_Start.SetActive (false);

		//Clear letters in start UI
		ClearTargetsToFind();

		//Fill in letters to find
		FillInLettersToFind();

		StartCoroutine (ShowLettersToFind ());
	}

	void ClearAnswersText(){
		foreach(Transform t in pnl_answer.transform){
			Destroy (t.gameObject);
		}
	}

	void ClearTargetsToFind(){
		for(int i = 0; i < pnl_TargetsTofind.transform.childCount; i++){
			Destroy (pnl_TargetsTofind.transform.GetChild (i).gameObject);
		}
	}

	void FillInLettersToFind(){
		playableLetters.Shuffle ();
		for(int i = 0; i < playableLetters.Count; i++){
			GameObject go = Instantiate (pref_LetterToFind);
			go.GetComponentInChildren<UnityEngine.UI.Image> ().sprite = DataUltility.GetLetterImage (playableLetters [i]);
			go.transform.SetParent (pnl_TargetsTofind.transform, false);
			LeanTween.rotateY (go, 90f, 0f);
			go.SetActive (false);
		}
	}

	IEnumerator ShowLettersToFind(){
		int id = 0;
		for (int i = 0; i < pnl_TargetsTofind.transform.childCount;) {
			if(!LeanTween.isTweening(id)){
				pnl_TargetsTofind.transform.GetChild(i).gameObject.SetActive (true);
				id = LeanTween.rotateAroundLocal(pnl_TargetsTofind.transform.GetChild(i).gameObject, Vector3.up, 270f, timeToShowCard).setEase(LeanTweenType.easeOutBack).setOnComplete(() => {
					i ++;
				}).id;
			}
			yield return null;
		}
		btn_Start.SetActive (true);
	}

	void _UpdateWordFoundUI(){
		AudioManager.Instance.PlayAudio(AudioKey.UNIQUE_KEY.WORDGAME_CORRECT);

		HidePnlHint ();

		GameObject go = Instantiate (pref_answerText);
		go.GetComponent<Text> ().text = foundAnswers.LastOrDefault ();
		go.transform.SetParent (pnl_answer.transform, false);
	}

	void _UpdateTimerValue(){
		int time = fsm.FsmVariables.GetFsmInt ("timer").Value;
		txt_timer.text = Mathf.FloorToInt(time / 60).ToString("D2") + ":" + (time % 60).ToString ("D2");
	}

	void _ToggleIngameMenu(bool state){
		Messenger.Broadcast <bool> (EventManager.GUI.TOGGLE_MENU_BTN.ToString (), state);
	}

	void _ShowHint(){
		if(foundAnswers.Count <= 0){
			string hint = answers.FirstOrDefault ();
			for(int i = 0; i < hint.Length; i++){
				GameObject go = Instantiate (pref_LetterToFind);
				go.GetComponentInChildren<UnityEngine.UI.Image> ().sprite = DataUltility.GetLetterImage (hint[i].ToString());
				go.transform.SetParent (pnl_LetterToHint.transform, false);
				LeanTween.alpha (go.GetComponent<RectTransform>(), 0f, 0f);
			}
		}
		ShowPnlHint ();
	}

	void HidePnlHint(){
		if( pnl_LetterToHint.transform.parent.gameObject.activeSelf ){
			pnl_LetterToHint.transform.parent.gameObject.SetActive (false);
			for (int i = 0; i < pnl_LetterToHint.transform.childCount; i++) {
				Destroy (pnl_LetterToHint.transform.GetChild(i).gameObject);
			}
		}
	}

	void ShowPnlHint(){
		pnl_LetterToHint.transform.parent.gameObject.SetActive (true);
		for (int i = 0; i < pnl_LetterToHint.transform.childCount; i++) {
			LeanTween.alpha (pnl_LetterToHint.transform.GetChild(i).gameObject.GetComponent<RectTransform> (), 1f, timeToShowCard);
		}
	}

	void _ShowWarningCountDown(){
		ToggleTimerColor ();
		AudioManager.Instance.PlayAudio(AudioKey.UNIQUE_KEY.WORDGAME_CORRECT);
	}

	void ToggleTimerColor(bool overrideRed = false){
		if (isTimerRed || overrideRed) {
			txt_timer.color = Color.white;
			isTimerRed = false;
		} else {
			txt_timer.color = Color.red;
			isTimerRed = true;
		}
	}

	void _WakeUp(){
		AudioManager.Instance.PlayAudio(AudioKey.UNIQUE_KEY.WORDGAME_CORRECT);
	}

	void ShowResult(){
		txt_ResultWords.text = foundAnswers.Count + "/" + (answers.Count + foundAnswers.Count);
		txt_ResultScore.text = currentScore.ToString();

		if (foundAnswers.Count > 0) {
			for (int i = 0; i < foundAnswers.Count; i++) {
				if (i == 0) {
					txt_FoundWords.text = foundAnswers [i];
				} else {
					txt_FoundWords.text = txt_FoundWords.text + ", " + foundAnswers [i];
				}
			}
		} else {
			txt_FoundWords.text = "";
		}

	}

	public void _TEST(){
		HandleWordFound (answers.FirstOrDefault());
	}
	#endregion

	#region Game funcs
	void ToggleGamePause(bool state){
		fsm.Fsm.GetFsmBool ("isPause").Value = state;
	}

	public void _ReadyGame()
	{
		fsm.Fsm.Event("ready");
	}

	public void _ResetGame(){
		fsm.Fsm.Event ("reset");
	}

	void _PreInit(){
		FSMTrackable[] imgTargs = FindObjectsOfType<FSMTrackable>();
		for (int i = 0; i < imgTargs.Length; i++)
		{
			letterToImgTarget.Add(imgTargs[i].targetName, imgTargs[i]);
		}
		GetDataList ();
		ArController.Instance.ToggleAR (true);
		ArController.Instance.SetCenterMode (true);

		timeToShowHint = gameTimeInSeconds - timeToShowHint;
		fsm.FsmVariables.GetFsmInt ("timeToShowHint").Value = timeToShowHint;
	}

	void _InitGame(){
		currentScore = 0;
		fsm.FsmVariables.GetFsmInt ("timer").Value = gameTimeInSeconds;
		letterToPosition.Clear ();
		foundAnswers.Clear ();
		playableLetters.Clear ();
		RandomData ();
		playableLetters = DataUltility.GetPlayableLetters (data);
		answers = DataUltility.GetAnswersList (data);

		ArController.Instance.SetArMaxStimTargets (playableLetters.Count);

		FindMinWordLength ();
	}

	void _ReadyAllTargets(){
		foreach (string letter in playableLetters){
			if( letterToImgTarget.ContainsKey(letter) ){
				FSMTrackable imageTarget;
				if ( letterToImgTarget.TryGetValue (letter, out imageTarget) ) {
					imageTarget.Ready ();
				}
			}
		}
	}

	void _GameOver(){
		if(PlayerDataController.Instance != null){
			PlayerDataController.Instance.UpdatePlayerCredit(currentScore);
		}

		ShowResult ();
	}

    void _AddPlayableTarget(string letter) {
        FSMTrackable imageTarget;
        if ( letterToImgTarget.TryGetValue(letter, out imageTarget) ){
            letterToPosition.Add(letter, imageTarget.gameObject.transform);
        }
    }

    void _RemovePlayableTarget(string letter) {
        if ( letterToImgTarget.ContainsKey(letter) ){
            letterToPosition.Remove(letter);
        }
    }

	void _CheckWord(){
        string wordFound = CheckWordOrder();
        HandleWordFound(wordFound);
    }

    string CheckWordOrder() {
        letterToPosition = letterToPosition.OrderBy(x => x.Value.position.x).ToDictionary(x => x.Key, x => x.Value);
        string word = "";
        for (int i = 0; i < letterToPosition.Count; i++){
            word = word + letterToPosition.Keys.ElementAt(i);
        }
        return word;
    }

    void HandleWordFound(string wordFound) {
		if ( answers.Count > 0 && CheckAnswer (wordFound) ){
			currentScore += GetWordScore (wordFound);
            answers.Remove (wordFound);
			foundAnswers.Add (wordFound);
			fsm.Fsm.Event ("wordFound");
        }
		if(answers.Count <= 0){
			fsm.Fsm.Event ("gameover");
		}
    }

    bool CheckAnswer(string word)
    {
        if ( word != null && word != "" && answers.Contains(word) ){
            return true;
        }else{
            return false;
        }
    }
	#endregion

	#region Score funcs
	void FindMinWordLength(){
		minWordLength = 100;//hehe
		for(int i=0; i<answers.Count; i++){
			if(answers[i].Length < minWordLength){
				minWordLength = answers [i].Length;
			}
		}
	}

	int GetWordScore(string word){
		return letterPoint * word.Length * ((word.Length - minWordLength) * scoreStep + 1);
	}
	#endregion
}
