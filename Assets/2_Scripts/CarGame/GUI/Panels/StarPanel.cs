using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class StarPanel : MonoBehaviour {

	public Image[] stars;

	void OnEnable () {
//		Messenger.AddListener <int> (EventManager.GameState.GAMEOVER.ToString (), HandleGameOver);
		// TODO: subscribe event to enable star
	}

	void OnDisable () {
//		Messenger.RemoveListener <int> (EventManager.GameState.GAMEOVER.ToString (), HandleGameOver);
	}

	void Start () {
		for (int i = 0; i < stars.Length; i++) {
			stars[i].enabled = false;
		}
	}

	void HandleGameOver (int _amount) {
		int i = 0; 
		while (i < _amount) {
			stars[i].enabled = true;
			i++;
		}
	}
}
