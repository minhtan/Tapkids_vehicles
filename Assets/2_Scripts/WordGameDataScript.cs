using UnityEngine;
using System.Collections.Generic;
using UnityEditor;

public class WordGameData : ScriptableObject
{
    //Both are case sensitive
    public string[] letters;
    public string[] answers;
}

public class WordGameDataList : ScriptableObject
{
    public List<WordGameData> list;
}

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

    [MenuItem("Assets/Create/WordGame Data %#d")]
    public static WordGameData CreateData()
    {
        WordGameData asset = ScriptableObject.CreateInstance<WordGameData>();

        AssetDatabase.CreateAsset(asset, AssetDatabase.GenerateUniqueAssetPath("Assets/4_Data/WordGameData.asset"));
        AssetDatabase.SaveAssets();

        EditorUtility.FocusProjectWindow();
        Selection.activeObject = asset;

        return asset;
    }
}