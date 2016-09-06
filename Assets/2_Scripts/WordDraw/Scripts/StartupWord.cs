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
//	public GestureAutoDrawer _autoDrawer;
	public GestureLineDrawing _drawer;
	public GameObject _letterHolder;
	public UITutText _tutText;

	public float _showTemplateDuration = 2f;
	public float _scaleTemplateFactor = 3f;
	private UILetterButton _currentLetterBut;
	private UIPausePanel pauseMenu;

	CaptureAndSave snapShot;
	public GameObject pnlLetterUI;
	public GameObject pnlVehicleUI;
	public GameObject canvasLetter;
	public GameObject letterTargets;

	string targetName;
	PlayMakerFSM fsm;
	GameObject sampleLetter;

	void Start ()
	{
		snapShot = GameObject.FindObjectOfType<CaptureAndSave> ();
		pauseMenu = GameObject.FindObjectOfType<UIPausePanel> ();
		fsm = GetComponent<PlayMakerFSM> ();
		if (ArController.Instance != null) {
			ArController.Instance.ToggleAR (true);
			ArController.Instance.SetCenterMode (false);
			ArController.Instance.SetArMaxStimTargets (1);
			ArController.Instance.ToggleLight (true);
		}						
		Messenger.Broadcast <bool> (EventManager.GUI.TOGGLE_MENU_BTN.ToString (), true);
	}

	void OnDestroy ()
	{
		if (ArController.Instance != null) {
			ArController.Instance.ToggleAR (false);
			ArController.Instance.ToggleLight (false);
		}
	}

	void OnEnable ()
	{
//		GestureAutoDrawer.OnDrawGestureDone += OnDrawGestureDone;
		StartupRecognizer.OnGestureDetected += OnGestureDetected;
		StartupRecognizer.OnGestureReset += OnGestureReset;
		Messenger.AddListener <bool, string> (EventManager.AR.LETTER_IMAGE_TRACKING.ToString (), OnLetterFound);
		Messenger.AddListener <bool, string> (EventManager.AR.VEHICLE_IMAGE_TRACKING.ToString (), OnVehicleFound);
		Messenger.AddListener (EventManager.GameState.RESET.ToString (), OnExitClick);
		Messenger.AddListener (EventManager.GUI.LETTER_AUTODRAW_DONE.ToString(), HandleLetterAutoDrawDone);

		_recognizer.RegisterInputHandler ();
	}

	void OnDisable ()
	{
//		GestureAutoDrawer.OnDrawGestureDone -= OnDrawGestureDone;
		StartupRecognizer.OnGestureDetected -= OnGestureDetected;
		StartupRecognizer.OnGestureReset -= OnGestureReset;
		Messenger.RemoveListener <bool, string> (EventManager.AR.LETTER_IMAGE_TRACKING.ToString (), OnLetterFound);
		Messenger.RemoveListener <bool, string> (EventManager.AR.VEHICLE_IMAGE_TRACKING.ToString (), OnVehicleFound);
		Messenger.RemoveListener (EventManager.GameState.RESET.ToString (), OnExitClick);
		Messenger.RemoveListener (EventManager.GUI.LETTER_AUTODRAW_DONE.ToString(), HandleLetterAutoDrawDone);
	}


	public void OnClick ()
	{
		ArController.Instance.ToggleAR (false);

		if (pauseMenu != null) {
			pauseMenu.ToggleResetButton (true);
		}

		pnlVehicleUI.SetActive (false);
		fsm.Fsm.BroadcastEvent ("G_reset");
		letterTargets.SetActive (false);
		_tutText.SetTutText (UITutText.TutText.WELCOME);
		DrawTutorial ();
	}

	IEnumerator CallTut(){
		yield return new WaitForSeconds(0.1f);
		pnlVehicleUI.SetActive (false);
		fsm.Fsm.BroadcastEvent ("G_reset");
		_tutText.SetTutText (UITutText.TutText.WELCOME);
		DrawTutorial ();
	}

	public void OnExitClick ()
	{
		if (canvasLetter.activeSelf) {
			ArController.Instance.ToggleAR (true);

			if (pauseMenu != null) {
				pauseMenu.ToggleResetButton (false);
			}

//			_drawer.ResetStroke ();
			DestroySampleLetter();
			canvasLetter.SetActive (false);
			letterTargets.SetActive (true);

		} else {
			
		}
		SetActiveInputGesture (false);
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

		if (found == false) {
			return;
		}

//		_currentLetterBut = _letterHolder.transform.GetChild (0).GetComponent<UILetterButton> ();
//
//		_currentLetterBut.Letter = WordDrawConfig.GetLetterFromName (letterName);
//
//		GameObject letterGo = Resources.Load<GameObject> ("Letters/" + letterName.ToUpper ());
//		GameObject child = _letterHolder.transform.GetChild (0).gameObject;
//
//		Image image = child.GetComponent<Image> ();
//		image.sprite = letterGo.GetComponent<Image> ().sprite;
//
//		image.SetNativeSize ();
//		RectTransform rectTrans = child.GetComponent<RectTransform> ();
//		Vector3 size = rectTrans.sizeDelta;
//		size.x = size.x * _scaleTemplateFactor;
//		size.y = size.y * _scaleTemplateFactor;
//		rectTrans.sizeDelta = size;

		_currentLetterBut = _letterHolder.transform.GetChild (0).GetComponent<UILetterButton> ();
		_currentLetterBut.Letter = WordDrawConfig.GetLetterFromName (letterName);

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
			StartCoroutine (ExitAfterDetect());
		} else {
			_tutText.SetTutText (UITutText.TutText.TRY_AGAIN);
		}
	}

	IEnumerator ExitAfterDetect(){
		yield return new WaitForSeconds (1.0f);
		OnExitClick ();
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

	void HandleLetterAutoDrawDone(){
		_tutText.SetTutText (UITutText.TutText.LET_WRITE);
		SetActiveInputGesture (true);
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
//		string gestureName = _currentLetterBut.Letter + "training";
//		_autoDrawer.AutoDrawGesture (gestureName);

		sampleLetter = Instantiate (Resources.Load<GameObject> ("StartUpLetters/" + targetName));
		sampleLetter.transform.SetParent (_letterHolder.transform, false);
	}

	void DestroySampleLetter(){
		if(sampleLetter != null){
			Destroy (sampleLetter);
			sampleLetter = null;
		}
	}

	private IEnumerator DelayResetStroke ()
	{
		yield return new WaitForSeconds (_showTemplateDuration);

		SetActiveInputGesture (true);

//		_autoDrawer.ResetStroke ();
		DestroySampleLetter ();
		_tutText.SetTutText (UITutText.TutText.LET_WRITE);
	}

	public void _CaptureAndSave ()
	{
		snapShot.CaptureAndSaveToAlbum ();
		StartCoroutine (ShowDialog ());
	}

	IEnumerator ShowDialog ()
	{
		yield return null;
		GUIController.Instance.OpenDialog (LeanLocalization.GetTranslation ("PhotoCaptured").Text).AddButton (
			LeanLocalization.GetTranslation ("Ok").Text, 
			UIDialogButton.Anchor.CENTER, 0, -25,
			() => {
			}
		);
	}

	public void _PlayModelSound ()
	{
		AudioClip clip = Resources.Load<AudioClip> ("Sounds/" + targetName);
		if (clip != null) {
			AudioManager.Instance.PlayTemp (clip);
		}
	}
}
