using UnityEngine;
using System.Collections.Generic;
using UnityEditor;
using System.IO;
using WordList;

namespace WordList
{
	public class WordListEditorWindow : EditorWindow
	{

		#region VAR
		[SerializeField]
		private string
			_directoryPath;

		[SerializeField]
		private string[]
			_letterGroups;

		[SerializeField]
		private TextAsset
			_textAsset;

		public enum FilterMode
		{
			REPEATED,
			NO_REPEATED
		}
	
		[SerializeField]
		private FilterMode
			_filterMode = FilterMode.REPEATED;

		public enum OutputFormat
		{
			NO_GROUP,
			GROUP_BY_LENGTH,
		}

		[SerializeField]
		private OutputFormat
			_outputFormat = OutputFormat.NO_GROUP;
		private List<string> _dict;
		private SerializedObject _serializedObject;
		private WordListReader _reader;
		private WordListWriter _writer;
		#endregion

	#region EDITOR_CODE
		[MenuItem("Window/Wordlist Editor")]
		public static void ShowWindow ()
		{
			EditorWindow.GetWindow(typeof(WordListEditorWindow), true, "WordList Editor", true);
		}

		void OnEnable ()
		{
			_serializedObject = EditorHelper.GetSerializedObjectEditor (this);
		}

		void OnGUI ()
		{

			GUILayout.Label ("INPUT CONFIGURATIONS", EditorStyles.boldLabel);

			EditorHelper.DrawProperty(_serializedObject, "_textAsset", Constant.GUI_LAYOUT_SPACE);

			EditorHelper.DrawArrayProperty (_serializedObject, "_letterGroups", Constant.GUI_LAYOUT_SPACE);
			EditorHelper.DrawProperty (_serializedObject, "_filterMode", Constant.GUI_LAYOUT_SPACE);

			GUILayout.Space(Constant.GUI_LAYOUT_SPACE * 2);
			GUILayout.Label ("OUTPUT CONFIGURATIONS", EditorStyles.boldLabel);

			GUILayout.Space(Constant.GUI_LAYOUT_SPACE);

			EditorHelper.DrawDirectoryPathSelector (_serializedObject, "_directoryPath");
			EditorHelper.DrawProperty (_serializedObject, "_outputFormat", Constant.GUI_LAYOUT_SPACE);

			GUILayout.Space(Constant.GUI_LAYOUT_SPACE);
			if (GUILayout.Button ("Generate list")) {
				GenerateWordList ();
			}

			
			EditorHelper.SaveWindowEditor (this, _serializedObject);
		}
	#endregion

	
	#region PRIVATE METHOD
		private void GenerateWordList ()
		{
			if (_outputFormat == OutputFormat.GROUP_BY_LENGTH)
				_writer = new WriterLengthFilter ();
			else
				_writer = new WriterNoFilter ();

			_reader = new ReaderDefault ();

			_dict = _reader.ReadWordList (_textAsset.name);

			for (int i = 0; i < _letterGroups.Length; i++) {
				List<string> result = FindWordWithLetters (_letterGroups [i]);

				if (_filterMode == FilterMode.NO_REPEATED)
					FilterRepeateLetter (result);

				_writer.WriteWordList (result, _letterGroups[i], _letterGroups [i], _directoryPath);
			}
		}

		private void FilterRepeateLetter (List<string> result)
		{

			for (int i = result.Count - 1; i >= 0; i--) {
				if (IsDuplicatedLetter (result [i]))
					result.RemoveAt (i);
			}
		}
	
		private bool IsDuplicatedLetter (string word)
		{
			for (int i = 0; i < word.Length - 1; i++) {
				for (int k = i + 1; k < word.Length; k++) {
					if (word [i] == word [k])
						return true;
				}
			}
		
			return false;
		}
	
		private List<string> FindWordWithLetters (string letters)
		{
			List<string> listWithWord = new List<string> ();
		
			foreach (string word in _dict) {
				if (IsContructedByLetters (word, letters)) {
					listWithWord.Add (word);
				}
			}
		
			return listWithWord;
		}
	
		private bool IsContructedByLetters (string word, string letters)
		{
			bool isSameAsGroup = false;
		
			for (int i = 0; i < word.Length; i++) {
				for (int k = 0; k < letters.Length; k++) {
					if (word [i] == letters [k]) {
						isSameAsGroup = true;
						break;
					} else
						isSameAsGroup = false;
				}
			
				if (!isSameAsGroup)
					return false;
			}
		
			return true;
		}
	#endregion
	}
}
