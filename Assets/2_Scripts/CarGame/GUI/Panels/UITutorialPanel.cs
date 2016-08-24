using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UITutorialPanel : MonoBehaviour {

	#region public members
	public GameObject ipad;
	public GameObject bgImage;
	public GameObject scanText;
	public GameObject vehicleTargetImage;
	public GameObject mapTargetImage;
	public GameObject icon;
	public GameObject bubbleText;
	public GameObject previousButton;
	public GameObject nextButton;
	public GameObject skipButton;

	public GameObject HUDImage;

	#endregion public members

	#region private members
	private CanvasGroup mCanvasGroup;
	private int currentStep = 1;

	private float duration = 0.5f;
	#endregion private members

	#region Mono
	void OnEnable () {
		Messenger.AddListener <bool> (EventManager.GUI.TOGGLE_TUTORIAL.ToString (), HandleToggleTutorial);
		Messenger.AddListener <int> (EventManager.GUI.NEXT_STEP_BTN.ToString (), HandleTutorialButtons);
		Messenger.AddListener <int> (EventManager.GUI.PREVIOUS_STEP_BTN.ToString (), HandleTutorialButtons);
		Messenger.AddListener <int> (EventManager.GUI.SKIP_TUT_BTN.ToString (), HandleTutorialButtons);
	}

	void Start () {
		mCanvasGroup = GetComponent <CanvasGroup> ();
//		DisplayLostLetterTutorial (currentStep);
		DisplayRoamingLetterTutorial (currentStep);
	}

	void OnDisable () {
		Messenger.RemoveListener <bool> (EventManager.GUI.TOGGLE_TUTORIAL.ToString (), HandleToggleTutorial);
		Messenger.RemoveListener <int> (EventManager.GUI.NEXT_STEP_BTN.ToString (), HandleTutorialButtons);
		Messenger.RemoveListener <int> (EventManager.GUI.PREVIOUS_STEP_BTN.ToString (), HandleTutorialButtons);
		Messenger.RemoveListener <int> (EventManager.GUI.SKIP_TUT_BTN.ToString (), HandleTutorialButtons);
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

	private void HandleTutorialButtons (int increment) {
		if (increment == 0) {
			HandleToggleTutorial (false);
			return;
		}

		currentStep += increment;
		if (currentStep <= 3) {
			if (SceneController.Instance.GetCurrentSceneID () == SceneController.SceneID.CARGAME) {
				DisplayLostLetterTutorial (currentStep);
			} else if (SceneController.Instance.GetCurrentSceneID () == SceneController.SceneID.CARGAME2) {
				DisplayRoamingLetterTutorial (currentStep);
			}
		} else {
//			PlayerPrefs.SetInt (GameConstant.hasPlayedTutorial, 2);
			if (SceneController.Instance.GetCurrentSceneID () == SceneController.SceneID.CARGAME) {
				PlayerDataController.Instance.UpdatePlayerTutorialState (0, true);
			} else if (SceneController.Instance.GetCurrentSceneID () == SceneController.SceneID.CARGAME2) {
				PlayerDataController.Instance.UpdatePlayerTutorialState (1, true);
			}

			HandleToggleTutorial (false);
		}
	}

	private void DisplayLostLetterTutorial (int step) {
		switch(step) {
		case 1:
			skipButton.SetActive (true);
			nextButton.SetActive (true);
			previousButton.SetActive (false);

			nextButton.GetComponent<Button> ().interactable = false;
			previousButton.GetComponent<Button> ().interactable = false;

			if (bgImage.GetComponent<CanvasGroup> ().alpha == 1f) {
				LeanTween.value (bgImage, 1f, 0f, duration)
					.setOnUpdate ((float alpha) => bgImage.GetComponent<CanvasGroup> ().alpha = alpha);
			}

			if (icon.GetComponent<CanvasGroup> ().alpha == 0) {
				LeanTween.value (icon, 0f, 1f, duration)
					.setOnUpdate ((float alpha) => icon.GetComponent<CanvasGroup> ().alpha = alpha)
					.setOnComplete (() => {

						bubbleText.GetComponentInChildren<Text> ().text = StringUltil.TextWrap (GameConstant.LostLetterStep1, 10);
						LeanTween.value (bubbleText, 0f, 1f, duration)
							.setOnUpdate ((float alpha) => bubbleText.GetComponent<CanvasGroup> ().alpha = alpha)
							.setOnComplete (() => {
								LeanTween.value (ipad, 0f, 1f, duration)
									.setOnUpdate ((float alpha) => ipad.GetComponent<CanvasGroup> ().alpha = alpha)
									.setOnComplete (() => {
										LeanTween.value (vehicleTargetImage, 0f, 1f, duration)
											.setOnUpdate ((float alpha) => vehicleTargetImage.GetComponent<CanvasGroup> ().alpha = alpha)
											.setOnComplete (() => {
												nextButton.GetComponent<Button> ().interactable = true;
												previousButton.GetComponent<Button> ().interactable = true;
											});

									});
							});
					});
			} else {
				bubbleText.GetComponentInChildren<Text> ().text = StringUltil.TextWrap (GameConstant.LostLetterStep1, 10);
			}

			if (scanText.GetComponent<CanvasGroup> ().alpha == 1f) {
				LeanTween.value (scanText, 1f, 0f, duration)
					.setOnUpdate ((float alpha) => scanText.GetComponent<CanvasGroup> ().alpha = alpha)
					.setOnComplete (() => {
						LeanTween.moveLocalX (vehicleTargetImage, 0f, duration)
							.setOnComplete (() => {
								nextButton.GetComponent<Button> ().interactable = true;
								previousButton.GetComponent<Button> ().interactable = true;
							});
					});
			}

			break;

		case 2:
			skipButton.SetActive (true);
			nextButton.SetActive(true);
			previousButton.SetActive(true);

			nextButton.GetComponent<Button> ().interactable = false;
			previousButton.GetComponent<Button> ().interactable = false;

			if (bgImage.GetComponent<CanvasGroup> ().alpha == 0) {
				LeanTween.value (bgImage, 0f, 1f, duration)
					.setOnUpdate ((float alpha) => bgImage.GetComponent<CanvasGroup> ().alpha = alpha);
			}

			if (mapTargetImage.GetComponent<CanvasGroup> ().alpha == 1) {
				LeanTween.value (mapTargetImage, 1f, 0f, duration)
					.setOnUpdate ((float alpha) => mapTargetImage.GetComponent<CanvasGroup> ().alpha = alpha);
			}

			if (vehicleTargetImage.GetComponent<CanvasGroup> ().alpha == 0f) {
				LeanTween.value (mapTargetImage, 0f, 1f, duration)
					.setOnUpdate ((float alpha) => vehicleTargetImage.GetComponent<CanvasGroup> ().alpha = alpha)
					.setOnComplete (() => {
						LeanTween.value (scanText, 0f, 1f, duration)
							.setOnUpdate ((float alpha) => scanText.GetComponent<CanvasGroup> ().alpha = alpha)
							.setOnComplete (() => {
								bubbleText.GetComponentInChildren <Text> ().text = StringUltil.TextWrap (GameConstant.LostLetterStep2, 11);

								nextButton.GetComponent<Button> ().interactable = true;
								previousButton.GetComponent<Button> ().interactable = true;
							});
					});
			} else {
				LeanTween.moveLocalX (vehicleTargetImage, 96.2f, duration)
					.setOnComplete (() => {
						LeanTween.value (scanText, 0f, 1f, duration)
							.setOnUpdate ((float alpha) => scanText.GetComponent<CanvasGroup> ().alpha = alpha)
							.setOnComplete (() => {
								bubbleText.GetComponentInChildren <Text> ().text = StringUltil.TextWrap (GameConstant.LostLetterStep2, 11);

								nextButton.GetComponent<Button> ().interactable = true;
								previousButton.GetComponent<Button> ().interactable = true;
							});
					});
			}
			break;

		case 3:
			skipButton.SetActive(false);
			nextButton.SetActive(true);
			previousButton.SetActive(true);

			nextButton.GetComponent<Button> ().interactable = false;
			previousButton.GetComponent<Button> ().interactable = false;

			if (bgImage.GetComponent<CanvasGroup> ().alpha == 1) {
				LeanTween.value (bgImage, 1f, 0f, duration)
					.setOnUpdate ((float alpha) => bgImage.GetComponent<CanvasGroup> ().alpha = alpha)
					.setOnComplete (() => {
						bubbleText.GetComponentInChildren <Text> ().text = StringUltil.TextWrap (GameConstant.LostLetterStep3, 12);

						LeanTween.value (vehicleTargetImage, 1f, 0f, duration)
							.setOnUpdate ((float alpha) => vehicleTargetImage.GetComponent<CanvasGroup> ().alpha = alpha);
						LeanTween.value (scanText, 1f, 0f, duration)
							.setOnUpdate ((float alpha) => scanText.GetComponent<CanvasGroup> ().alpha = alpha);
						LeanTween.value (mapTargetImage, 0f, 1f, duration)
							.setOnUpdate ((float alpha) => mapTargetImage.GetComponent<CanvasGroup> ().alpha = alpha)
							.setOnComplete (() => {
								nextButton.GetComponent<Button> ().interactable = true;
								previousButton.GetComponent<Button> ().interactable = true;
							});
 					});
			}

			break;
		}
	}

	private void DisplayRoamingLetterTutorial (int step) {
		switch (step){
		case 1:
			skipButton.SetActive (true);
			nextButton.SetActive (true);
			previousButton.SetActive (false);

			nextButton.GetComponent<Button> ().interactable = false;
			previousButton.GetComponent<Button> ().interactable = false;

			if (HUDImage.GetComponent<CanvasGroup> ().alpha == 1f) {
				LeanTween.value (mapTargetImage, 1f, 0f, duration)
					.setOnUpdate ((float alpha) => HUDImage.GetComponent<CanvasGroup> ().alpha = alpha);
			}

			if (icon.GetComponent<CanvasGroup> ().alpha == 0) {
				LeanTween.value (icon, 0f, 1f, duration)
					.setOnUpdate ((float alpha) => icon.GetComponent<CanvasGroup> ().alpha = alpha)
					.setOnComplete (() => {

						bubbleText.GetComponentInChildren<Text> ().text = StringUltil.TextWrap (GameConstant.RoamingLetterStep1, 12);
						LeanTween.value (bubbleText, 0f, 1f, duration)
							.setOnUpdate ((float alpha) => bubbleText.GetComponent<CanvasGroup> ().alpha = alpha)
							.setOnComplete (() => {
								LeanTween.value (ipad, 0f, 1f, duration)
									.setOnUpdate ((float alpha) => ipad.GetComponent<CanvasGroup> ().alpha = alpha)
									.setOnComplete (() => {
										LeanTween.value (mapTargetImage, 0f, 1f, duration)
											.setOnUpdate ((float alpha) => mapTargetImage.GetComponent<CanvasGroup> ().alpha = alpha)
											.setOnComplete (() => {
												nextButton.GetComponent<Button> ().interactable = true;
												previousButton.GetComponent<Button> ().interactable = true;
											});
									});
							});
					});
			} else {
				LeanTween.value (mapTargetImage, 0f, 1f, duration)
					.setOnUpdate ((float alpha) => mapTargetImage.GetComponent<CanvasGroup> ().alpha = alpha)
					.setOnComplete (() => {
						bubbleText.GetComponentInChildren<Text> ().text = StringUltil.TextWrap (GameConstant.RoamingLetterStep1, 12);
						nextButton.GetComponent<Button> ().interactable = true;
						previousButton.GetComponent<Button> ().interactable = true;
					});
			}
			break;
		case 2:
			skipButton.SetActive (true);
			nextButton.SetActive(true);
			previousButton.SetActive(true);

			nextButton.GetComponent<Button> ().interactable = false;
			previousButton.GetComponent<Button> ().interactable = false;

			if (mapTargetImage.GetComponent<CanvasGroup> ().alpha == 1f) {
				LeanTween.value (mapTargetImage, 1f, 0f, duration)
					.setOnUpdate ((float alpha) => mapTargetImage.GetComponent<CanvasGroup> ().alpha = alpha);
			}

			if (HUDImage.GetComponent<CanvasGroup> ().alpha == 0) {
				LeanTween.value (mapTargetImage, 0f, 1f, duration)
					.setOnUpdate ((float alpha) => HUDImage.GetComponent<CanvasGroup> ().alpha = alpha)
					.setOnComplete (() => {
						bubbleText.GetComponentInChildren<Text> ().text = StringUltil.TextWrap (GameConstant.RoamingLetterStep2, 12);
						nextButton.GetComponent<Button> ().interactable = true;
						previousButton.GetComponent<Button> ().interactable = true;
					});
			} else {
				bubbleText.GetComponentInChildren<Text> ().text = StringUltil.TextWrap (GameConstant.RoamingLetterStep2, 12);
				nextButton.GetComponent<Button> ().interactable = true;
				previousButton.GetComponent<Button> ().interactable = true;
			}

			break;
		case 3:
			skipButton.SetActive(false);
			nextButton.SetActive(true);
			previousButton.SetActive(true);

//			nextButton.GetComponent<Button> ().interactable = false;
//			previousButton.GetComponent<Button> ().interactable = false;

			bubbleText.GetComponentInChildren<Text> ().text = StringUltil.TextWrap (GameConstant.RoamingLetterStep3, 12);

			break;
		}

	}

	#endregion private functions

}
