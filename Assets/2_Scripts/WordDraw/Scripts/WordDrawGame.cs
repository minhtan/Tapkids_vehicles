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

		private void OnGestureReset ()
		{
			_drawer.ResetStroke ();
		}

		private void OnGameOver()
		{
			_statusText.text = "GAMEOVER";
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
