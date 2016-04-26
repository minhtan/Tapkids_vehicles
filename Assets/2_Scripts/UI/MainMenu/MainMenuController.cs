using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour {
	float width;
	float height;

	public RectTransform menu;
	public RectTransform car;
	public RectTransform garage;
	float menuInitPos;

	public Camera carRenderCam;
	public RawImage carRawImage;

	bool isPressingOnWheel = false;

	void Start(){
		CreateCarTexture ();
		InitPosition ();
	}

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

	void OnEnable(){
		Lean.LeanTouch.OnFingerSwipe += OnFingerSwipe;
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
			} else {
				
			}
		}

		//swipe right
		if (swipe.x > Mathf.Abs(swipe.y))
		{
			if (!isPressingOnWheel) {
				_OpenGarage ();
			} else {
				
			}
		}

		_OnWheelPress (false);
	}

}
