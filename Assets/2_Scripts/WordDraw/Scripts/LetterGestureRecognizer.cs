using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using WordDraw;
using PDollarGestureRecognizer;

/// <summary>
/// Letter gesture recognizer.
/// </summary>
public class LetterGestureRecognizer : LeanGestureRecognizer {

	public LetterSpawner _spawner;
	private Dictionary<Letters, List<Gesture>> _gestureDict;
	private List<Gesture> _optimizedGestureList;

	protected override void Awake ()
	{
		base.Awake ();
	}

	protected override void Start ()
	{
		base.Start ();
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
		for(int i = 0; i < letters.Length; i++)
		{
			_gestureDict.Add (letters[i], new List<Gesture>());
		}
			

		for(int i = 0; i < GestureList.Count; i++)
		{
			_gestureDict [WordDrawConfig.GetLetterFromName (GestureList [i].Name)].Add (GestureList[i]);
		}
	}

	protected override List<Gesture> GetOptimizedGestureSet ()
	{

		List<UILetter> uiLetterList = _spawner.CurrentInStackLetters;

		if (uiLetterList == null)
			return null;

		_optimizedGestureList.Clear ();
		
		for(int i = 0; i < uiLetterList.Count; i++)
		{
			_optimizedGestureList.AddRange(_gestureDict[uiLetterList[i].Letter]);
		}

		return _optimizedGestureList;
	}
}
