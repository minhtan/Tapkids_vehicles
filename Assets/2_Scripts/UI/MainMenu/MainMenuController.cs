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

	public RectTransform menu;
	public RectTransform car;
	public RectTransform garage;
	float menuInitPos;

	public Camera carRenderCam;
	public RawImage carRawImage;

	bool isPressingOnWheel = false;
	Dictionary<SceneController.SceneID, string> sceneToLeanTransPhrase;
	List<string> sceneName = new List<string>();
	int currentIndex = 0;

	public RectTransform pnlTitles;
	public GameObject txtGameTitle;
	float displacement;
	#endregion

	#region mono
	void Start(){
		if (GUIController.Instance != null) {
			Messenger.Broadcast <bool> (EventManager.GUI.TOGGLE_INGAME.ToString (), false);
		}
		CreateCarTexture ();
		InitPosition ();
		SetUpSceneIndex ();
		UpdateLocalization ();
		SetUpUI ();
	}

	void OnEnable(){
		Lean.LeanTouch.OnFingerSwipe += OnFingerSwipe;
		Messenger.AddListener<float> (EventManager.GUI.MENUWHEELTURN.ToString(), OnMenuWheelTurn);
		Messenger.AddListener<float> (EventManager.GUI.MENUWHEELRELEASE.ToString(), OnMenuWheelRelease);
		LeanLocalization.OnLocalizationChanged += UpdateLocalization;
	}

	void OnDisable(){
		Lean.LeanTouch.OnFingerSwipe -= OnFingerSwipe;
		Messenger.RemoveListener<float> (EventManager.GUI.MENUWHEELTURN.ToString(), OnMenuWheelTurn);
		Messenger.RemoveListener<float> (EventManager.GUI.MENUWHEELRELEASE.ToString(), OnMenuWheelRelease);
		LeanLocalization.OnLocalizationChanged -= UpdateLocalization;
	}
	#endregion

	#region tween menu
	void InitPosition(){
		LeanTween.moveX (car, -width/2, 0f);
		LeanTween.moveX (garage, -width/2, 0f);
		menuInitPos = menu.anchoredPosition.x;
	}

	public void _OpenGarage(){
		LeanTween.moveX (menu, menu.sizeDelta.x, 0.5f).setEase(LeanTweenType.easeInBack);
		LeanTween.moveX (car, 0, 0.5f).setEase(LeanTweenType.easeInBack);
		LeanTween.moveX (garage, 0, 0.5f).setEase(LeanTweenType.easeInBack);
	}

	public void _BackToMenu(){
		LeanTween.moveX (menu, menuInitPos, 0.5f).setEase(LeanTweenType.easeInBack);
		LeanTween.moveX (car, -width/2, 0.5f).setEase(LeanTweenType.easeInBack);
		LeanTween.moveX (garage, -width/2, 0.5f).setEase(LeanTweenType.easeInBack);
	}

	void CreateCarTexture(){
		width = Screen.width;
		height = Screen.height;

		RenderTexture rt = new RenderTexture ((int)(width / 2), (int)height, 24);
		carRenderCam.targetTexture = rt;
		carRawImage.texture = rt;
	}

	public void _OnWheelPress(bool isTouching){
		isPressingOnWheel = isTouching;
	}

	void OnFingerSwipe(Lean.LeanFinger finger){
		var swipe = finger.SwipeDelta;

		//swipe left
		if (swipe.x < -Mathf.Abs(swipe.y))
		{
			if (!isPressingOnWheel) {
				_BackToMenu ();
			}
		}

		//swipe right
		if (swipe.x > Mathf.Abs(swipe.y))
		{
			if (!isPressingOnWheel) {
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
			Debug.Log (isNext);
			if (isNext) {
				currentIndex++;
			} else{
				currentIndex--;
			} 

			currentIndex = CycleIndex (currentIndex);
			ChangeTitle (currentIndex, isNext);
		}
	}

	public void UpdateLocalization()
	{
		try {
			sceneName.Clear ();
			foreach(KeyValuePair<SceneController.SceneID, string> entry in sceneToLeanTransPhrase)
			{
				sceneName.Add (LeanLocalization.GetTranslation(entry.Value).Text);
			}
			ChangeTitleToCurrentIndex();
		} catch (NullReferenceException ex) {
			Debug.Log (ex.ToString ());
		}
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
		displacement = pnlTitles.sizeDelta.x;
		LeanTween.moveX (txtGameTitle.GetComponent<RectTransform> (), 0, 0f);
	}

	void ChangeTitleToCurrentIndex(){
		txtGameTitle.GetComponent<Text> ().text = sceneName [currentIndex];
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
		nextTitle.GetComponent<Text> ().text = sceneName [currentIndex];
		nextTitle.transform.SetParent (pnlTitles, false);

		float tempDis = isNext ? -displacement : displacement;
		LeanTween.moveX (nextTitle.GetComponent<RectTransform> (), tempDis, 0f).setOnComplete( () => {
			LeanTween.moveX (nextTitle.GetComponent<RectTransform> (), 0, 0.5f);
			LeanTween.moveX (txtGameTitle.GetComponent<RectTransform> (), -tempDis, 0.5f).setDestroyOnComplete(true);
			txtGameTitle = nextTitle;
		});
	}

	public void _OnPlayClick(){
		SceneController.Instance.LoadingSceneAsync (sceneToLeanTransPhrase.Keys.ElementAt(currentIndex));
	}
	#endregion
}
