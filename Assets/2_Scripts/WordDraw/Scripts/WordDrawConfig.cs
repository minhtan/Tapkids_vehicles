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

		public static Letters GetGestureResult (Result result)
		{
			string name = result.GestureClass[0] + "";
			byte[] asciiBytes = Encoding.ASCII.GetBytes (name);
			int index = asciiBytes [0] - 65;
			return (index >= 0 && index <= 26) ? LetterEnum [index] : LetterEnum[LetterNumber - 1];
		}

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

		public static bool CompareLetterWithResult(UILetterButton letterBut, Result detectedResult)
		{
			Letters resultLetter = GetGestureResult (detectedResult);

			if (letterBut.Letter == resultLetter)
				return true;

			return false;
		}

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
