using UnityEngine;
using System.Collections.Generic;

public class WordDataLoader
{
	private List<string> _loadedAnswer;
	
	public struct WordData
	{
		public string[] Letters { set ; get; }

		public string[] Answers{ set; get; }
	}

	private WordData wordData = new WordData();

	public WordDataLoader()
	{
		_loadedAnswer = new List<string>();
	}

	public void LoadWordData (string fileName)
	{
		TextAsset sourceWordList = Resources.Load<TextAsset>(fileName);
		JSONObject j = new JSONObject(sourceWordList.text);


	}

	private void AccessData (JSONObject obj)
	{
		switch (obj.type) {
		case JSONObject.Type.OBJECT:
			for (int i = 0; i < obj.list.Count; i++) {
				string key = (string)obj.keys [i];
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
			break;
		case JSONObject.Type.NUMBER:
			break;
		case JSONObject.Type.BOOL:
			break;
		case JSONObject.Type.NULL:
			break;
		}
	}
}
