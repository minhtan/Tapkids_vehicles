using UnityEngine;
using System.Collections.Generic;

namespace WordList
{
	public class WriterLengthFilter : WordListWriter
	{

		private Dictionary<int, List<string>> groupLengthResult = new Dictionary<int, List<string>> ();

	#region implemented abstract members of WordListWriter

		public override void WriteWordList (List<string> resultList, string letterGroup, string fileName, string directoryPath)
		{
			foreach (string word in resultList)
				GroupWordByLength (word);
		
			JSONObject j = new JSONObject (JSONObject.Type.OBJECT);
			j.AddField ("letters", letterGroup);
			foreach (var key in groupLengthResult.Keys) {
				JSONObject arr = new JSONObject (JSONObject.Type.ARRAY);
				j.AddField ("" + key, arr);
			
				foreach (string word in groupLengthResult[key])
					arr.Add (word);
			
				string encodedJson = j.Print ();
			
				WriteTextFile (encodedJson, fileName, directoryPath);
			}
		}

	#endregion

		private void GroupWordByLength (string word)
		{
			if (groupLengthResult.ContainsKey (word.Length))
				groupLengthResult [word.Length].Add (word);
			else
				groupLengthResult.Add (word.Length, new List<string> ());
		}

	}
}
