using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class CarGameController : MonoBehaviour {

	#region public members


	#endregion public members

	#region private members
	private string mLetter;
	private WordGameData wordData;
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
		GetCarData ();
		GenerateLetter ();
		GenerateObstacles ();
	}

	void GetWordData () {
		wordData = DataUltility.ReadDataForCarGame (mLetter);
	}

	List<string> GetPlayableLetters()
	{
		List<string> letters = new List<string>();
		if (wordData == null)
			return null;
		char[] chars = wordData.letters.ToCharArray ();
		for(int i=0; i < chars.Length; i++){
			letters.Add (chars [i].ToString ());
		}
		return letters;
	}

	List<string> GetAnswersList()
	{
		List<string> answers = new List<string>();
		if (wordData.wordlist.Length > 0) {
			foreach (string answer in wordData.wordlist) {
				answers.Add (answer);
			}
		}
		return answers;
	}

	void GetCarData () {

	}

	void GenerateLetter () {

	}

	void GenerateObstacles () {

	}

	#endregion private functions

}
