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
		car.anchoredPosition = new Vector2(-car.sizeDelta.x/4, car.anchoredPosition.y);
		carInitPos = car.anchoredPosition.x;
		menuInitPos = menu.anchoredPosition.x;
		garageInitPos = garage.anchoredPosition.x;
		offset = (width / 2) - car.sizeDelta.x;
	}

	public void _OpenGarage(){
		LeanTween.moveX (menu, menu.sizeDelta.x, 0.5f).setEase(LeanTweenType.easeInBack);
		LeanTween.moveX (car, -carInitPos*2 + offset, 0.5f).setEase(LeanTweenType.easeInBack);
		LeanTween.moveX (garage, 0, 0.5f).setEase(LeanTweenType.easeInBack);
	}

	public void _BackToMenu(){
		LeanTween.moveX (menu, menuInitPos, 0.5f).setEase(LeanTweenType.easeInBack);
		LeanTween.moveX (car, carInitPos, 0.5f).setEase(LeanTweenType.easeInBack);
		LeanTween.moveX (garage, garageInitPos, 0.5f).setEase(LeanTweenType.easeInBack);
	}
}
