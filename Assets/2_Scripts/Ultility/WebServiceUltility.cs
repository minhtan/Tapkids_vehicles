using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using MiniJSON;
using System.Text;
using Lean;

public class WebServiceUltility : MonoBehaviour {

	#region Vars
	static Dictionary<string, System.Object> dictInput;
	static Dictionary<string, string> headers;
	static string UUID;
	public const string baseURL = "http://103.27.239.161/api/EnglishGame/";
	public const string CHECK_KEY_URL = "check_key";
	public const string OVERRIDE_KEY_URL = "override_key";

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

	public static IEnumerator CheckKey(string methodURL, string key, string userID, System.Action<WebData> returnData){
		string url = baseURL + methodURL;

		dictInput.Clear();
		dictInput.Add("keyID", key);
		dictInput.Add("userID", userID);
		dictInput.Add("uuID", UUID);
		dictInput.Add ("lang", LeanLocalization.GetInstance().CurrentLanguage);
		dictInput.Add ("islandID", GameConstant.ISLAND_ID);

		string input = Json.Serialize(dictInput);
		byte[] body = Encoding.UTF8.GetBytes(input);
		WWW www = new WWW(url, body, headers);
		yield return www;

		string result = www.text;
		Debug.Log (result);
		if (result != "" && result != null) {
			result = result.Substring (1, result.Length - 2).Replace("\\", "");
			Debug.Log ("CheckKey API result: " + result);

			Dictionary<string, System.Object> dictResult = Json.Deserialize (result) as Dictionary<string, System.Object>;

			string success = dictResult ["success"].ToString ().ToLower();
			string msg = dictResult ["message"].ToString ();
			int statusCode = System.Int32.Parse(dictResult ["status_code"].ToString());

			WebData data = new WebData ();
			if (success.Equals ("true")) {
				data.success = true;
			} else {
				data.success = false;
			}
			data.message = msg;
			data.status_code = statusCode;

			returnData (data);
		} else {
			returnData (null);
		}
	}
}
