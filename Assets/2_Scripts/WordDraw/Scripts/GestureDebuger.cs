using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using PDollarGestureRecognizer;

public class GestureDebuger : MonoBehaviour {
	public Text _debugText;

	void OnEnable()
	{
		LeanGestureRecognizer.OnGestureLoaded += OnLoaded;
	}

	void OnDisable()
	{
		LeanGestureRecognizer.OnGestureLoaded -= OnLoaded;
	}

	void OnLoaded(List<Gesture> gestures)
	{
		string db = "";

		for(int i = 0; i < gestures.Count; i++)
		{
			db += gestures [i].Name;
		}

		_debugText.text = db;
	}
}
