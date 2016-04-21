using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.Events;
using AssetBundles;
using UnityEngine.UI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

public class SceneController : UnitySingletonPersistent<SceneController>
{
	#region VAR

	public Text _text;

	public enum SceneID
	{
		INTRO,
		MENU,
		WORDGAME,
		CARGAME,
		WORDDRAWGAME
	}

	private Dictionary<SceneID, string> _sceneDict;

	#endregion

	public static UnityAction OnStartLoading;
	public static UnityAction OnEndLoading;
	public static UnityAction<float> OnLoadingScene;

	public override void Awake ()
	{
		base.Awake ();
		InitSceneGroup ();
	}

	public void ReloadCurrentScene ()
	{
		SceneManager.LoadScene (SceneManager.GetActiveScene ().buildIndex);
	}

	public void LoadingSceneAsync (SceneID id, float delay = 0f)
	{
		StartCoroutine (LoadingOperation (id, delay));
	}

	private IEnumerator LoadingOperation (SceneID id, float delay)
	{
		if (delay > 0f)
			yield return new WaitForSeconds (delay);

		if (OnStartLoading != null)
			OnStartLoading ();
		
		_text.text = "id " + id;

		AsyncOperation async = SceneManager.LoadSceneAsync ((int)id);

		async.allowSceneActivation = false;	

		float averagePercent = 0f;
		while (true) {
			if (OnLoadingScene != null)
				OnLoadingScene (averagePercent);	
			
			_text.text = async.progress + " " + AssetBundleManager.ReturnProgress ();

			if (AssetBundleManager.IsInprogress ()) {
				averagePercent = (async.progress + AssetBundleManager.ReturnProgress ()) / 2;
			} else {
				averagePercent = (async.progress + 1f) / 2;
			}

			if (averagePercent == 0.95f) {
				if (OnEndLoading != null)
					OnEndLoading ();
				async.allowSceneActivation = true;
				yield break;
			}
			
			yield return null;
		}
	}

	#region PRIVATE METHOD

	private void InitSceneGroup ()
	{
		/*string scenePath = "1_Scenes/";

		TextAsset[] sceneArray = Resources.LoadAll<TextAsset>(scenePath);
		Debug.Log (sceneArray.Length);

		for(int i = 0; i < sceneArray.Length; i++)
		{
			Debug.Log (sceneArray[i].name);
		}

		string[] sceneFiles = new string[sceneArray.Length];
			
		string[] sceneFileNames = new string[sceneFiles.Length];
		string[] separator = { "." };

		for(int i = 0; i < sceneFileNames.Length; i++)
		{
			sceneFileNames[i] = Path.GetFileName (sceneFiles[i]).Split(separator, 0)[0];
		}

		for(int i = 0; i < sceneArray.Length; i++)
		{
			sceneFileNames [i] = sceneArray [i].name;
			Debug.Log (sceneFileNames[i]);
		}

		_sceneDict = new Dictionary<SceneID, string> ();

		string[] sceneEnumNames = Enum.GetNames (typeof(SceneID));
		Array sceneids = Enum.GetValues (typeof(SceneID));

		int length = sceneFiles.Length;
		string tmpSceneName = null;

		for (int i = 0; i < length; i++) {
			tmpSceneName = sceneFileNames [i];
		
			for (int k = 0; k < sceneEnumNames.Length; k++) {

				if (tmpSceneName.ToUpper ().Contains (sceneEnumNames [k])) {
					_sceneDict.Add ((SceneID)sceneids.GetValue (k), tmpSceneName);
					break;
				}
			}
		}*/
	}

	#endregion
}
