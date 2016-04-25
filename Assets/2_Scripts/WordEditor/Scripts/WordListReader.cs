using UnityEngine;
using System.Collections.Generic;

namespace WordList
{
	public abstract class WordListReader
	{
		private List<string> wordList = new List<string>();

		public List<string> ReadWordList(string fileName)
		{
			TextAsset wordTextFile = Resources.Load<TextAsset> (fileName);
			JSONObject jsonObject = new JSONObject (wordTextFile.text);
			
			AccessData (jsonObject);

			return wordList;
		}

		public abstract void StringData(string data, List<string> wordList);
		public abstract void NumberData(float data , List<string> wordList);
		public abstract void BoolData(bool data, List<string> wordList);
		public abstract void NullData(List<string> wordList);

		private void AccessData (JSONObject obj)
		{
			switch (obj.type) {
			case JSONObject.Type.OBJECT:
				foreach (JSONObject jsonObject in obj.list) {
					//string key = (string)obj.keys [i];
					JSONObject j = jsonObject;
					AccessData (j);
				}
				break;
			case JSONObject.Type.ARRAY:
				foreach (JSONObject j in obj.list) {
					AccessData (j);
				}
				break;
			case JSONObject.Type.STRING:
				StringData(obj.str, wordList);
				break;
			case JSONObject.Type.NUMBER:
				NumberData(obj.f, wordList);
				break;
			case JSONObject.Type.BOOL:
				BoolData(obj.b, wordList);
				break;
			case JSONObject.Type.NULL:
				NullData(wordList);
				break;
			}
		}
	}
}
