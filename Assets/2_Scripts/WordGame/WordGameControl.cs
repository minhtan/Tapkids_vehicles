using UnityEngine;
using System;
using Vuforia;
using System.Collections.Generic;
using System.Linq;

public class WordGameControl : MonoBehaviour {
    void Awake()
    {
        fsm = gameObject.GetComponent<PlayMakerFSM>();

        DefaultTrackableEventHandlerFSM[] imgTargs = FindObjectsOfType<DefaultTrackableEventHandlerFSM>();
        for (int i = 0; i < imgTargs.Length; i++)
        {
            dic_targetImages.Add(imgTargs[i].targetName, imgTargs[i]);
        }
    }

    //*********************************************************Game vars*********************************************************
    private Dictionary<string, DefaultTrackableEventHandlerFSM> dic_targetImages = new Dictionary<string, DefaultTrackableEventHandlerFSM>();
	private Dictionary<string, Transform> dic_playableTargets = new Dictionary<string, Transform> ();
    private List<string> lst_answers;
    private PlayMakerFSM fsm;
    public int timeInSeconds;
    public AudioClip correctSound;
    //********************************************************End game vars******************************************************

    //*********************************************************Data vars*********************************************************
    public WordGameDataList dataList;
    private WordGameData data;
    //********************************************************End data vars******************************************************

    //*********************************************************Data funcs********************************************************
    void RandomData()
    {
        UnityEngine.Random.seed = Environment.TickCount;
        data = dataList.list[Mathf.RoundToInt(UnityEngine.Random.Range(0, dataList.list.Count - 1))];
    }

    List<string> GetPlayableLetters()
    {
        List<string> letters = new List<string>();
        foreach (string letter in data.letters)
        {
            letters.Add(letter);
        }
        return letters;
    }

    List<string> GetAnswersList()
    {
        List<string> answers = new List<string>();
        foreach (string answer in data.answers)
        {
            answers.Add(answer);
        }
        return answers;
    }
    //*******************************************************End data funcs*******************************************************

    //*********************************************************UI funcs***********************************************************
    public void _ResetGame()
    {
        fsm.Fsm.BroadcastEvent("G_reset");
    }

    public void _ReadyGame()
    {
        fsm.Fsm.Event("ready");
    }
    //*******************************************************End UI funcs*********************************************************

    //*********************************************************Game funcs*********************************************************
    void _GameOver() {
        dic_playableTargets.Clear();
    }

    void _InitGame(){
        RandomData();
        List<string> playableLetters = GetPlayableLetters ();
        lst_answers = GetAnswersList();

        fsm.FsmVariables.GetFsmInt("timer").Value = timeInSeconds;

        foreach (string letter in playableLetters){
			if( dic_targetImages.ContainsKey(letter) ){
				DefaultTrackableEventHandlerFSM imageTarget;
				if ( dic_targetImages.TryGetValue (letter, out imageTarget) ) {
					imageTarget.Ready ();
				}
			}
		}
	}

    void _AddPlayableTarget(string letter) {
        DefaultTrackableEventHandlerFSM imageTarget;
        if ( dic_targetImages.TryGetValue(letter, out imageTarget) ){
            dic_playableTargets.Add(letter, imageTarget.gameObject.transform);
        }
    }

    void _RemovePlayableTarget(string letter) {
        if ( dic_targetImages.ContainsKey(letter) ){
            dic_playableTargets.Remove(letter);
        }
    }

	void _CheckWord(){
        string wordFound = CheckWordOrder();
        HandleWordFound(wordFound);
    }

    string CheckWordOrder() {
        dic_playableTargets = dic_playableTargets.OrderBy(x => x.Value.position.x).ToDictionary(x => x.Key, x => x.Value);
        string word = "";
        for (int i = 0; i < dic_playableTargets.Count; i++){
            word = word + dic_playableTargets.Keys.ElementAt(i);
        }
        return word;
    }

    void HandleWordFound(string wordFound) {
        if ( CheckAnswer(wordFound) ){
            SoundSingleton.Instance.PlaySound(correctSound);
            lst_answers.Remove(wordFound);
        }else{
            
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
    //*********************************************************End game funcs*********************************************************
}
