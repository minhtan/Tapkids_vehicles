using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class OffScreenIndicator : MonoBehaviour {

	//	public Image OnScreenSprite; // can use gameobject with spriterenderer, or reference a Sprite here, and create a new gameobject, add a sprite renderer to it.
	public Image offScreenSprite;

	//public Image[] onScreenSprites; // sprite object pooling


	#region private member
	private GameObject[] letters;
	private Image[] offScreenSprites;

	private float screenWidth = Screen.width;
	private float screenHeight = Screen.height;

	private float screenOffSet = 0;

	#endregion private member

	#region MONO
	// Use this for initialization
	void Start () {
		letters = GameObject.FindGameObjectsWithTag ("Letter");

		// create off screen sprite pool based on amount of letters
		offScreenSprites = new Image[letters.Length];
		for (int i = 0; i < letters.Length; i++) {
			offScreenSprites [i] = Instantiate (offScreenSprite) as Image; 
			offScreenSprites [i].gameObject.SetActive (false);
			offScreenSprites [i].rectTransform.SetParent (transform);
		}
	}

	void Update () {
		for (int i = 0; i < letters.Length; i++) {
			Vector3 onScreenPosition = Camera.main.WorldToScreenPoint (letters[i].transform.position);

			// ON SCREEN
			if (onScreenPosition.z > 0 && (onScreenPosition.x > 0 && onScreenPosition.x < screenWidth) && (onScreenPosition.y > 0 && onScreenPosition.y < screenHeight)) {
				if (offScreenSprites[i].gameObject.activeInHierarchy)
					offScreenSprites[i].gameObject.SetActive (false);
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


				if (!offScreenSprites[i].gameObject.activeInHierarchy)
					offScreenSprites[i].gameObject.SetActive (true);
				offScreenSprites[i].rectTransform.position = new Vector3 (x, y, 0);
			}
		}
	}
	#endregion MONO
}﻿
