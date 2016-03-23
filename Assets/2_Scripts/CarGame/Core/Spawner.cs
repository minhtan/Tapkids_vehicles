using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using HutongGames.PlayMaker;
public class Spawner : MonoBehaviour {

	// techniques: 
	// - asset bundle 
	// - object pool 
	// handles:
	// - remove old letters then genrate new letters at init game state
	// - re generate missing letters after player gather word, whether true or false

//	float time;
	// object type
	public GameObject[] letterPrefabs; 	// Todo: should replace this prefab hard references with loading assetbundle method
	private GameObject[] wordPoints;
	//

	private Dictionary <int, GameObject> IndexToLetterDictionary = new Dictionary<int, GameObject> ();
	List<GameObject> letterGos;

	void OnEnable () {
		CarGameEventController.InitGame += OnInitGame;
		CarGameEventController.ValidateWord += OnValidateWord;
	}
	void OnDisable () {
		CarGameEventController.InitGame -= OnInitGame;
		CarGameEventController.ValidateWord -= OnValidateWord;
	}

	void Start () {
		letterGos = new List<GameObject> ();
		wordPoints = GameObject.FindGameObjectsWithTag ("WordPoint");
	}

	void OnInitGame (string _letter) {
		for (int i = 0; i < _letter.Length; i++) {
			for (int j = 0; j < letterPrefabs.Length; j++) {
				if (_letter[i].ToString ().Equals (letterPrefabs[j].name)) {
					GameObject go = (GameObject) Instantiate (letterPrefabs [j], wordPoints [i].transform.position, Quaternion.identity);
					letterGos.Add (go);
				}
			}
		}
	}

	void OnValidateWord () {
		
	}
}
