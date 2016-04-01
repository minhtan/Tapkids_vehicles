using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using PDollarGestureRecognizer;
using WordDraw;

public class WordTrainingGame : MonoBehaviour {
	
	public GestureAutoDrawer _autoDrawer;
	public GestureLineDrawing _drawer;
	public UILetterController _uiLetterController;
	public UITutText _tutText;
	public GameObject[] _enableGO;
	public GameObject[] _disableGO;

	public float _showTemplateDuration = 2f;

	private UILetterButton _currentLetterBut;
	private DrawWordSession _currentSession;

	void OnEnable()
	{
		UILetterButton.OnLetterClickEvent += OnLetterClickEvent;
		UILetterController.OnMoveToCenterDone += OnMoveCenterDone;
		GestureAutoDrawer.OnDrawGestureDone += OnDrawGestureDone;
		LeanGestureRecognizer.OnGestureDetected += OnGestureDetected;
		LeanGestureRecognizer.OnGestureReset += OnGestureReset;
	}

	void OnDisable()
	{
		UILetterButton.OnLetterClickEvent -= OnLetterClickEvent;
		UILetterController.OnMoveToCenterDone -= OnMoveCenterDone;
		GestureAutoDrawer.OnDrawGestureDone -= OnDrawGestureDone;
		LeanGestureRecognizer.OnGestureDetected -= OnGestureDetected;
		LeanGestureRecognizer.OnGestureReset -= OnGestureReset;
	}

	void Awake()
	{
		_currentSession = new DrawWordSession ();
	}

	private void OnLetterClickEvent(UILetterButton clickedLetter)
	{
		for(int i = 0; i < _enableGO.Length; i++)
		{
			_enableGO [i].SetActive (true);
		}

		for(int i = 0; i < _disableGO.Length; i++)
		{
			_disableGO [i].SetActive (false);
		}

		_tutText.SetTutText (UITutText.TutText.DRAW_TEMPLATE);
		_currentLetterBut = clickedLetter;
	}	

	private void OnMoveCenterDone(UILetterButton but)
	{
		DrawTutorial ();
	}

	private void OnDrawGestureDone(Gesture gesture)
	{
		StartCoroutine (DelayResetStroke());
	}

	private void OnGestureDetected(Result result)
	{
		if(WordDrawConfig.CompareLetterWithResult(_currentLetterBut, result))
		{
			_tutText.SetTutText (UITutText.TutText.CORRECT);
			_currentSession.CorrectCount++;
		}
		else{
			_tutText.SetTutText (UITutText.TutText.TRY_AGAIN);
			_currentSession.FailCount++;
		}
	}

	private void EndLetterSession()
	{
		_uiLetterController.ResetLettersPosition ();
		_drawer.enabled = false;
		_tutText.SetTutText (UITutText.TutText.WELCOME);
	}

	private void OnGestureReset()
	{
		_drawer.ResetStroke ();
	}

	private void DrawTutorial()
	{
		_autoDrawer.AutoDrawGesture ("Atraining");
	}

	private IEnumerator DelayResetStroke()
	{
		yield return new WaitForSeconds (_showTemplateDuration);
		_drawer.enabled = true;
		_autoDrawer.ResetStroke ();
		_tutText.SetTutText (UITutText.TutText.LET_DRAW);
	}

	public class DrawWordSession {
		public int CorrectCount;
		public int FailCount;

		public void ResetSession()
		{
			CorrectCount = 0;
			FailCount = 0;
		}
	}
}
