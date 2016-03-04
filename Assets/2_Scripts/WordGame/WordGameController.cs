using UnityEngine;
using System;
using Vuforia;
using System.Collections.Generic;
using System.Linq;

public class WordGameController : MonoBehaviour {
    void Awake()
    {
        fsm = gameObject.GetComponent<PlayMakerFSM>();

        DefaultTrackableEventHandlerFSM[] imgTargs = FindObjectsOfType<DefaultTrackableEventHandlerFSM>();
        for (int i = 0; i < imgTargs.Length; i++)
        {
            letterToImgTarget.Add(imgTargs[i].targetName, imgTargs[i]);
        }
		GetDataList ();
    }

	#region Vars
	//Core
	private Dictionary<string, DefaultTrackableEventHandlerFSM> letterToImgTarget = new Dictionary<string, DefaultTrackableEventHandlerFSM>();
	private Dictionary<string, Transform> letterToPosition = new Dictionary<string, Transform> ();
    private List<string> lst_answers;
    private PlayMakerFSM fsm;
    public int gameTimeInSeconds;
    public AudioClip correctSound;

	//Data
	private List<WordGameData> dataList;
	private WordGameData data;

	//Score
	private int currentScore;
	private int minWordLength;
	private int winScore;
	[Range(0.0f, 1.0f)]
	public float winScorePercentage = 0.5f;
	public int letterPoint = 5;
	public int scoreStep = 1;
	#endregion

	#region Data funcs
	void GetDataList(){
		dataList = DataUltility.ReadWordListByLevel ("");
	}

    void RandomData()
    {
        UnityEngine.Random.seed = Environment.TickCount;
        data = dataList[Mathf.RoundToInt(UnityEngine.Random.Range(0, dataList.Count - 1))];
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
	#endregion

	#region Game funcs
	void _InitGame(){
		currentScore = 0;
		fsm.FsmVariables.GetFsmInt("timer").Value = gameTimeInSeconds;
		letterToPosition.Clear();
		RandomData();
		List<string> playableLetters = DataUltility.GetPlayableLetters (data);
		lst_answers = DataUltility.GetAnswersList(data);

		foreach (string letter in playableLetters){
			if( letterToImgTarget.ContainsKey(letter) ){
				DefaultTrackableEventHandlerFSM imageTarget;
				if ( letterToImgTarget.TryGetValue (letter, out imageTarget) ) {
					imageTarget.Ready ();
				}
			}
		}

		FindMinWordLength ();
		GetWinScore ();
	}

    void _AddPlayableTarget(string letter) {
        DefaultTrackableEventHandlerFSM imageTarget;
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
        if ( CheckAnswer(wordFound) ){
            SoundController.Instance.PlaySound(correctSound);
			currentScore = currentScore + GetWordScore(wordFound);
			fsm.FsmVariables.GetFsmInt("currentScore").Value = currentScore;
            lst_answers.Remove(wordFound);
        }
		if(lst_answers.Count <= 0){
			fsm.Fsm.Event ("gameover");
		}
    }

    bool CheckAnswer(string word)
    {
        if ( word != null && word != "" && lst_answers.Contains(word) ){
            return true;
        }else{
            return false;
        }
    }
	#endregion

	#region Score funcs
	void FindMinWordLength(){
		minWordLength = 100;
		for(int i=0; i<lst_answers.Count; i++){
			if(lst_answers[i].Length < minWordLength){
				minWordLength = lst_answers [i].Length;
			}
		}
	}

	void GetWinScore(){
		winScore = 0;
		for(int i = 0; i < lst_answers.Count; i++){
			winScore = winScore + GetWordScore(lst_answers[i]);
		}
		winScore = (int)(winScore * winScorePercentage);
		fsm.FsmVariables.GetFsmInt("winScore").Value = winScore;
	}

	int GetWordScore(string word){
		return letterPoint * word.Length * ((word.Length - minWordLength) * scoreStep + 1);
	}


	#endregion
}
