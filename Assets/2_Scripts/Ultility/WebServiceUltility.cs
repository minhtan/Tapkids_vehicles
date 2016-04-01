using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using MiniJSON;
using System.Text;

public class WebServiceUltility : MonoBehaviour {

	#region Vars
	static Dictionary<string, System.Object> dictInput;
	static Dictionary<string, string> headers;
	static string UUID;
	static string baseURL = "http://103.27.239.161/api/EnglishGame/";

	public class WebData
	{
		public bool success { get; set; }
		public int status_code { get; set; }
		public string message { get; set; }
		public string extra { get; set; }
	}
	#endregion

	static WebServiceUltility()
	{
		dictInput = new Dictionary<string, System.Object>();
		headers = new Dictionary<string, string>();
		headers.Add("Content-Type", "application/json");
		UUID = SystemInfo.deviceUniqueIdentifier;
	}

	public static IEnumerator CheckKey(string key, string userID, System.Action<WebData> returnData, string methodURL = "check_key"){
		string url = baseURL + methodURL;

		dictInput.Clear();
		dictInput.Add("keyID", key);
		dictInput.Add("userID", userID);
		dictInput.Add("uuID", UUID);

		string input = Json.Serialize(dictInput);
		byte[] body = Encoding.UTF8.GetBytes(input);
		WWW www = new WWW(url, body, headers);
		yield return www;

		string result = www.text;
		if (result != "" && result != null) {
			result = result.Substring (1, result.Length - 2).Replace("\\", "");
			Debug.Log ("CheckKey API result: " + result);
			returnData (JsonUtility.FromJson<WebData> (result));
		} else {
			returnData (null);
		}
	}
}
