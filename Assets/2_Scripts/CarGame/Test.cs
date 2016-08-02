using UnityEngine;
using System.Collections;
using System;
public class Test : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnGUI () {
		DateTime time = DateTime.Now;

		string day = time.Day.ToString().PadLeft(2, '0');;
		string month = time.Month.ToString().PadLeft(2, '0');;
		string year = time.Year.ToString();

		string hour = time.Hour.ToString().PadLeft(2, '0');
		string minute = time.Minute.ToString().PadLeft(2, '0');
		string second = time.Second.ToString().PadLeft(2, '0');

		GUILayout.Label(day + "/" + month + "/" + year + " - " + hour + ":" + minute + ":" + second);
	}
}
