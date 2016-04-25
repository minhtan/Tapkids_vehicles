using UnityEngine;
using System.Collections;

public class MainMenuController : MonoBehaviour {
	float width;

	public RectTransform menu;
	public RectTransform car;
	public RectTransform garage;
	float carInitPos;
	float menuInitPos;
	float garageInitPos;
	float offset;

	void Start(){
		width = Screen.width;
		offset = menu.sizeDelta.x + (width - menu.sizeDelta.x - car.sizeDelta.x) / 2; 
		car.anchoredPosition = new Vector2(-offset, car.anchoredPosition.y);
		carInitPos = car.anchoredPosition.x;
		menuInitPos = menu.anchoredPosition.x;
		garageInitPos = garage.anchoredPosition.x;
	}

	public void _OpenGarage(){
		LeanTween.moveX (menu, menu.sizeDelta.x, 0.5f).setEase(LeanTweenType.easeInBack);
		LeanTween.moveX (car, 0, 0.5f).setEase(LeanTweenType.easeInBack);
		LeanTween.moveX (garage, 0, 0.5f).setEase(LeanTweenType.easeInBack);
	}

	public void _BackToMenu(){
		LeanTween.moveX (menu, menuInitPos, 0.5f).setEase(LeanTweenType.easeInBack);
		LeanTween.moveX (car, carInitPos, 0.5f).setEase(LeanTweenType.easeInBack);
		LeanTween.moveX (garage, garageInitPos, 0.5f).setEase(LeanTweenType.easeInBack);
	}
}
