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

		private const string TEXT_CAREFULLY = "More Carefully";
		private const string TEXT_GOOD_JOB = "Good Job";
		private const string TEXT_COMBO = "Combo X";
		private const string TEXT_GAME_OVER = "Game Over";

		void OnEnable ()
		{
			LeanGestureRecognizer.OnGestureReset += OnGestureReset;
			LetterSpawner.OnGameOver += OnGameOver;
			LetterSpawner.OnNoMatchResult += OnNoMatch;
			LetterSpawner.OnCorrect += OnCorrect;
			LetterSpawner.OnReturnBonusCount += OnBonus;
			LeanGestureRecognizer.OnGestureDetected += OnResult;
			Messenger.AddListener<bool> (EventManager.GameState.PAUSE.ToString(), Pause);
			UICountDownText.OnEndCountDown += OnStartGame;
		}

		void OnDisable ()
		{
			LeanGestureRecognizer.OnGestureReset -= OnGestureReset;
			LetterSpawner.OnGameOver -= OnGameOver;
			LetterSpawner.OnNoMatchResult -= OnNoMatch;
			LetterSpawner.OnCorrect -= OnCorrect;
			LetterSpawner.OnReturnBonusCount -= OnBonus;
			LeanGestureRecognizer.OnGestureDetected -= OnResult;
			UICountDownText.OnEndCountDown -= OnStartGame;
		}

		void OnStartGame()
		{
			AudioManager.Instance.PlayAudio (AudioKey.UNIQUE_KEY.CORRECT_WORD);
			Messenger.Broadcast<bool> (EventManager.GUI.TOGGLE_MENU_BTN.ToString(), true);
			Messenger.Broadcast <bool> (EventManager.GUI.TOGGLE_PLAYER_PNL.ToString (), false);
			StartCoroutine (GameCoroutine());
		}

		private void Pause(bool isPaused)
		{
			if (isPaused)
				Time.timeScale = 0;
			else
				Time.timeScale = 1;
		}

		public void Exit()
		{
			Messenger.Broadcast(EventManager.GameState.EXIT_TO_MENU.ToString());
			SceneController.Instance.LoadingSceneAsync (SceneController.SceneID.MENU);
		}

		public void Restart ()
		{
			_drawer.ResetStroke ();
			Messenger.Broadcast(EventManager.GameState.RESET.ToString());
			SceneController.Instance.ReloadCurrentScene ();
		}

		private void OnCorrect()
		{
			_statusText.text = TEXT_GOOD_JOB;
			StartCoroutine (ResetStatusTextCor());
		}

		private void OnBonus(int count)
		{
			_statusText.text = TEXT_COMBO + count;
			StartCoroutine (ResetStatusTextCor());
		}

		private void OnNoMatch()
		{
			_statusText.text = TEXT_CAREFULLY;
			StartCoroutine (ResetStatusTextCor());
		}

		private IEnumerator ResetStatusTextCor()
		{
			yield return new WaitForSeconds (2f);
			_statusText.text = "";
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
			_statusText.text = TEXT_GAME_OVER;
			_drawer.enabled = false;
			_wordDrawScore.enabled = false;
		}

		private void OnResult(Result result)
		{
			_resultText.text = result.GestureClass;
		}
	}
}
