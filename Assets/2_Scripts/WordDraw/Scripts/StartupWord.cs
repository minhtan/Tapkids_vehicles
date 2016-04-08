using UnityEngine;
using System.Collections;
using WordDraw;
using PDollarGestureRecognizer;
using UnityEngine.Events;

public class StartupWord : MonoBehaviour {

	public LeanGestureRecognizer _recognizer;
	public GestureAutoDrawer _autoDrawer;
	public GestureLineDrawing _drawer;
	public GameObject _letterHolder;
	public UITutText _tutText;
	public float _showTemplateDuration = 2f;

	private UILetterButton _currentLetterBut;

	void OnEnable ()
	{
		GestureAutoDrawer.OnDrawGestureDone += OnDrawGestureDone;
		LeanGestureRecognizer.OnGestureDetected += OnGestureDetected;
		LeanGestureRecognizer.OnGestureReset += OnGestureReset;
	}

	void OnDisable ()
	{
		GestureAutoDrawer.OnDrawGestureDone -= OnDrawGestureDone;
		LeanGestureRecognizer.OnGestureDetected -= OnGestureDetected;
		LeanGestureRecognizer.OnGestureReset -= OnGestureReset;
	}


	public void OnClick()
	{
		DrawTutorial ();
		_currentLetterBut = _letterHolder.transform.GetChild (0).GetComponent<UILetterButton> ();
	}

	private void OnDrawGestureDone (Gesture gesture)
	{
		StartCoroutine (DelayResetStroke ());
	}

	private void OnGestureDetected (Result result)
	{
		if (WordDrawConfig.CompareLetterWithResult (_currentLetterBut, result)) {
			_tutText.SetTutText (UITutText.TutText.CORRECT);
		} else {
			_tutText.SetTutText (UITutText.TutText.TRY_AGAIN);
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
		_drawer.enabled = false;
		_recognizer.enabled = false;
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
}
