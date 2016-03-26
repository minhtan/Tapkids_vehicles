using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using PDollarGestureRecognizer;

namespace WordDraw
{
	public class LetterSpawner : MonoBehaviour
	{
		[SerializeField]
		private RectTransform _spawnPoint;
		[SerializeField]
		private GameObject _letterPrefab;
		[SerializeField]
		private float _spawnPeriod = 3f;
		[SerializeField]
		private int _maxStackCap = 9;

		private List<UILetter> _letterList;

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
			SpawnLetters ();
		}

		void OnGestureDetected (Result result)
		{
			Letters letter = WordDrawConfig.GetGestureResult (result);
			DestroyResultGesture (letter);
			Debug.Log (letter);
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
			StartCoroutine (SpawnCor (_spawnPeriod));
		}

		IEnumerator SpawnCor (float period)
		{
			while (true) {
				GameObject letter = Instantiate (_letterPrefab);
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
}
