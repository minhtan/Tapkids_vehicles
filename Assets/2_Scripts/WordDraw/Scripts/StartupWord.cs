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
	public GameObject _drawLetterBut;
	public GameObject _exitBut;
	public float _showTemplateDuration = 2f;

	private UILetterButton _currentLetterBut;

	void OnEnable ()
	{
		GestureAutoDrawer.OnDrawGestureDone += OnDrawGestureDone;
		LeanGestureRecognizer.OnGestureDetected += OnGestureDetected;
		LeanGestureRecognizer.OnGestureReset += OnGestureReset;
		Messenger.AddListener<bool, string> (EventManager.AR.LETTERTRACKING.ToString(), OnLetterFound);

	}

	void OnDisable ()
	{
		GestureAutoDrawer.OnDrawGestureDone -= OnDrawGestureDone;
		LeanGestureRecognizer.OnGestureDetected -= OnGestureDetected;
		LeanGestureRecognizer.OnGestureReset -= OnGestureReset;
		Messenger.Cleanup ();
	}


	public void OnClick()
	{
		ArController.Instance.ToggleAR (false);
		DrawTutorial ();
	}

	public void OnExitClick()
	{
		ArController.Instance.ToggleAR (true);
		_exitBut.SetActive (false);
	}

	private void OnLetterFound(bool found, string letterName)
	{
		if (!found) {
			_drawLetterBut.SetActive (false);
			return;
		}

		_currentLetterBut = _letterHolder.transform.GetChild (0).GetComponent<UILetterButton> ();

		_currentLetterBut.Letter = WordDrawConfig.GetLetterFromName (letterName);

		_drawLetterBut.SetActive (true);
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
		_exitBut.SetActive (true);
	}
}
