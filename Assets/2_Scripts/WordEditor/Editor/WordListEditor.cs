using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(WordListHandler))]
public class WordListEditor : Editor {

    private WordListHandler _wordListHandler;

    void OnEnable()
    {
        _wordListHandler = (WordListHandler)target;
    }

    public override void OnInspectorGUI()
    {
        GUILayout.Space(10f);
        serializedObject.Update();
        GUILayout.Label("WORD LIST CONFIGURATION");
        GUILayout.Space(5f);

        DrawProperty("_inputWordFileName", 5f);

        GUILayout.Space(5f);
        if(GUILayout.Button("Read Word List"))
        {
            _wordListHandler.ReadWordList();
        }

        DrawArrayProperty("_letterGroups", 5f);
        DrawProperty("_filterMode", 5f);

        // Export Menu
        GUILayout.Space(10f);
        GUILayout.Label("SAVE OPTION");
        GUILayout.Space(5f);
        SerializedProperty directoryPath = serializedObject.FindProperty("_directoryPath");
        if (string.IsNullOrEmpty(directoryPath.stringValue))
        {
            EditorGUILayout.HelpBox("Please select a directory where the level will be saved.", MessageType.Error, true);
        }
        else
        {
            if (!System.IO.Directory.Exists(directoryPath.stringValue))
            {
                directoryPath.stringValue = string.Empty;
            }
        }

        if (GUILayout.Button(!string.IsNullOrEmpty(directoryPath.stringValue) ? (directoryPath.stringValue.Contains("Assets") ? directoryPath.stringValue.Substring(directoryPath.stringValue.IndexOf("Assets")) : directoryPath.stringValue) : "Select directory", "PreDropDown"))
        {
            string path = EditorUtility.OpenFolderPanel("Select directory, where the level will be saved.", "", "");
            if (!string.IsNullOrEmpty(path))
            {
                directoryPath.stringValue = path;
            }
        }

        DrawProperty("_outputFileName", 5f);


        GUILayout.Space(10f);
        if (GUILayout.Button("Generate Word List"))
        {
            _wordListHandler.CreateWordList();
        }

		GUILayout.Space(10f);
		if (GUILayout.Button("Generate Group Length Word List"))
		{
			_wordListHandler.CreateGroupWordList();
		}

        serializedObject.ApplyModifiedProperties();
        bool guiEnabled = GUI.enabled;

        GUI.enabled = guiEnabled;

        if (GUI.changed)
            EditorUtility.SetDirty(_wordListHandler);
    }

    private SerializedProperty DrawArrayProperty(string propertyName, float space)
    {
        GUILayout.Space(space);
        return DrawProperty(propertyName, true, true);
    }

    private SerializedProperty DrawProperty(string propertyName, float space, bool isArray = false)
    {
        GUILayout.Space(space);
        return DrawProperty(propertyName, false, isArray);
    }

    private SerializedProperty DrawProperty(string propertyName, bool isArray = false)
    {
        return DrawProperty(propertyName, false, isArray);
    }

    private SerializedProperty DrawProperty(string propertyName, bool insert, bool isArray = false)
    {
        SerializedProperty property = serializedObject.FindProperty(propertyName);
        if (insert)
        {
            EditorGUI.indentLevel += 1;
        }
        EditorGUILayout.PropertyField(property, isArray);

        if (insert)
        {
            EditorGUI.indentLevel -= 1;
        }
        return property;
    }
}
