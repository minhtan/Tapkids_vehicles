using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;
using System.Collections.Generic;
using Lean;
using System.Linq;

public class MainMenuController : MonoBehaviour {
	#region var
	float width;
	float height;

	float distance;
	bool isGarageOpen = false;
	float menuTweenTime = 0.3f;

	public RectTransform menu;
	public RectTransform car;
	public RectTransform garage;
	int[] menutweenIds = new int[3];

	float menuOFDpos;
	float carOFDpos;
	float garageOFDpos;

	public Camera carRenderCam;
	public RawImage carRawImage;

	bool isPressingOnWheel = false;
	Dictionary<SceneController.SceneID, string> sceneToLeanTransPhrase;
	List<string> gameNames = new List<string>();
	int currentIndex = 0;

	public RectTransform pnlTitles;
	public GameObject txtGameTitle;
	float displacmentOfText;

	List<Sprite> gameImages = new List<Sprite>();
	public RectTransform pnlGameImages;
	public GameObject imgGameImage;
	float displacmentOfImage;
	#endregion

	#region mono
	void Start(){
//		if (GUIController.Instance != null) {
		Messenger.Broadcast <bool> (EventManager.GUI.TOGGLE_MENU_BTN.ToString (), false);
		Messenger.Broadcast <bool> (EventManager.GUI.TOGGLE_PLAYER_PNL.ToString (), true);
//		}
		CreateCarTexture ();
		InitPosition ();
		SetUpSceneIndex ();
		UpdateLocalization ();
		SetUpUI ();
	}

	void OnEnable(){
		Lean.LeanTouch.OnFingerDown += OnFingerDown;
		Lean.LeanTouch.OnFingerUp += OnFingerUp;
		Lean.LeanTouch.OnFingerDrag += OnFingerDrag;
		Lean.LeanTouch.OnFingerSwipe += OnFingerSwipe;
		Messenger.AddListener<float> (EventManager.GUI.MENU_WHEEL_TURN.ToString(), OnMenuWheelTurn);
		Messenger.AddListener<float> (EventManager.GUI.MENU_WHEEL_RELEASE.ToString(), OnMenuWheelRelease);
		LeanLocalization.OnLocalizationChanged += UpdateLocalization;
	}

	void OnDisable(){
		Lean.LeanTouch.OnFingerDown -= OnFingerDown;
		Lean.LeanTouch.OnFingerUp -= OnFingerUp;
		Lean.LeanTouch.OnFingerDrag -= OnFingerDrag;
		Lean.LeanTouch.OnFingerSwipe -= OnFingerSwipe;
		Messenger.RemoveListener<float> (EventManager.GUI.MENU_WHEEL_TURN.ToString(), OnMenuWheelTurn);
		Messenger.RemoveListener<float> (EventManager.GUI.MENU_WHEEL_RELEASE.ToString(), OnMenuWheelRelease);
		LeanLocalization.OnLocalizationChanged -= UpdateLocalization;
	}
	#endregion

	#region tween menu
	void InitPosition(){
		LeanTween.moveX (car, -width/2, 0f);
		LeanTween.moveX (garage, -width/2, 0f);
	}

	void CancelMenuTweens(){
		for (int i = 0; i < menutweenIds.Length; i++) {
			if(menutweenIds[i] != null && menutweenIds[i] != 0){
				LeanTween.cancel (menutweenIds[i]);
			}
		}
	}

	public void _OpenGarage(){
		CancelMenuTweens ();
		menutweenIds[0] = LeanTween.moveX (menu, width / 2, menuTweenTime).setEase (LeanTweenType.easeOutQuart).id;
		menutweenIds[1] = LeanTween.moveX (car, 0, menuTweenTime).setEase (LeanTweenType.easeOutQuart).id;
		menutweenIds[2] = LeanTween.moveX (garage, 0, menuTweenTime).setEase (LeanTweenType.easeOutQuart).setOnComplete ( () => {
			isGarageOpen = true;
			Messenger.Broadcast (EventManager.GUI.TO_GARAGE.ToString ());
		}).id;

	}

	public void _BackToMenu(){
		CancelMenuTweens ();
		menutweenIds[0] = LeanTween.moveX (menu, 0, menuTweenTime).setEase (LeanTweenType.easeOutQuart).id;
		menutweenIds[1] = LeanTween.moveX (car, -width / 2, menuTweenTime).setEase (LeanTweenType.easeOutQuart).id;
		menutweenIds[2] = LeanTween.moveX (garage, -width / 2, menuTweenTime).setEase (LeanTweenType.easeOutQuart).setOnComplete ( () => {
			isGarageOpen = false;
			Messenger.Broadcast (EventManager.GUI.TO_MENU.ToString ());
		}).id;
	}

	void CreateCarTexture(){
		width = GetComponent<RectTransform>().sizeDelta.x;
		height = GetComponent<RectTransform>().sizeDelta.y;

		RenderTexture rt = new RenderTexture ((int)(width), (int)height*2, 24);
		carRenderCam.targetTexture = rt;
		carRawImage.texture = rt;
	}

	public void _OnWheelPress(bool isTouching){
		isPressingOnWheel = isTouching;
	}

	void OnFingerDown(Lean.LeanFinger finger){
		if (!isPressingOnWheel) {
			distance = 0;
			menuOFDpos = menu.anchoredPosition.x;
			carOFDpos = car.anchoredPosition.x;
			garageOFDpos = garage.anchoredPosition.x;
		}
	}

	void OnFingerUp(Lean.LeanFinger finger){
		if (!isPressingOnWheel) {
			if (distance <= -width / 4) {
				MoveLeft ();
			} 
			else if (distance >= width / 4) {
				MoveRight ();
			} 
			else {
				SnapBack ();
			}
			_OnWheelPress (false);
		}
	}

	void MoveLeft(){
//		if (isGarageOpen) {
			_BackToMenu ();
//		} else {
//			
//		}
	}

	void MoveRight(){
//		if (isGarageOpen) {
			_OpenGarage ();
//		} else {
//			
//		}
	}

	void SnapBack(){
		if (isGarageOpen) {
			_OpenGarage ();
		} else {
			_BackToMenu ();
		}
	}

	void OnFingerDrag(Lean.LeanFinger finger){
		if (!isPressingOnWheel) {
			distance += finger.DeltaScreenPosition.x;
			LeanTween.moveX (menu, menuOFDpos + distance, 0f);
			LeanTween.moveX (car, carOFDpos + distance, 0f);
			LeanTween.moveX (garage, garageOFDpos + distance, 0f);
		}
	}

	void OnFingerSwipe(Lean.LeanFinger finger){
		if (!isPressingOnWheel) {
			var swipe = finger.SwipeDelta;

			//swipe left
			if (swipe.x < -Mathf.Abs (swipe.y)) {
				_BackToMenu ();
			}

			//swipe right
			if (swipe.x > Mathf.Abs (swipe.y)) {
				_OpenGarage ();
			}
		}
		_OnWheelPress (false);
	}
	#endregion

	#region wheel handling
	void OnMenuWheelTurn(float angle){

	}

	void OnMenuWheelRelease(float angle){
		if(Mathf.Abs(angle) > SteeringWheel.angleThreshold/2){
			bool isNext = angle > 0 ? true : false;
			if (isNext) {
				currentIndex++;
			} else{
				currentIndex--;
			} 

			currentIndex = CycleIndex (currentIndex);
			ChangeTitle (currentIndex, isNext);
			ChangeImage (currentIndex, isNext);
		}
	}

	public void UpdateLocalization()
	{
		try {
			gameNames.Clear ();
			foreach(KeyValuePair<SceneController.SceneID, string> entry in sceneToLeanTransPhrase)
			{
				gameNames.Add (LeanLocalization.GetTranslation(entry.Value).Text);
			}
			ChangeTitleToCurrentIndex();
		} catch (NullReferenceException ex) {
			Debug.Log (ex.ToString ());
		}
	}

	void SetUpGameImages(){
		foreach(KeyValuePair<SceneController.SceneID, string> entry in sceneToLeanTransPhrase)
		{
			gameImages.Add (DataUltility.GetGameImage(entry.Value));
		}
		ChangeGameImageToCurrentIndex ();
	}

	void SetUpSceneIndex(){
		sceneToLeanTransPhrase = new Dictionary<SceneController.SceneID, string> (){
			{SceneController.SceneID.STARTUP, "Startup"},
			{SceneController.SceneID.CARGAME, "Cargame"},
			{SceneController.SceneID.CARGAME2, "Cargame2"},
			{SceneController.SceneID.WORDGAME, "WordGame"},
			{SceneController.SceneID.WORDDRAWGAME, "WordDrawGame"}
			//the phrases must be identical to phrases in localization files in 7_Localization folder
		};
	}

	void SetUpUI(){
		displacmentOfText = pnlTitles.sizeDelta.x;
		displacmentOfImage = pnlGameImages.sizeDelta.x;
		LeanTween.moveX (txtGameTitle.GetComponent<RectTransform> (), 0f, 0f);
		LeanTween.moveX (imgGameImage.GetComponent<RectTransform> (), 0f, 0f);
		SetUpGameImages ();
	}

	void ChangeTitleToCurrentIndex(){
		txtGameTitle.GetComponent<Text> ().text = gameNames [currentIndex];
	}

	void ChangeGameImageToCurrentIndex(){
		imgGameImage.GetComponent<Image> ().sprite = gameImages [currentIndex];
	}

	int CycleIndex(int index){
		if(index < 0){
			index = sceneToLeanTransPhrase.Count-1;
		}else if(currentIndex > sceneToLeanTransPhrase.Count-1){
			index = 0;
		}
		return index;
	}

	void ChangeTitle(int index, bool isNext){
		GameObject nextTitle = Instantiate (txtGameTitle);
		nextTitle.GetComponent<Text> ().text = gameNames [index];
		nextTitle.transform.SetParent (pnlTitles, false);

		float tempDis = isNext ? -displacmentOfText : displacmentOfText;
		LeanTween.textAlpha (nextTitle.GetComponent<RectTransform> (), 0f, 0f);
		LeanTween.moveX (nextTitle.GetComponent<RectTransform> (), tempDis, 0f).setOnComplete( () => {
			LeanTween.textAlpha (nextTitle.GetComponent<RectTransform> (), 1f, 0.5f).setEase(LeanTweenType.easeOutExpo);
			LeanTween.moveX (nextTitle.GetComponent<RectTransform> (), 0f, 0.5f).setEase(LeanTweenType.easeOutBack);

			LeanTween.textAlpha (txtGameTitle.GetComponent<RectTransform>(), 0f, 0.5f).setEase(LeanTweenType.easeOutExpo);
			LeanTween.moveX (txtGameTitle.GetComponent<RectTransform> (), -tempDis, 0.5f).setEase(LeanTweenType.easeOutBack).setDestroyOnComplete(true);

			txtGameTitle = nextTitle;
		});
	}

	void ChangeImage(int index, bool isNext){
		GameObject nextImage = Instantiate (imgGameImage);
		nextImage.GetComponent<Image> ().sprite = gameImages [index];
		nextImage.transform.SetParent (pnlGameImages, false);

		float tempDis = isNext ? -displacmentOfImage : displacmentOfImage;
		LeanTween.alpha (nextImage.GetComponent<RectTransform> (), 0f, 0f);
		LeanTween.moveX (nextImage.GetComponent<RectTransform> (), tempDis, 0f).setOnComplete( () => {
			LeanTween.alpha (nextImage.GetComponent<RectTransform> (), 1f, 0.5f).setEase(LeanTweenType.easeOutExpo);
			LeanTween.moveX (nextImage.GetComponent<RectTransform> (), 0f, 0.5f).setEase(LeanTweenType.easeOutBack);

			LeanTween.alpha (imgGameImage.GetComponent<RectTransform>(), 0f, 0.5f).setEase(LeanTweenType.easeOutExpo);
			LeanTween.moveX (imgGameImage.GetComponent<RectTransform> (), -tempDis, 0.5f).setEase(LeanTweenType.easeOutBack).setDestroyOnComplete(true);

			imgGameImage = nextImage;
		});
	}

	public void _OnPlayClick(){
		if (SceneController.Instance != null) 
			SceneController.Instance.LoadingSceneAsync (sceneToLeanTransPhrase.Keys.ElementAt(currentIndex));
	}
	#endregion
}
