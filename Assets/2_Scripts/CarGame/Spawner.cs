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
	// object type
//	public GameObject[] obstacles;
	public GameObject[] letterPrefabs;
	public Transform[] checkPoints;
	//

	void OnEnable () {
		CarGameEventController.InitGame += OnInitGame;
		CarGameEventController.GameOver += OnGameOver;
	}
	void OnDisable () {
		CarGameEventController.InitGame -= OnInitGame;
		CarGameEventController.GameOver -= OnGameOver;
	}

	void Start () {
		
	}

	void OnInitGame (string letter) {
		for (int i = 0; i < letter.Length; i++) {
			for (int j = 0; j < letterPrefabs.Length; j++) {
				if (letter[i].ToString ().Equals (letterPrefabs[j].name)) {
					Debug.Log (letter [i].ToString ());
					TrashMan.spawn (letterPrefabs[j], checkPoints [Random.Range (0, checkPoints.Length)].position, Quaternion.identity);
				}
			}
		}
	}

	void OnGameOver () {

	}
//	void Init () {
//		Debug.Log (FsmVariables.GlobalVariables.GetFsmString ("givenWord").Value);
//	}

	void SpawnLetter () {
//		TrashMan.spawn (letters[Random.Range (0, letters.Length)], checkPoints[Random.Range(0, checkPoints.Length)], Quaternion.identity);
	}

}
