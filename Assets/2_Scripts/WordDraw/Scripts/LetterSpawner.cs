﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using PDollarGestureRecognizer;

namespace WordDraw
{
	public class LetterSpawner : MonoBehaviour
	{
		[SerializeField]
		private WordDrawDifficulty[] _difficulties; 

		[SerializeField]
		private RectTransform _spawnPoint;
	
		[SerializeField]
		private int _maxStackCap = 9;

		private List<UILetter> _letterList;
		private GameObject[] _letterPrefabs;
		private int _curDifficulty = 0;

		public WordDrawDifficulty CurrentDifficulty { get { return _difficulties [_curDifficulty]; } }

		void OnEnable ()
		{
			LeanGestureRecognizer.OnGestureDetected += OnGestureDetected;
		}

		void OnDisable ()
		{
			LeanGestureRecognizer.OnGestureDetected -= OnGestureDetected;
		}

		public delegate void OnGameOverEvent ();

		public static event OnGameOverEvent OnGameOver;

		// Use this for initialization
		void Awake ()
		{
			_letterList = new List<UILetter> ();
			_letterPrefabs = Resources.LoadAll <GameObject> ("Letters");
			SpawnLetters ();
		}

		void OnGestureDetected (Result result)
		{
			Letters letter = WordDrawConfig.GetGestureResult (result);
			DestroyResultGesture (letter);
		}

		private void DestroyResultGesture (Letters target)
		{
			for (int i = _letterList.Count - 1; i >= 0; i--) {
				if (target == _letterList [i].Letter) {
					Destroy (_letterList [i].gameObject);
					_letterList.RemoveAt (i);
				}
			}
		}

		private void SpawnLetters ()
		{
			StartCoroutine (SpawnCor (CurrentDifficulty.SpawnPeriod));
		}

		private void ChangeToMextDifficulty ()
		{
			_curDifficulty++;
		}

		private GameObject GetRandomLetterPrefab ()
		{
			return _letterPrefabs [Random.Range (0, _letterPrefabs.Length)];
		}

		IEnumerator SpawnCor (float period)
		{
			while (true) {
				GameObject letter = Instantiate (GetRandomLetterPrefab ());
				letter.transform.SetParent (_spawnPoint, false);

				_letterList.Add (letter.GetComponent<UILetter> ());

				if (_letterList.Count > _maxStackCap) {
					if (OnGameOver != null)
						OnGameOver ();
					yield break;
				}

				yield return new WaitForSeconds (period);
			}
		}
	}

	[System.Serializable]
	public class WordDrawDifficulty : System.Object
	{
		[SerializeField]
		private float _spawnPeriod;

		public float SpawnPeriod { get { return _spawnPeriod; } }
	}
}
