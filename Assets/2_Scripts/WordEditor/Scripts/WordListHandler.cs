using UnityEngine;
using System.Collections.Generic;
using System.IO;

public class WordListHandler : MonoBehaviour
{
	[SerializeField]
	private string
		_directoryPath;
	[SerializeField]
	private string
		_outputFileName;
	[SerializeField]
	private string
		_inputWordFileName;
	[SerializeField]
	private string[]
		_letterGroups;

	public enum FilterMode
	{
		REPEATED,
		NO_REPEATED
	}

	[SerializeField]
	private FilterMode
		_filterMode = FilterMode.REPEATED;

	private List<string> _dict;
	private Dictionary<int, List<string>> groupLengthResult = new Dictionary<int, List<string>> ();

	public void ReadWordList ()
	{
		_dict = new List<string> ();

		TextAsset wordlist = Resources.Load<TextAsset> (_inputWordFileName);
		JSONObject jsonObject = new JSONObject (wordlist.text);

		AccessData (jsonObject);
	}

	public void CreateWordList ()
	{
		List<string> result = FindWordWithLetters (_letterGroups [0]);

		if(_filterMode == FilterMode.NO_REPEATED)
			FilterRepeateLetter(result);

		JSONObject j = new JSONObject (JSONObject.Type.OBJECT);
		j.AddField ("letters", _letterGroups [0]);
		JSONObject arr = new JSONObject (JSONObject.Type.ARRAY);
		j.AddField ("wordlist", arr);
		
		foreach (string word in result)
			arr.Add (word);
		
		string encodedJson = j.Print ();
		WriteTextFile (encodedJson, _outputFileName, _directoryPath);
	}

	public void CreateGroupWordList ()
	{
		List<string> result = FindWordWithLetters (_letterGroups [0]);

		if(_filterMode == FilterMode.NO_REPEATED)
			FilterRepeateLetter(result);

		foreach (string word in result)
			GroupWordByLength (word);
		 
		JSONObject j = new JSONObject (JSONObject.Type.OBJECT);
		j.AddField ("letters", _letterGroups [0]);
		foreach (var key in groupLengthResult.Keys) {
			JSONObject arr = new JSONObject (JSONObject.Type.ARRAY);
			j.AddField ("" + key, arr);
			
			foreach (string word in groupLengthResult[key])
				arr.Add (word);
			
			string encodedJson = j.Print ();
			
			WriteTextFile (encodedJson, _outputFileName, _directoryPath);
		}
	}
	

    #region PRIVATE METHOD
	private void FilterRepeateLetter (List<string> result)
	{
		foreach(string word in result)
		{
			if(IsDuplicatedLetter(word))
				result.Remove(word);
		}
	}

	private bool IsDuplicatedLetter(string word)
	{
		for(int i = 0; i < word.Length - 1; i++)
		{
			for(int k = i + 1; k < word.Length; k++)
			{
				if(word[i] == word[k])
					return true;
			}
		}

		return false;
	}

	private void GroupWordByLength (string word)
	{
		if (groupLengthResult.ContainsKey (word.Length))
			groupLengthResult [word.Length].Add (word);
		else
			groupLengthResult.Add (word.Length, new List<string> ());
	}

	private void WriteTextFile (string data, string fileName, string directoryPath)
	{
		var sr = File.CreateText (directoryPath + "/" + fileName);
		sr.Write (data);
		sr.Close ();
	}

	private List<string> FindWordWithLetters (string letters)
	{
		List<string> listWithWord = new List<string> ();

		foreach (string word in _dict) {
			if (IsContructedByLetters (word, letters)) {
				listWithWord.Add (word);
			}
		}

		return listWithWord;
	}

	private bool IsContructedByLetters (string word, string letters)
	{
		bool isSameAsGroup = false;

		for (int i = 0; i < word.Length; i++) {
			for (int k = 0; k < letters.Length; k++) {
				if (word [i] == letters [k]) {
					isSameAsGroup = true;
					break;
				} else
					isSameAsGroup = false;
			}

			if (!isSameAsGroup)
				return false;
		}

		return true;
	}

	private void AccessData (JSONObject obj)
	{
		switch (obj.type) {
		case JSONObject.Type.OBJECT:
			for (int i = 0; i < obj.list.Count; i++) {
				//string key = (string)obj.keys [i];
				JSONObject j = (JSONObject)obj.list [i];
				AccessData (j);
			}
			break;
		case JSONObject.Type.ARRAY:
			foreach (JSONObject j in obj.list) {
				AccessData (j);
			}
			break;
		case JSONObject.Type.STRING:
			if (obj.str.Length > 2)
				_dict.Add (obj.str);
			break;
		case JSONObject.Type.NUMBER:
			break;
		case JSONObject.Type.BOOL:
			break;
		case JSONObject.Type.NULL:
			break;
		}
	}
	#endregion
	
}

