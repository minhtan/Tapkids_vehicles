using UnityEngine;
using System.Collections.Generic;
using System.IO;

namespace WordList
{
	public abstract class WordListWriter
	{
	
		public abstract void WriteWordList (List<string> resultList, string letterGroup, string fileName, string directoryPath);

		protected void WriteTextFile (string data, string fileName, string directoryPath)
		{
			var sr = File.CreateText (directoryPath + "/" + fileName + ".txt");
			sr.Write (data);
			sr.Close ();
		}
	}
}
