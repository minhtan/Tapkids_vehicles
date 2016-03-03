using UnityEngine;
using System.Collections;

namespace WordList
{
	public class ReaderDefault : WordListReader
	{

		#region implemented abstract members of WordListReader

		public override void StringData (string data, System.Collections.Generic.List<string> wordList)
		{
			if(data.Length > Constant.MIN_WORD_LENGTH)
				wordList.Add(data);
		}

		public override void NumberData (float data, System.Collections.Generic.List<string> wordList)
		{

		}

		public override void BoolData (bool data, System.Collections.Generic.List<string> wordList)
		{
	
		}

		public override void NullData (System.Collections.Generic.List<string> wordList)
		{
	
		}

		#endregion


	}
}
