using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using PDollarGestureRecognizer;
using WordDraw;
using UnityEngine.Events;

public class WordTrainingGame : MonoBehaviour
{
	public TrainingDifficulty[] _difficulty;
	public LeanGestureRecognizer _recognizer;
	public GestureAutoDrawer _autoDrawer;
	public GestureLineDrawing _drawer;
	public UILetterController _uiLetterController;
	public UITutText _tutText;
	public GameObject[] _enableGO;
	public GameObject[] _disableGO;

	public float _showTemplateDuration = 2f;

	private UILetterButton _currentLetterBut;
	private DrawWordSession _currentSession;

	void OnEnable ()
	{
		UILetterButton.OnLetterClickEvent += OnLetterClickEvent;
		UILetterController.OnMoveToCenterDone += OnMoveCenterDone;
		GestureAutoDrawer.OnDrawGestureDone += OnDrawGestureDone;
		LeanGestureRecognizer.OnGestureDetected += OnGestureDetected;
		LeanGestureRecognizer.OnGestureReset += OnGestureReset;
		DrawWordSession.OnDrawSessionResult += OnSessionResult;
	}

	void OnDisable ()
	{
		UILetterButton.OnLetterClickEvent -= OnLetterClickEvent;
		UILetterController.OnMoveToCenterDone -= OnMoveCenterDone;
		GestureAutoDrawer.OnDrawGestureDone -= OnDrawGestureDone;
		LeanGestureRecognizer.OnGestureDetected -= OnGestureDetected;
		LeanGestureRecognizer.OnGestureReset -= OnGestureReset;
		DrawWordSession.OnDrawSessionResult -= OnSessionResult;
	}


	private void OnLetterClickEvent (UILetterButton clickedLetter)
	{
		for (int i = 0; i < _enableGO.Length; i++) {
			_enableGO [i].SetActive (true);
		}

		for (int i = 0; i < _disableGO.Length; i++) {
			_disableGO [i].SetActive (false);
		}

		_tutText.SetTutText (UITutText.TutText.DRAW_TEMPLATE);
		_currentLetterBut = clickedLetter;

		_currentSession = new DrawWordSession (_difficulty [0]);
	}

	private void OnMoveCenterDone (UILetterButton but)
	{
		DrawTutorial ();
	}

	private void OnDrawGestureDone (Gesture gesture)
	{
		StartCoroutine (DelayResetStroke ());
	}

	private void OnGestureDetected (Result result)
	{
		if (WordDrawConfig.CompareLetterWithResult (_currentLetterBut, result)) {
			_tutText.SetTutText (UITutText.TutText.CORRECT);
			_currentSession.CorrectCount++;
		} else {
			_tutText.SetTutText (UITutText.TutText.TRY_AGAIN);
			_currentSession.FailCount++;
		}
	}

	private void OnSessionResult (bool isPass)
	{
		if (isPass)
		{
			_tutText.SetTutText (UITutText.TutText.GOOD_JOB);
			EndLetterSession ();
		}
		else
		{
			_tutText.SetTutText (UITutText.TutText.OPPS_LET_SEE_AGAIN);
			_currentSession = _currentSession.NewInstance ();
			DrawTutorial ();
			SetActiveInputGesture (false);
		}
	}

	private void SetActiveInputGesture(bool active)
	{
		_drawer.enabled = active;
		_recognizer.enabled = active;
	}

	private void EndLetterSession ()
	{
		_uiLetterController.ResetLettersPosition ();
		_drawer.enabled = false;
		_recognizer.enabled = false;
		_uiLetterController.SetActiveScroll (true);
		_tutText.SetTutText (UITutText.TutText.WELCOME);
	}

	private void OnGestureReset ()
	{
		_drawer.ResetStroke ();
	}

	private void DrawTutorial ()
	{
		_autoDrawer.AutoDrawGesture ("Atraining");
	}

	private IEnumerator DelayResetStroke ()
	{
		yield return new WaitForSeconds (_showTemplateDuration);

		SetActiveInputGesture (true);

		_autoDrawer.ResetStroke ();
		_tutText.SetTutText (UITutText.TutText.LET_WRITE);
	}

	public class DrawWordSession
	{

		public static UnityAction<bool> OnDrawSessionResult;

		public int CorrectCount { 
			set { 
				_correctCount = value;
				_correctPercent = value / _difficulty.DrawRound * 100f; 
				CheckResult ();
			}

			get {
				return _correctCount;
			}
		}

		public int FailCount {
			set {
				_failCount = value;
				CheckResult ();
			}

			get { return _failCount; }
		}

		private bool IsPass {
			get {
				if (_correctPercent < _difficulty.PassPercent)
					return false;

				return true;
			}
		}

		private TrainingDifficulty _difficulty = null;
		private float _correctPercent = 0f;
		private int _correctCount = 0;
		private int _failCount = 0;

		public DrawWordSession (TrainingDifficulty difficulty)
		{
			_difficulty = difficulty;
		}

		public DrawWordSession NewInstance()
		{
			return new DrawWordSession (_difficulty);
		}

		private void CheckResult()
		{
			if (_correctCount + _failCount == _difficulty.DrawRound) {
				if (OnDrawSessionResult != null)
					OnDrawSessionResult (IsPass);
			}
		}
	}

	[System.Serializable]
	public class TrainingDifficulty : System.Object
	{
		[SerializeField]
		private Letters[] _trainingTargets;

		[SerializeField]
		private int _drawRound;

		[SerializeField]
		private float _passPercent;

		[SerializeField]
		private float _perStrokePeriod;

		public Letters[] TraningTargets{ set { _trainingTargets = value; } get { return _trainingTargets; } }

		public int DrawRound{ set { _drawRound = value; } get { return _drawRound; } }

		public float PassPercent{ get { return _passPercent; } }

		public float PerStrokePeriod{ set { _perStrokePeriod = value; } get { return _perStrokePeriod; } }
	}
}
