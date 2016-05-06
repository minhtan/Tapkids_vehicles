using UnityEngine;
using System.Collections;
using System.IO;

public class FixReadingFile : MonoBehaviour
{

	public string path;
	public string appendedString;

	void Awake ()
	{
		TextAsset[] allTextFile = Resources.LoadAll<TextAsset> (path);

		for (int i = 0; i < allTextFile.Length; i++) {
			TextAsset currentFile = allTextFile [i];

			StreamWriter sw = File.AppendText (path);

			sw.Write (appendedString);
			sw.Close ();
		}
	}
}
