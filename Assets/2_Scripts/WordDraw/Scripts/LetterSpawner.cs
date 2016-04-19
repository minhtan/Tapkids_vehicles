using UnityEngine;
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

		private List<UILetter> _letterList;
		private GameObject[] _letterPrefabs;
			
		[SerializeField]
		private int _curDifficulty = 0;

		public WordDrawDifficulty CurrentDifficulty { get { return _difficulties [_curDifficulty]; } }

		public static UnityAction<Letters> OnReturnRecognizedResult;
		public static UnityAction<int> OnReturnBonusCount;
		public static UnityAction OnNoMatchResult;

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
			List<UILetter> destroyedLetter = new List<UILetter> (_maxStackCap);
			UILetter letter;
			int correctCount = 0;

			for (int i = _letterList.Count - 1; i >= 0; i--) {
				letter = _letterList [i];
				if (target == letter.Letter) {
					correctCount++;
					destroyedLetter.Add (letter);
					_letterList.RemoveAt (i);
					Destroy (letter.gameObject);

					if (OnReturnRecognizedResult != null) {
						OnReturnRecognizedResult (letter.Letter);
					}
				}
			}


			if (OnReturnBonusCount != null)
				OnReturnBonusCount (correctCount);

			if (correctCount > 0)
				return;

			if (OnNoMatchResult != null)
				OnNoMatchResult ();
		}

		private void SpawnLetters ()
		{
			StartCoroutine (SpawnCor ());
		}

		public void ChangeToMextDifficulty ()
		{
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
