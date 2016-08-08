using UnityEngine;
using System.Collections;
using WordDraw;
using PDollarGestureRecognizer;
using UnityEngine.Events;
using UnityEngine.UI;
using Lean;

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
	public float _scaleTemplateFactor = 3f;
	private UILetterButton _currentLetterBut;
	private UIPausePanel pauseMenu;

	CaptureAndSave snapShot;
	public GameObject pnlLetterUI;
	public GameObject pnlVehicleUI;
	public GameObject canvasLetter;

	string targetName;

	void Start ()
	{
		snapShot = GameObject.FindObjectOfType<CaptureAndSave> ();
		pauseMenu = GameObject.FindObjectOfType<UIPausePanel> ();
		if(ArController.Instance != null){
			ArController.Instance.ToggleAR (true);
			ArController.Instance.SetCenterMode (false);
			ArController.Instance.SetArMaxStimTargets (1);
			ArController.Instance.ToggleLight (true);
		}						
		Messenger.Broadcast <bool> (EventManager.GUI.TOGGLE_MENU_BTN.ToString (), true);
	}

	void OnDestroy(){
		if(ArController.Instance != null){
			ArController.Instance.ToggleAR (false);
			ArController.Instance.ToggleLight (false);
		}
	}

	void OnEnable ()
	{
		GestureAutoDrawer.OnDrawGestureDone += OnDrawGestureDone;
		StartupRecognizer.OnGestureDetected += OnGestureDetected;
		StartupRecognizer.OnGestureReset += OnGestureReset;
		Messenger.AddListener <bool, string> (EventManager.AR.LETTER_IMAGE_TRACKING.ToString (), OnLetterFound);
		Messenger.AddListener <bool, string> (EventManager.AR.VEHICLE_IMAGE_TRACKING.ToString (), OnVehicleFound);
		Messenger.AddListener( EventManager.GameState.RESET.ToString(), OnExitClick );

		_recognizer.RegisterInputHandler ();
	}

	void OnDisable ()
	{
		GestureAutoDrawer.OnDrawGestureDone -= OnDrawGestureDone;
		StartupRecognizer.OnGestureDetected -= OnGestureDetected;
		StartupRecognizer.OnGestureReset -= OnGestureReset;
		Messenger.RemoveListener <bool, string> (EventManager.AR.LETTER_IMAGE_TRACKING.ToString (), OnLetterFound);
		Messenger.RemoveListener <bool, string> (EventManager.AR.VEHICLE_IMAGE_TRACKING.ToString (), OnVehicleFound);
		Messenger.RemoveListener( EventManager.GameState.RESET.ToString(), OnExitClick );

	}


	public void OnClick ()
	{
		ArController.Instance.ToggleAR (false);

		if(pauseMenu != null){
			pauseMenu.ToggleResetButton (true);
		}

		pnlLetterUI.SetActive (false);
		_tutText.SetTutText (UITutText.TutText.WELCOME);

		DrawTutorial ();
	}

	public void OnExitClick ()
	{
		if(canvasLetter.activeSelf){
			ArController.Instance.ToggleAR (true);

			if(pauseMenu != null){
				pauseMenu.ToggleResetButton (false);
			}

			_drawer.ResetStroke ();
			canvasLetter.SetActive (false);
//		_exitBut.SetActive (false);
		}else{
			
		}
	}

	private void OnVehicleFound (bool state, string vehicleName)
	{
		pnlVehicleUI.SetActive (state);
		targetName = vehicleName;
	}

	private void OnLetterFound (bool found, string letterName)
	{
		pnlLetterUI.SetActive (found);
		targetName = letterName;

		if (!found) {
			_drawLetterBut.SetActive (false);
			return;
		}

		_currentLetterBut = _letterHolder.transform.GetChild (0).GetComponent<UILetterButton> ();

		_currentLetterBut.Letter = WordDrawConfig.GetLetterFromName (letterName);

		GameObject letterGo = Resources.Load<GameObject> ("Letters/" + letterName.ToUpper ());
		GameObject child = _letterHolder.transform.GetChild (0).gameObject;

		Image image = child.GetComponent<Image> ();
		image.sprite = letterGo.GetComponent<Image> ().sprite;

		image.SetNativeSize ();
		RectTransform rectTrans = child.GetComponent<RectTransform> ();
		Vector3 size = rectTrans.sizeDelta;
		size.x = size.x * _scaleTemplateFactor;
		size.y = size.y * _scaleTemplateFactor;
		rectTrans.sizeDelta = size;
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
		GameObject autoDrawStrokes = GameObject.Find ("GestureAutoStroke0");

		string gestureName = _currentLetterBut.Letter.ToString () + "training";
		_autoDrawer.AutoDrawGesture (gestureName);
	}

	private IEnumerator DelayResetStroke ()
	{
		yield return new WaitForSeconds (_showTemplateDuration);

		SetActiveInputGesture (true);

		_autoDrawer.ResetStroke ();
		_tutText.SetTutText (UITutText.TutText.LET_WRITE);
//		_exitBut.SetActive (true);
	}

	public void _CaptureAndSave ()
	{
		snapShot.CaptureAndSaveToAlbum ();
		StartCoroutine (ShowDialog ());
	}

	IEnumerator ShowDialog(){
		yield return null;
		GUIController.Instance.OpenDialog (LeanLocalization.GetTranslation("PhotoCaptured").Text).AddButton (
			LeanLocalization.GetTranslation("Ok").Text, 
			UIDialogButton.Anchor.CENTER, 0, -25,
			() => {}
		);
	}

	public void _PlayModelSound(){
		AudioClip clip = Resources.Load<AudioClip> ("Sounds/" + targetName);
		if (clip != null) {
			AudioManager.Instance.PlayTemp (clip);
		}
	}
}
