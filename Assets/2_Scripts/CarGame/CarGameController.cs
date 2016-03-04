using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using  HutongGames.PlayMaker;
public class CarGameController : MonoBehaviour {

	#region public members
	#endregion public members

	#region private members
	private string mLetter;
	private WordGameData wordData;
//	private List <string> answers;
	private string letters;

	private List <GameObject> letterPrefabs;
	#endregion private members

	#region Mono
	#endregion Mono

	#region public functions
	public void GetTargetLetter (string letter) {
		mLetter = letter;
	}
	#endregion public functions

	#region private functions

	void Init () {
		GetWordData ();
//		GetCarData ();
//		GenerateLetter ();
		GenerateObstacles ();
		FsmVariables.GlobalVariables.GetFsmString ("givenWord").Value = wordData.letters;
	}

	void GetWordData () {
//		if (mLetter == null)
//			return;
		wordData = DataUltility.ReadDataForCarGame (mLetter);
//		answers = DataUltility.GetAnswersList (wordData);
//		letters = DataUltility.GetPlayableLetters (wordData);

	}
//
//	void GetCarData () {
//
//	}
//
//	void GenerateLetter () {
//		// generate letter object base on letter list
//		if (letters.Count <= 0)
//			return;
//		for (int i = 0; i < letters.Count; i++) {
//			Debug.Log (letters [i]);
//		}
//	}

	void GenerateObstacles () {

	}

	#endregion private functions

}
