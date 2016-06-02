﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using PDollarGestureRecognizer;
using UnityEngine.Events;

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

		[SerializeField]
		private int _curDifficulty = 0;

		private List<UILetter> _letterList;
		private GameObject[] _letterPrefabs;
		private List<UILetter> _noDuplicateLetterList;

		public List<UILetter> CurrentInStackLetters {
			get {
				if (_noDuplicateLetterList == null) {
					_noDuplicateLetterList = new List<UILetter> (26);
					return _noDuplicateLetterList;
				} else {
					return WordDrawConfig.GetNonDuplicate (_letterList, _noDuplicateLetterList);
				}

				return null;
			}
		}

		public WordDrawDifficulty CurrentDifficulty { get { return _difficulties [_curDifficulty]; } }

		public static UnityAction OnCorrect;
		public static UnityAction<Letters> OnReturnRecognizedResult;
		public static UnityAction<int> OnReturnBonusCount;
		public static UnityAction OnNoMatchResult;

		void OnEnable ()
		{
			LeanGestureRecognizer.OnGestureDetected += OnGestureDetected;
			UICountDownText.OnEndCountDown += OnStartGame;
		}

		void OnDisable ()
		{
			LeanGestureRecognizer.OnGestureDetected -= OnGestureDetected;
			UICountDownText.OnEndCountDown -= OnStartGame;
		}

		public delegate void OnGameOverEvent ();

		public static event OnGameOverEvent OnGameOver;

		// Use this for initialization
		void Awake ()
		{
			_letterList = new List<UILetter> ();
			_letterPrefabs = Resources.LoadAll <GameObject> ("Letters");
		}

		private void OnStartGame ()
		{
			SpawnLetters ();	
		}

		void OnGestureDetected (Result result)
		{
			Letters letter = WordDrawConfig.GetGestureResult (result);
			DestroyResultGesture (letter);
		}

		private void DestroyResultGesture (Letters target)
		{
			List<UILetter> destroyedLetter = new List<UILetter> (_maxStackCap);
			UILetter letter;
			int correctCount = 0;

			for (int i = _letterList.Count - 1; i >= 0; i--) {
				letter = _letterList [i];
				if (target == letter.Letter) {
					correctCount++;
					destroyedLetter.Add (letter);
					_letterList.RemoveAt (i);
					letter.DestroyLetter (_curDifficulty);

					if (OnReturnRecognizedResult != null) {
						OnReturnRecognizedResult (letter.Letter);
					}
				}
			}

			if(correctCount == 1)
			{
				if (OnCorrect != null)
					OnCorrect ();
			}
			else if (correctCount > 1) {
				if (OnReturnBonusCount != null)
					OnReturnBonusCount (correctCount);
			} else if (correctCount == 0) {
				if (OnNoMatchResult != null)
					OnNoMatchResult ();
			}
		}

		private void SpawnLetters ()
		{
			StartCoroutine (SpawnCor ());
		}

		public void ChangeToMextDifficulty ()
		{
			if (_curDifficulty < _difficulties.Length - 1)
				_curDifficulty++;
		}

		private GameObject GetRandomLetterPrefab ()
		{
			return _letterPrefabs [Random.Range (0, _letterPrefabs.Length)];
		}

		IEnumerator SpawnCor ()
		{
			while (true) {

				GameObject letter = Instantiate (GetRandomLetterPrefab ());

				letter.transform.SetParent (_spawnPoint, false);
				 
				letter.GetComponent<Rigidbody2D> ().mass = CurrentDifficulty.FallingSpeed;
				_letterList.Add (letter.GetComponent<UILetter> ());

				if (_letterList.Count > _maxStackCap) {
					if (OnGameOver != null)
						OnGameOver ();
					yield break;
				}

				yield return new WaitForSeconds (CurrentDifficulty.SpawnPeriod);
			}
		}
	}

	/// <summary>
	/// Contain difficulty property
	/// </summary>
	[System.Serializable]
	public class WordDrawDifficulty : System.Object
	{
		[SerializeField]
		private float _spawnPeriod;

		public float SpawnPeriod { get { return _spawnPeriod; } }

		[SerializeField]
		private int _requiredScore;

		public int RequiredScore {
			get { return _requiredScore; }
		}

		[SerializeField]
		private float _fallingSpeed;

		public float FallingSpeed { get { return _fallingSpeed; } }
	}
}
