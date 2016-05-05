using UnityEngine;
using System.Collections;
using WordDraw;

public class DestroyOnLoad : MonoBehaviour {

	void OnEnable()
	{
		Messenger.AddListener(EventManager.GameState.EXIT_TO_MENU.ToString(), DestroyGO);
		Messenger.AddListener(EventManager.GameState.RESET.ToString(), DestroyGO);
	}

	public void DestroyGO() {
		Destroy(GameObject.Find ("VectorCanvas"));
	}
}
