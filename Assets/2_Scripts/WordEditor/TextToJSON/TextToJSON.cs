using UnityEngine;
using System.Collections;
using System.IO;
<<<<<<< HEAD
#if UNITY_EDITOR
using UnityEditor;
=======
>>>>>>> 6318629eb8111dfb492b75f740dc44a4f358c240

#if UNITY_EDITOR
using UnityEditor;

public class TextToJSON : MonoBehaviour
{
	public string _sourceFileName;
	public string _desFileName;

	
	void Awake ()
	{
		TextAsset text = Resources.Load<TextAsset> (_sourceFileName);
		string data = text.text;

		string[] seperator = { ","};
		string[] words = data.Split (seperator, 0);

		JSONObject json = new JSONObject (JSONObject.Type.OBJECT);
	
		JSONObject arr = new JSONObject (JSONObject.Type.ARRAY);
		json.AddField ("wordlist", arr);

		for(int i = 0; i < words.Length; i++)
		{
			arr.Add (words[i].TrimStart(' '));
		}

		string encodedJSON = json.Print (true);

		DataUltility.WriteTextFile (encodedJSON, _desFileName, Application.dataPath + "/Resources/");
	}
}
#endif
