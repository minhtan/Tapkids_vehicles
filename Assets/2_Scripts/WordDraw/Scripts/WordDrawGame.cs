using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using PDollarGestureRecognizer;

namespace WordDraw
{
	public class WordDrawGame : MonoBehaviour
	{

		public GestureLineDrawing _drawer;
		public Text _statusText;
		public Text _resultText;
		public LetterSpawner _spawner;
		public WordDrawScore _wordDrawScore;

		void OnEnable ()
		{
			LeanGestureRecognizer.OnGestureReset += OnGestureReset;
			LetterSpawner.OnGameOver += OnGameOver;
			LeanGestureRecognizer.OnGestureDetected += OnResult;
		}

		void OnDisable ()
		{
			LeanGestureRecognizer.OnGestureReset -= OnGestureReset;
			LetterSpawner.OnGameOver -= OnGameOver;
			LeanGestureRecognizer.OnGestureDetected -= OnResult;
		}

		void Start()
		{
			StartCoroutine (GameCoroutine());
		}

		private IEnumerator GameCoroutine()
		{
			while(true)
			{
				if (_wordDrawScore.CurrentScore > _spawner.CurrentDifficulty.RequiredScore)
					_spawner.ChangeToMextDifficulty ();
				
				yield return null;
			}
		}

		private void OnGestureReset ()
		{
			_drawer.ResetStroke ();
		}

		private void OnGameOver()
		{
			_statusText.text = "GAMEOVER";
			_drawer.enabled = false;
			_wordDrawScore.enabled = false;
		}

		private void OnResult(Result result)
		{
			_resultText.text = result.GestureClass;
		}

		public void Restart ()
		{
			_drawer.ResetStroke ();
			SceneController.Instance.ReloadCurrentScene ();
		}
	}
}
