using UnityEngine;
using System.Collections;

public class UIColorButton : MonoBehaviour {

	public int matId;

	void OnEnable () {
		Messenger.AddListener <int> (EventManager.GUI.CHANGE_MATERIAL.ToString (), HandleChangeMaterial);
	}

	void Disable () {
		Messenger.RemoveListener <int> (EventManager.GUI.CHANGE_MATERIAL.ToString (), HandleChangeMaterial);
	}

	void HandleChangeMaterial (int _id) {
		if (matId == _id) {

		}
	}
}
