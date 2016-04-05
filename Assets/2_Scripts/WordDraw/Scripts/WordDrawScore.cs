using UnityEngine;
using System.Collections;

namespace WordDraw
{

	public class WordDrawScore : MonoBehaviour
	{
		public int _bonusPerX2Combo;


		private int currentSessionScore = 0;
		private float _timer = 0f;

		void OnEnable ()
		{
			LetterSpawner.OnReturnRecognizedResult += OnReturnRecognizedResult;
		}

		void OnDisable ()
		{
			LetterSpawner.OnReturnRecognizedResult -= OnReturnRecognizedResult;
		}

		private void OnReturnRecognizedResult (bool isCorrect, Letters correctLetter, Letters inputLetter)
		{
			if(isCorrect)
			{
				AddScore ();
			}
			else
			{
				
			}
		}

		private void AddScore()
		{
			
		}
	}
}
