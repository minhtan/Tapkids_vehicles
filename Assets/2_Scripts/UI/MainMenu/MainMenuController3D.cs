using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Lean;

public class MainMenuController3D : MonoBehaviour {

	public RawImage rawImage;
	public Camera renderCam;
	Vector3 menuPos;
	public Vector3 garagePos;
	float menuTweenTime = 0.3f;
	float width;
	float height;

	// Use this for initialization
	void Start () {
//		CreateTexture ();
		menuPos = renderCam.gameObject.transform.localRotation.eulerAngles;
	}
	
	void OnEnable(){
		LeanTouch.OnFingerSwipe += OnFingerSwipe;
	}

	void OnDisable(){
		LeanTouch.OnFingerSwipe -= OnFingerSwipe;
	}

	void OnFingerSwipe(LeanFinger finger){
		var swipe = finger.SwipeDelta;

		//swipe left
		if (swipe.x < -Mathf.Abs (swipe.y)) {
			BackToMenu ();
		}

		//swipe right
		if (swipe.x > Mathf.Abs (swipe.y)) {
			OpenGarage ();
		}
	}
		
	void CreateTexture(){
		width = GetComponent<RectTransform>().sizeDelta.x;
		height = GetComponent<RectTransform>().sizeDelta.y;

		RenderTexture rt = new RenderTexture ((int)width*2, (int)height*2, 24);
		renderCam.targetTexture = rt;
		rawImage.texture = rt;
	}
		
	void BackToMenu(){
		LeanTween.rotateLocal (renderCam.gameObject, menuPos, menuTweenTime);
	}

	void OpenGarage(){
		LeanTween.rotateLocal (renderCam.gameObject, garagePos, menuTweenTime);
	}
}
