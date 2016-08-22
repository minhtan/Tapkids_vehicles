using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UITutorialPanel : MonoBehaviour {

	#region public members
	public GameObject ipad;
	public GameObject icon;
	public GameObject bubbleText;
	public GameObject backButton;
	public GameObject nextButton;
	public GameObject skipButton;
	#endregion public members

	#region private members
	private CanvasGroup mCanvasGroup;
	private int currentStep = 1;
	#endregion private members

	#region Mono
	void OnEnable () {
		Messenger.AddListener <bool> (EventManager.GUI.TOGGLE_TUTORIAL.ToString (), HandleToggleTutorial);
		Messenger.AddListener <int> (EventManager.GUI.NEXT_TUT_BTN.ToString (), DoSomething);
		Messenger.AddListener <int> (EventManager.GUI.BACK_TUT_BTN.ToString (), DoSomething);
		Messenger.AddListener <int> (EventManager.GUI.SKIP_TUT_BTN.ToString (), DoSomething);
	}

	void Start () {
		mCanvasGroup = GetComponent <CanvasGroup> ();
		DisplayStep (currentStep);
	}

	void OnDisable () {
		Messenger.RemoveListener <bool> (EventManager.GUI.TOGGLE_TUTORIAL.ToString (), HandleToggleTutorial);
		Messenger.RemoveListener <int> (EventManager.GUI.NEXT_TUT_BTN.ToString (), DoSomething);
		Messenger.RemoveListener <int> (EventManager.GUI.BACK_TUT_BTN.ToString (), DoSomething);
		Messenger.RemoveListener <int> (EventManager.GUI.SKIP_TUT_BTN.ToString (), DoSomething);
	}
	#endregion Mono

	#region public functions
	#endregion public functions

	#region private functions
	private void HandleToggleTutorial (bool _isFound) {
		if (mCanvasGroup == null) return;

		mCanvasGroup.alpha = _isFound ? 1f : 0f;
		mCanvasGroup.blocksRaycasts = _isFound ? true : false;
		mCanvasGroup.interactable = _isFound ? true : false;
	}

	private void DoSomething (int increment) {
		currentStep += increment;
		DisplayStep(currentStep);
	}

	private void DisplayStep (int step) {
		switch(step) {
		case 0:
			// skip tut
			break;
		case 1:
			// display ipad, tapu icon, buttons
			LeanTween.value (ipad, 0f, 1f, 1f)
				.setOnUpdate ((float alpha) => ipad.GetComponent<CanvasGroup> ().alpha = alpha)
				.setOnComplete (() => {
					LeanTween.value (icon, 0f, 1f, 1f)
						.setOnUpdate ((float alpha) => icon.GetComponent<CanvasGroup> ().alpha = alpha)
						.setOnComplete (() => {
							bubbleText.GetComponent<Text> ().text = "Scan Any Vehicle Card";
							LeanTween.value (bubbleText, 0f, 1f, 1f)
								.setOnUpdate ((float alpha) => bubbleText.GetComponent<CanvasGroup> ().alpha = alpha)
								.setOnComplete (() => {
									LeanTween.value (nextButton, 0f, 1f, 1f)
										.setOnUpdate ((float alpha) => nextButton.GetComponent<CanvasGroup> ().alpha = alpha)
										.setOnComplete (() => {
											nextButton.GetComponent<CanvasGroup> ().interactable = true;
											nextButton.GetComponent<CanvasGroup> ().blocksRaycasts = true;
											LeanTween.value (skipButton, 0f, 1f, 1f)
												.setOnUpdate ((float alpha) => skipButton.GetComponent<CanvasGroup> ().alpha = alpha)
												.setOnComplete (() => {
													skipButton.GetComponent<CanvasGroup> ().interactable = true;
													skipButton.GetComponent<CanvasGroup> ().blocksRaycasts = true;
												});
										});
								});
						});
				});
			Debug.Log (step);
			break;

		case 2:
			Debug.Log (step);
			break;

		case 3:
			Debug.Log (step);
			break;
		}
	}

	#endregion private functions

}
