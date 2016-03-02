using UnityEngine;
using System.Collections.Generic;
using System;
#if UNITY_EDITOR
using UnityEditor;
#endif

[Serializable]
public class WordGameData
{
    //Both are case sensitive
    public string letters;
    public string[] wordlist;
}

[Serializable]
public class WordGameDataList : ScriptableObject
{
    public List<WordGameData> list;
}

#if UNITY_EDITOR
public class CreateWordGameDataList
{
    [MenuItem("Assets/Create/WordGame Data List")]
    public static WordGameDataList CreateList()
    {
        WordGameDataList asset = ScriptableObject.CreateInstance<WordGameDataList>();

        AssetDatabase.CreateAsset(asset, AssetDatabase.GenerateUniqueAssetPath("Assets/4_Data/WordGameDataList.asset"));
        AssetDatabase.SaveAssets();

        EditorUtility.FocusProjectWindow();
        Selection.activeObject = asset;

        return asset;
    }
}
#endif