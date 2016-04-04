using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using PDollarGestureRecognizer;
using System.Collections.Generic;

[RequireComponent(typeof(Text))]
public class UITutText : MonoBehaviour {

	public Tutorial[] _tutorials;

	private Text _tutText;
	private Tutorial _curTutText;
	private int _curIndex = -1;

	public enum TutText {WELCOME, DRAW_TEMPLATE, LET_DRAW, CORRECT, TRY_AGAIN};

	private Dictionary<TutText, Tutorial> _tutDict;
		
	// Use this for initialization
	void Awake () {
		_tutText = GetComponent<Text> ();
		_tutDict = new Dictionary<TutText, Tutorial> ();

		for(int i = 0; i < _tutorials.Length; i++)
		{
			_tutDict.Add (_tutorials[i]._tutText, _tutorials[i]);
		}
	}

	public void ChangeToNextTut()
	{
		_curIndex++;

		_tutText.text = _tutorials [_curIndex].text;
	}

	public void SetTutText(TutText tutText)
	{
		_tutText.text = _tutDict [tutText].text;
	}

	[System.Serializable]
	public class Tutorial : System.Object{
		public TutText _tutText;
		public string text;
	}
}
