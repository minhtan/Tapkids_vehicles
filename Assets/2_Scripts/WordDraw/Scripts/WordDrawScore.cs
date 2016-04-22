﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace WordDraw
{

	public class WordDrawScore : MonoBehaviour
	{
		public Text _scoreText;
		public int _baseScore = 1;
		public int _bonusPerX2Combo;

		private int _currentSessionScore = 0;

		public int CurrentScore { get { return _currentSessionScore; } }

		void OnEnable ()
		{
			LetterSpawner.OnReturnRecognizedResult += OnReturnRecognizedResult;
			LetterSpawner.OnReturnBonusCount += BonusCount;
		}

		void OnDisable ()
		{
			LetterSpawner.OnReturnRecognizedResult -= OnReturnRecognizedResult;
			LetterSpawner.OnReturnBonusCount -= BonusCount;
		}

		private void OnReturnRecognizedResult (Letters correctLetter)
		{
			AddScore ();
		}

		private void AddScore ()
		{
			_currentSessionScore += _baseScore;
		}

		private void BonusCount (int count)
		{
			_currentSessionScore += count * _bonusPerX2Combo;

			_scoreText.text = _currentSessionScore.ToString ();
		}
	}
}
