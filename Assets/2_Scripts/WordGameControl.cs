using UnityEngine;
using System;
using System.Collections;
using Vuforia;
using System.Collections.Generic;
using HutongGames.PlayMaker;
using System.Linq;

public class WordGameControl : MonoBehaviour {

	private Dictionary<string, DefaultTrackableEventHandlerFSM> dic_targetImages = new Dictionary<string, DefaultTrackableEventHandlerFSM>();
	private Dictionary<string, Transform> playableTargetTransform = new Dictionary<string, Transform> ();

	void Awake(){
		DefaultTrackableEventHandlerFSM[] imgTargs = FindObjectsOfType<DefaultTrackableEventHandlerFSM> ();

		for(int i = 0; i < imgTargs.Length; i++){
			dic_targetImages.Add (imgTargs[i].targetName, imgTargs[i]);
		}
	}

	void InitGame(){
		List<string> playableLetters = GetPlayableLetters ();
		playableTargetTransform.Clear ();

		foreach(string letter in playableLetters){
			if(dic_targetImages.ContainsKey(letter)){
				DefaultTrackableEventHandlerFSM imageTarget;
				if ( dic_targetImages.TryGetValue (letter, out imageTarget) ) {
					//Set ready state for playable image target
					imageTarget.Ready ();
					//Add image target to playable dictionary
					playableTargetTransform.Add(letter, imageTarget.gameObject.transform);
				}
			}
		}
	}

	List<string> GetPlayableLetters(){
		List<string> letters = new List<string> ();
		letters.Add ("M");
		letters.Add ("D");
		letters.Add ("N");
		return letters;
	}

	string CheckingWordsOrder(){
		playableTargetTransform = playableTargetTransform.OrderBy(x=>x.Value.position.x).ToDictionary(x => x.Key, x => x.Value);
		string word = "";
		for(int i = 0; i < playableTargetTransform.Count; i++){
			word = word + playableTargetTransform.Keys.ElementAt (i);
		}
		Debug.Log (word);
		return word;
	}
}
