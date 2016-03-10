using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using HutongGames.PlayMaker;
public class Spawner : MonoBehaviour {

//	// spawn time
//	float maxSpawnTime = 3f;
//	float minSpawnTime = 1f;
//
//	// spawn rate
//	float maxSpawnRate = 3f;
//	float minSpawnRate = 1f;

//	string letters;
	float time;
	string letter;
	// object type
//	public GameObject[] obstacles;
	public GameObject[] letterPrefabs;
	public Transform[] checkPoints;
	//

	List<GameObject> letterGos;

	void OnEnable () {
		CarGameEventController.InitGame += OnInitGame;
		CarGameEventController.GameOver += OnGameOver;
		CarGameEventController.ResetGame += OnValidateWord;
		CarGameEventController.ValidateWord += OnValidateWord;
	}
	void OnDisable () {
		CarGameEventController.InitGame -= OnInitGame;
		CarGameEventController.GameOver -= OnGameOver;
		CarGameEventController.ResetGame -= OnValidateWord;
		CarGameEventController.ValidateWord -= OnValidateWord;
	}

	void Start () {
		letterGos = new List<GameObject> ();
	}

	void OnInitGame (string _letter) {
		letter = _letter;
		for (int i = 0; i < letter.Length; i++) {
			for (int j = 0; j < letterPrefabs.Length; j++) {
				if (letter[i].ToString ().Equals (letterPrefabs[j].name)) {
//					TrashMan.spawn (letterPrefabs[j], checkPoints [i].position, Quaternion.identity);
					GameObject go = (GameObject) Instantiate (letterPrefabs [j], checkPoints [i].position, Quaternion.identity);
					letterGos.Add (go);
				}
			}
		}
	}

	void OnGameOver () {

	}

	void OnValidateWord () {

//		if(_isCorrect) return;

		// clear current letter
		if(letterGos.Count > 0) {
			for (int i = 0; i < letterGos.Count; i++) {
				if(letterGos[i] != null) {
					Destroy (letterGos[i]);
				}
			}
		}
		letterGos.Clear ();
		// re spawn

		for (int i = 0; i < letter.Length; i++) {
			for (int j = 0; j < letterPrefabs.Length; j++) {
				if (letter[i].ToString ().Equals (letterPrefabs[j].name)) {
//					TrashMan.spawn (letterPrefabs[j], checkPoints [i].position, Quaternion.identity);
					GameObject go = (GameObject) Instantiate (letterPrefabs [j], checkPoints [i].position, Quaternion.identity);
					letterGos.Add (go);
				}
			}
		}
	}
}
