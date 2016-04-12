using UnityEngine;
using System.Collections;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif
using System.IO;

public class DataUltility {	
	
	public static List<WordGameData> ReadDataForWordGame(){
		List<WordGameData> list = new List<WordGameData> ();

		TextAsset[] textList = Resources.LoadAll<TextAsset> ("WordGame/");
		for(int i=0; i < textList.Length; i++){
			list.Add(JsonUtility.FromJson<WordGameData> (textList [i].text));
		}

		return list;
	}

	public static WordGameData ReadDataForCarGame (string letter) {
		return JsonUtility.FromJson <WordGameData> (Resources.Load <TextAsset> ("CarGame/" + letter).text);
	}

	public static List<string> GetPlayableLetters(WordGameData data)
	{
		List<string> letters = new List<string>();
		char[] c_letters = data.letters.ToCharArray ();
		for(int i=0; i < c_letters.Length; i++){
			letters.Add (c_letters [i].ToString ());
		}
		return letters;
	}

	public static List<string> GetAnswersList(WordGameData data)
	{
		List<string> answers = new List<string>();
		foreach (string answer in data.wordlist)
		{
			answers.Add(answer);
		}
		return answers;
	}
	#if UNITY_EDITOR
	public static void WriteTextFile (string data, string fileName, string directoryPath)
	{
		string assetPath = directoryPath + "/" + fileName + ".txt";
		var sr = File.CreateText (assetPath);
		sr.Write (data);

		AssetDatabase.Refresh();

		int index = assetPath.IndexOf("Assets");
		string relativePath = assetPath.Substring(index, assetPath.Length - index);
		TextAsset txtGO = AssetDatabase.LoadAssetAtPath<TextAsset>(relativePath);
		EditorGUIUtility.PingObject(txtGO);

		sr.Close ();
	}
	#endif
}
