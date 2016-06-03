using UnityEngine;
using System.Collections;
using WordDraw;
using PDollarGestureRecognizer;
using UnityEngine.Events;
using UnityEngine.UI;

public class StartupWord : MonoBehaviour
{

	public LeanGestureRecognizer _recognizer;
	public GestureAutoDrawer _autoDrawer;
	public GestureLineDrawing _drawer;
	public GameObject _letterHolder;
	public UITutText _tutText;
	public GameObject _drawLetterBut;
	public GameObject _exitBut;
	public float _showTemplateDuration = 2f;

	private UILetterButton _currentLetterBut;

	CaptureAndSave snapShot;
	public GameObject pnlLetterUI;
	public GameObject pnlVehicleUI;

	void Start ()
	{
		snapShot = GameObject.FindObjectOfType<CaptureAndSave> ();
		if(ArController.Instance != null){
			ArController.Instance.ToggleAR (true);
			ArController.Instance.SetCenterMode (true);
			ArController.Instance.SetArMaxStimTargets (1);
		}
		Messenger.Broadcast <bool> (EventManager.GUI.TOGGLE_MENU_BTN.ToString (), true);
		Messenger.Broadcast <bool> (EventManager.GUI.TOGGLE_PLAYER_PNL.ToString (), false);
	}

	void OnDestroy(){
		if(ArController.Instance != null){
			ArController.Instance.ToggleAR (false);
		}
	}

	void OnEnable ()
	{
		GestureAutoDrawer.OnDrawGestureDone += OnDrawGestureDone;
		StartupRecognizer.OnGestureDetected += OnGestureDetected;
		StartupRecognizer.OnGestureReset += OnGestureReset;
		Messenger.AddListener <bool, string> (EventManager.AR.LETTER_IMAGE_TRACKING.ToString (), OnLetterFound);
		Messenger.AddListener <bool, string> (EventManager.AR.VEHICLE_IMAGE_TRACKING.ToString (), OnVehicleFound);
		_recognizer.RegisterInputHandler ();
	}

	void OnDisable ()
	{
		GestureAutoDrawer.OnDrawGestureDone -= OnDrawGestureDone;
		StartupRecognizer.OnGestureDetected -= OnGestureDetected;
		StartupRecognizer.OnGestureReset -= OnGestureReset;
		Messenger.RemoveListener <bool, string> (EventManager.AR.LETTER_IMAGE_TRACKING.ToString (), OnLetterFound);
		Messenger.RemoveListener <bool, string> (EventManager.AR.VEHICLE_IMAGE_TRACKING.ToString (), OnVehicleFound);
	}


	public void OnClick ()
	{
		ArController.Instance.ToggleAR (false);
		DrawTutorial ();
	}

	public void OnExitClick ()
	{
		ArController.Instance.ToggleAR (true);
		_drawer.ResetStroke ();
		_exitBut.SetActive (false);
	}

	private void OnVehicleFound (bool state, string vehicleName)
	{
		pnlVehicleUI.SetActive (state);
	}

	private void OnLetterFound (bool found, string letterName)
	{
		pnlLetterUI.SetActive (found);

		if (!found) {
			_drawLetterBut.SetActive (false);
			return;
		}

		_currentLetterBut = _letterHolder.transform.GetChild (0).GetComponent<UILetterButton> ();

		_currentLetterBut.Letter = WordDrawConfig.GetLetterFromName (letterName);

		GameObject letterGo = Resources.Load<GameObject> ("Letters/" + letterName.ToUpper ());

		_letterHolder.transform.GetChild (0).GetComponent<Image> ().sprite = letterGo.GetComponent<Image> ().sprite;
		_drawLetterBut.SetActive (true);
	}

	private void OnDrawGestureDone (Gesture gesture)
	{
		StartCoroutine (DelayResetStroke ());	
	}

	private void OnGestureDetected (Result result)
	{
		Debug.Log (result.GestureClass);
		if (WordDrawConfig.CompareLetterWithResult (_currentLetterBut, result)) {
			_tutText.SetTutText (UITutText.TutText.CORRECT);
		} else {
			_tutText.SetTutText (UITutText.TutText.TRY_AGAIN);
		}
	}

	private void OnSessionResult (bool isPass)
	{
		if (isPass) {
			_tutText.SetTutText (UITutText.TutText.GOOD_JOB);
			EndLetterSession ();
		} else {
			_tutText.SetTutText (UITutText.TutText.OPPS_LET_SEE_AGAIN);
			DrawTutorial ();
			SetActiveInputGesture (false);
		}
	}

	private void SetActiveInputGesture (bool active)
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
		string gestureName = _currentLetterBut.Letter.ToString () + "training";
		_autoDrawer.AutoDrawGesture (gestureName);
	}

	private IEnumerator DelayResetStroke ()
	{
		yield return new WaitForSeconds (_showTemplateDuration);

		SetActiveInputGesture (true);

		_autoDrawer.ResetStroke ();
		_tutText.SetTutText (UITutText.TutText.LET_WRITE);
		_exitBut.SetActive (true);
	}

	public void _CaptureAndSave ()
	{
		snapShot.CaptureAndSaveToAlbum ();
	}
}
