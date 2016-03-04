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

	string letters;
	float time;
	// object type
//	public GameObject[] obstacles;
	public GameObject[] letterPrefabs;
	public Transform[] checkPoints;

	//

	// Use this for initialization
	void Start () {
		
	}

	void Init () {
		Debug.Log (FsmVariables.GlobalVariables.GetFsmString ("givenWord").Value);
	}
	void SpawnLetter () {
//		TrashMan.spawn (letters[Random.Range (0, letters.Length)], checkPoints[Random.Range(0, checkPoints.Length)], Quaternion.identity);
	}

}
