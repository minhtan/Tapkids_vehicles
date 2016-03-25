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

		private static string[] LetterEnumNames {
			get {
				if (_letterEnumNames == null) {
					_letterEnumNames = Enum.GetNames (typeof(Letters));
				}

				return _letterEnumNames;
			}
		}

		private static Letters[] _letterEnum;

		private static Letters[] LetterEnum {
			get {
				if (_letterEnum == null)
					_letterEnum = Enum.GetValues (typeof(Letters)) as Letters[];

				return _letterEnum;
			}
		}

		public static int LetterNumber{ get { return LetterEnumNames.Length; } }

		public static Letters GetGestureResult (Result result)
		{
			byte[] asciiBytes = Encoding.ASCII.GetBytes (result.GestureClass);
			int index = asciiBytes [0] - 65;
			return (index >= 0 && index <= 26) ? LetterEnum [index] : LetterEnum[LetterNumber - 1];
		}

	}
}
