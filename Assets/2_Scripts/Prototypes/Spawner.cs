using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Spawner : MonoBehaviour {

	// spawn time
	float maxSpawnTime = 3f;
	float minSpawnTime = 1f;

	// spawn rate
	float maxSpawnRate = 3f;
	float minSpawnRate = 1f;


	float time;
	// object type
	public GameObject[] obstacles;
	public GameObject[] letters;
	public Transform[] checkPoints;

	//

	// Use this for initialization
	void Start () {
		for (int i = 0; i < checkPoints.Length; i++) {
			TrashMan.spawn (letters[Random.Range (0, letters.Length)], checkPoints[Random.Range(0, checkPoints.Length)].position, Quaternion.identity);
		}
	}

//	void SpawnLetter () {
//		TrashMan.spawn (letters[Random.Range (0, letters.Length)], checkPoints[Random.Range(0, checkPoints.Length)], 0, Random.Range(-spawnArea, spawnArea)), Quaternion.identity);
//	}


}
