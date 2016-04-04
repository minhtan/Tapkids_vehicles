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

	void OnDestroy(){
		ArController.Instance.ToggleAR (false);
	}

	void Update(){
		UpdateScoreSliderValue ();
	}
	#region Vars
	//Core
	private Dictionary<string, FSMTrackable> letterToImgTarget = new Dictionary<string, FSMTrackable>();
	private Dictionary<string, Transform> letterToPosition = new Dictionary<string, Transform> ();
	private List<string> playableLetters;
	private List<string> answers;
	private List<string> foundAnswers = new List<string>();
    private PlayMakerFSM fsm;
    public int gameTimeInSeconds;

	//Data
	private List<WordGameData> dataList;
	private WordGameData data;

	//Score
	private int minWordLength;
	private int totalScore;
	private int winScore;
	private int currentScore;
	[Range(0.0f, 1.0f)]
	public float winScorePercentage = 0.7f;
	public int letterPoint = 5;
	public int scoreStep = 1;

	//Sound
	public AudioClip correctSound;

	//GUI
	public Text txt_letters;
	public Text txt_answers;
	public Slider sld_score;
	#endregion

	#region Data funcs
	void GetDataList(){
		dataList = DataUltility.ReadDataForWordGame ();
	}

    void RandomData()
    {
        UnityEngine.Random.seed = Environment.TickCount;
		data = dataList[Mathf.RoundToInt (UnityEngine.Random.Range (0, dataList.Count))];
    }
	#endregion

	#region UI funcs
    public void _ReadyGame()
    {
        fsm.Fsm.Event("ready");
    }

	public void _ResetGame(){
		fsm.Fsm.Event ("reset");
	}

	void _UpdateStartUI(){
		txt_answers.text = "";
		txt_letters.text = "";
		for(int i = 0; i < playableLetters.Count; i++){
			if (i > 0) {
				txt_letters.text = txt_letters.text + " " + playableLetters [i];
			} else {
				txt_letters.text = txt_letters.text + playableLetters [i];
			}
		}
	}

	void _UpdateWordFoundUI(){
		SoundController.Instance.PlaySound(correctSound);

		txt_answers.text = "";
		for(int i=0; i < foundAnswers.Count; i++){
			if (i > 0) {
				txt_answers.text = txt_answers.text + "\n" + foundAnswers [i];
			} else {
				txt_answers.text = txt_answers.text + foundAnswers [i];
			}
		}
	}

	void UpdateScoreSliderValue(){
		sld_score.value = GetCurrentScorePercentage ();
	}
	#endregion

	#region Game funcs
	void _PreInit(){
		FSMTrackable[] imgTargs = FindObjectsOfType<FSMTrackable>();
		for (int i = 0; i < imgTargs.Length; i++)
		{
			letterToImgTarget.Add(imgTargs[i].targetName, imgTargs[i]);
		}
		GetDataList ();
		ArController.Instance.ToggleAR (true);
		ArController.Instance.SetCenterMode (true);
		ArController.Instance.SetArMaxStimTargets (5);
	}

	void _InitGame(){
		currentScore = 0;
		fsm.FsmVariables.GetFsmInt ("timer").Value = gameTimeInSeconds;
		letterToPosition.Clear ();
		foundAnswers.Clear ();
		RandomData ();
		playableLetters = DataUltility.GetPlayableLetters (data);
		answers = DataUltility.GetAnswersList (data);
		ArController.Instance.SetArMaxStimTargets (playableLetters.Count);

		FindMinWordLength ();
		GetWinScore ();
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
		if (currentScore >= winScore) {
			fsm.Fsm.Event ("win");
		} else {
			fsm.Fsm.Event ("lose");
		}
	}

	void _Win(){
		//add score
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
		Debug.Log (word);
        return word;
    }

    void HandleWordFound(string wordFound) {
        if ( CheckAnswer (wordFound) ){
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

	void GetWinScore(){
		totalScore = 0;
		for(int i = 0; i < answers.Count; i++){
			totalScore = totalScore + GetWordScore(answers[i]);
		}
		winScore = (int)(totalScore * winScorePercentage);
	}

	int GetWordScore(string word){
		return letterPoint * word.Length * ((word.Length - minWordLength) * scoreStep + 1);
	}

	float GetCurrentScorePercentage(){
		return (float)currentScore / totalScore;
	}
	#endregion
}
