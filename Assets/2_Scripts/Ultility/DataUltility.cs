using UnityEngine;
using System.Collections;

public class DataUltility {	


	public static string ReadWordList(string fileName)
	{
		return Resources.Load<TextAsset>(fileName).text;
	}
}
