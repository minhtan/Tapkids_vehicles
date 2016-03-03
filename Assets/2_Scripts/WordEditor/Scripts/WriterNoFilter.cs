using UnityEngine;
using System.Collections.Generic;

namespace WordList
{
	public class WriterNoFilter : WordListWriter
	{

	#region IWordListWriter implementation

		public override void WriteWordList (List<string> resultList, string[] letterGroups, string fileName, string directoryPath)
		{
			JSONObject j = new JSONObject (JSONObject.Type.OBJECT);
			j.AddField ("letters", letterGroups [0]);
			JSONObject arr = new JSONObject (JSONObject.Type.ARRAY);
			j.AddField ("wordlist", arr);
		
			foreach (string word in resultList)
				arr.Add (word);
		
			string encodedJson = j.Print ();
			WriteTextFile (encodedJson, fileName, directoryPath);
		}

	#endregion

	}

}
