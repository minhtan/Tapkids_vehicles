using UnityEngine;
using System.Collections.Generic;
using System;
using System.Text;
using PDollarGestureRecognizer;

namespace WordDraw
{

	public enum Letters
	{
		A,
		B,
		C,
		D,
		E,
		F,
		G,
		H,
		I,
		J,
		K,
		L,
		M,
		N,
		O,
		P,
		Q,
		R,
		S,
		T,
		U,
		V,
		W,
		X,
		Y,
		Z,
		NULL
	}

	public class WordDrawConfig
	{
		private static string[] _letterEnumNames;

		public static string[] LetterEnumNames {
			get {
				if (_letterEnumNames == null) {
					_letterEnumNames = Enum.GetNames (typeof(Letters));
				}

				return _letterEnumNames;
			}
		}

		private static Letters[] _letterEnum;

		public static Letters[] LetterEnum {
			get {
				if (_letterEnum == null)
					_letterEnum = Enum.GetValues (typeof(Letters)) as Letters[];

				return _letterEnum;
			}
		}

		public static int LetterNumber{ get { return LetterEnumNames.Length; } }

		/// <summary>
		/// Gets the gesture result.
		/// </summary>
		/// <returns>The gesture result.</returns>
		/// <param name="result">Result.</param>
		public static Letters GetGestureResult (Result result)
		{
			string name = result.GestureClass[0] + "";
			byte[] asciiBytes = Encoding.ASCII.GetBytes (name);
			int index = asciiBytes [0] - 65;
			return (index >= 0 && index <= 26) ? LetterEnum [index] : LetterEnum[LetterNumber - 1];
		}

		/// <summary>
		/// Gets the name of the letter from.
		/// </summary>
		/// <returns>The letter from name.</returns>
		/// <param name="letterName">Letter name.</param>
		public static Letters GetLetterFromName(string letterName)
		{
			letterName = letterName.ToUpper ();
			for(int i = 0; i <LetterEnumNames.Length; i++ )
			{
				if (LetterEnumNames [i] == letterName)
					return LetterEnum [i];
			}

			return Letters.NULL;
		}

		/// <summary>
		/// Compares the letter with result.
		/// </summary>
		/// <returns><c>true</c>, if letter with result was compared, <c>false</c> otherwise.</returns>
		/// <param name="letterBut">Letter but.</param>
		/// <param name="detectedResult">Detected result.</param>
		public static bool CompareLetterWithResult(UILetterButton letterBut, Result detectedResult)
		{
			Letters resultLetter = GetGestureResult (detectedResult);

			if (letterBut.Letter == resultLetter)
				return true;

			return false;
		}

		/// <summary>
		/// Gets the non duplicate.
		/// </summary>
		/// <returns>The non duplicate.</returns>
		/// <param name="source">Source.</param>
		/// <param name="des">DES.</param>
		public static List<UILetter> GetNonDuplicate(List<UILetter> source, List<UILetter> des)
		{
			des.Clear ();
			for(int i = 0; i < source.Count; i++)
			{
				if (des.Count == 0) {
					des.Add (source [i]);
					continue;
				}

				for(int k = 0; k < des.Count; k++)
				{
					if (source [i].Letter == des [k].Letter)
						break;
					else
					{
						if (k == des.Count - 1)
							des.Add (source[i]);
					}
				}
			}

			return des;
		}
	}
}
