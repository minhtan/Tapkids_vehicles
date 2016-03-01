using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DataUltility {	


	public static string ReadWordList(string fileName)
	{
		return Resources.Load<TextAsset>(fileName).text;
	}

	public static List<WordGameData> ReadWordListByLevel(string level){
		List<WordGameData> list = new List<WordGameData> ();

		TextAsset[] textList = Resources.LoadAll<TextAsset> ("WordList/" + level);
		for(int i=0; i < textList.Length; i++){
			list.Add(JsonUtility.FromJson<WordGameData> (textList [0].text));
		}

		return list;
	}
}
