using UnityEngine;
using System.Collections;
#if UNITY_EDITOR
using UnityEditor;

namespace WordList
{
	public class EditorHelper
	{

		public static SerializedObject GetSerializedObjectEditor (EditorWindow window)
		{
			return new SerializedObject (window);
		}

		public static void SaveWindowEditor (EditorWindow editor, SerializedObject serializedObject)
		{
			serializedObject.ApplyModifiedProperties ();
			bool guiEnabled = GUI.enabled;
		
			GUI.enabled = guiEnabled;
		
			if (GUI.changed)
				EditorUtility.SetDirty (editor);
		}

	#region DRAW_PROPERTIES
		public static SerializedProperty DrawArrayProperty (SerializedObject serializedObject, string propertyName, float space)
		{
			GUILayout.Space (space);
			return DrawProperty (serializedObject, propertyName, true, true);
		}
	
		public static SerializedProperty DrawProperty (SerializedObject serializedObject, string propertyName, float space, bool isArray = false)
		{
			GUILayout.Space (space);
			return DrawProperty (serializedObject, propertyName, false, isArray);
		}
	
		public static SerializedProperty DrawProperty (SerializedObject serializedObject, string propertyName, bool isArray = false)
		{
			return DrawProperty (serializedObject, propertyName, false, isArray);
		}
	
		public static SerializedProperty DrawProperty (SerializedObject serializedObject, string propertyName, bool insert, bool isArray = false)
		{
			SerializedProperty property = serializedObject.FindProperty (propertyName);
			if (insert) {
				EditorGUI.indentLevel += 1;
			}
			EditorGUILayout.PropertyField (property, isArray);
		
			if (insert) {
				EditorGUI.indentLevel -= 1;
			}
			return property;
		}

		public static void DrawDirectoryPathSelector (SerializedObject serializedObject, string propertyName, string title = "Select directory, where the file will be saved.", string helpBoxText = "Please select a directory where the file will be saved.")
		{
			SerializedProperty directoryPath = serializedObject.FindProperty (propertyName);
			if (string.IsNullOrEmpty (directoryPath.stringValue)) {
				EditorGUILayout.HelpBox (helpBoxText, MessageType.Error, true);
			} else {
				if (!System.IO.Directory.Exists (directoryPath.stringValue)) {
					directoryPath.stringValue = string.Empty;
				}
			}
		
			if (GUILayout.Button (!string.IsNullOrEmpty (directoryPath.stringValue) ? (directoryPath.stringValue.Contains ("Assets") ? directoryPath.stringValue.Substring (directoryPath.stringValue.IndexOf ("Assets")) : directoryPath.stringValue) : "Select directory", "PreDropDown")) {
				string path = EditorUtility.OpenFolderPanel (title, "", "");
				if (!string.IsNullOrEmpty (path)) {
					directoryPath.stringValue = path;
				}
			}
		}

		public static int GetEnum (SerializedObject serializedObject, string enumPropertyName)
		{
			return DrawProperty (serializedObject, enumPropertyName).enumValueIndex;
		}

	#endregion
	}
}
#endif