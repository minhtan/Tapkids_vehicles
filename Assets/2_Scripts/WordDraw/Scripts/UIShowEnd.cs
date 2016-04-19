using UnityEngine;
using System.Collections;

namespace WordDraw
{
	public class UIShowEnd : MonoBehaviour
	{
		private GameObject[] _childGOs;

		void OnEnable ()
		{
			LetterSpawner.OnGameOver += OnGameOver;
		}

		void OnDisable()
		{
			LetterSpawner.OnGameOver += OnGameOver;
		}

		void Start()
		{
			_childGOs = new GameObject[transform.childCount];
			for(int i = 0; i < _childGOs.Length; i++)
			{
				_childGOs [i] = transform.GetChild (i).gameObject;
			}
		}


		private void OnGameOver()
		{
			for(int i = 0; i < _childGOs.Length; i++)
			{
				_childGOs [i].SetActive (true);
			}
		}
	}
}
