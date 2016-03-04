using UnityEngine;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using System;

namespace WordList
{
	public abstract class WordListWriter
	{
	
		public abstract void WriteWordList (List<string> resultList, string letterGroup, string fileName, string directoryPath);

		protected void WriteTextFile (string data, string fileName, string directoryPath)
		{
			string assetPath = directoryPath + "/" + fileName + ".txt";
			var sr = File.CreateText (assetPath);
			sr.Write (data);
		
			AssetDatabase.Refresh();

			int index = assetPath.IndexOf("Assets");
			string relativePath = assetPath.Substring(index, assetPath.Length - index);
			TextAsset txtGO = AssetDatabase.LoadAssetAtPath<TextAsset>(relativePath);
			EditorGUIUtility.PingObject(txtGO);

			sr.Close ();
		}
	}
}
