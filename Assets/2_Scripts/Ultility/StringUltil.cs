using UnityEngine;
using System.Collections;

public class StringUltil : MonoBehaviour {

	public static string TextWrap (string input, int _size){
		string[] words = input.Split(" "[0]);
		string result = "";
		string line = "";
		foreach(string s in words){
			string temp = line + " " + s;
			if(temp.Length > _size){
				result += line + "\n";
				line = s;
			}
			else {
				line = temp;
			}
		}
		result += line;
		return result.Substring(1,result.Length-1);
	}

}
