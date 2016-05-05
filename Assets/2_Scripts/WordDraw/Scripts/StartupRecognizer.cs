using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using PDollarGestureRecognizer;
using WordDraw;

public class StartupRecognizer : LeanGestureRecognizer
{
	
	private Dictionary<Letters, List<Gesture>> _gestureDict;
	private List<Gesture> _optimizedGestureList;
	private string _currentLetter;

	protected override void OnEnable ()
	{
		base.OnEnable ();
	}

	protected override void Awake ()
	{
		Messenger.AddListener<bool, string> (EventManager.AR.LETTER_TRACKING.ToString (), OnLetterFound);
		base.Awake ();
	}

	protected override void Start ()
	{
		base.Start ();
	}

	private void OnLetterFound (bool found, string letter)
	{
		if (found)
			_currentLetter = letter;
	}

	/// <summary>
	/// Loads the gestures.
	/// </summary>
	protected override void LoadGestures ()
	{
		GestureIO.LoadPremadeGestureTemplates ("GestureTemplates", GestureList);
	
		_gestureDict = new Dictionary<Letters, List<Gesture>> ();
		_optimizedGestureList = new List<Gesture> ();

		Letters[] letters = WordDrawConfig.LetterEnum;
		for (int i = 0; i < letters.Length; i++) {
			_gestureDict.Add (letters [i], new List<Gesture> ());
		}
			
		for (int i = 0; i < GestureList.Count; i++) {
			_gestureDict [WordDrawConfig.GetLetterFromName (GestureList [i].Name)].Add (GestureList [i]);
		}
	}

	protected override List<Gesture> GetOptimizedGestureSet ()
	{
		return _gestureDict [WordDrawConfig.GetLetterFromName (_currentLetter)];
	}
}
