﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class UIOffScreenIndicator : MonoBehaviour {

	//	public Image OnScreenSprite; // can use gameobject with spriterenderer, or reference a Sprite here, and create a new gameobject, add a sprite renderer to it.
//	public Image offScreenSprite;

	//public Image[] onScreenSprites; // sprite object pooling

	#region public members
	public RectTransform letterIndicator;
	public RectTransform carIndicator;
	#endregion public members

	#region private members
	private GameObject car;
	private RectTransform offScreenCar;

	private List<GameObject> letters;
	private RectTransform[] offScreenLetters;

	private float screenWidth = Screen.width;
	private float screenHeight = Screen.height;

	private float screenOffSet = 15f;

	private bool isInitiated;
	private int letterCount;

	private RectTransform mRectTransform;
	#endregion private member

	#region MONO
	void OnEnable () {
		Messenger.AddListener <string, string> (EventManager.GameState.INIT.ToString (), HandleInitGame);
		Messenger.AddListener <bool> (EventManager.GameState.START.ToString (), HandleStartGame);
	}

	void OnDisable () {
		Messenger.RemoveListener <string, string> (EventManager.GameState.INIT.ToString (), HandleInitGame);
		Messenger.RemoveListener <bool>  (EventManager.GameState.START.ToString (), HandleStartGame);
	}

	void Start () {
		mRectTransform = GetComponent <RectTransform> ();
//		letters = new List<GameObject> ();
	}

	void Update () {
		if (!isInitiated) return;

		CarIndicator ();
		LetterIndicator ();
	}
	#endregion MONO

	#region private functions
	void HandleInitGame (string envLetter, string _letters) {
		if(_letters.Length <= 0) return;

		// pre setup car indicator
		offScreenCar = Instantiate (carIndicator) as RectTransform;
		offScreenCar.SetParent (mRectTransform);
		offScreenCar.gameObject.SetActive (false);

		//TODO: pre setup letter's indicators
		offScreenLetters = new RectTransform[_letters.Length];
		for (int i = 0; i < _letters.Length; i++) {
			offScreenLetters[i] = Instantiate (letterIndicator) as RectTransform; 
			offScreenLetters[i].GetComponentInChildren <Text> ().text = _letters[i].ToString ();
			offScreenLetters[i].SetParent (mRectTransform, false);
			offScreenLetters[i].gameObject.SetActive (false);
		}
	}

	void HandleStartGame (bool state) {
		if (state) 
			isInitiated = true;
	}

	private void CarIndicator () {
		// looking for vehicle in scene
		if(car == null) {
			car = GameObject.FindGameObjectWithTag ("Vehicle");
			return;
		} 

		if (!car.activeInHierarchy) {
			offScreenCar.gameObject.SetActive (false);
		} else {
			Vector3 onScreenPosition = Camera.main.WorldToScreenPoint (car.transform.position);
			// ON SCREEN
			if (onScreenPosition.z > 0 && (onScreenPosition.x > 0 && onScreenPosition.x < screenWidth) && (onScreenPosition.y > 0 && onScreenPosition.y < screenHeight)) {
				if (offScreenCar.gameObject.activeInHierarchy)
					offScreenCar.gameObject.SetActive (false);
			} else { // OFF SCREEN
				float x = onScreenPosition.x;
				float y = onScreenPosition.y;

				if (onScreenPosition.z < 0) {
					onScreenPosition = -onScreenPosition;
				}

				if (onScreenPosition.x > screenWidth) {
					x = screenWidth - screenOffSet;
				}

				if (onScreenPosition.x < 0) {
					x = screenOffSet;
				}

				if(onScreenPosition.y > screenHeight) {
					y = screenHeight - screenOffSet;
				}

				if (onScreenPosition.y < 0) {
					y = screenOffSet;
				}

				offScreenCar.gameObject.SetActive (true);
				offScreenCar.position = new Vector3 (x, y, 0);
			}
		}
	}

	private void LetterIndicator () {
		if (letterCount < offScreenLetters.Length) {
			GameObject[]  _letters = GameObject.FindGameObjectsWithTag ("Letter");
			letterCount = _letters.Length;
			letters = new List<GameObject> (_letters);
			return;
		}

		for (int i = 0; i < letters.Count; i++) {
			if (!letters[i].activeInHierarchy) {
				offScreenLetters[i].gameObject.SetActive (false);
			} else {
				Vector3 onScreenPosition = Camera.main.WorldToScreenPoint (letters[i].transform.position);

				// ON SCREEN
				if (onScreenPosition.z > 0 && (onScreenPosition.x > 0 && onScreenPosition.x < screenWidth) && (onScreenPosition.y > 0 && onScreenPosition.y < screenHeight)) {
					if (offScreenLetters[i].gameObject.activeInHierarchy)
						offScreenLetters[i].gameObject.SetActive (false);
				} else { // OFF SCREEN
					float x = onScreenPosition.x;
					float y = onScreenPosition.y;

					if (onScreenPosition.z < 0) {
						onScreenPosition = -onScreenPosition;
					}

					if (onScreenPosition.x > screenWidth) {
						x = screenWidth - screenOffSet;
					}

					if (onScreenPosition.x < 0) {
						x = screenOffSet;
					}

					if(onScreenPosition.y > screenHeight) {
						y = screenHeight - screenOffSet;
					}

					if (onScreenPosition.y < 0) {
						y = screenOffSet;
					}

					offScreenLetters[i].gameObject.SetActive (true);
					offScreenLetters[i].position = new Vector3 (x, y, 0);
				}
			}
		}
	}
	#endregion private functions
}﻿
