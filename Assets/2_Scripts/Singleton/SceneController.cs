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

	public enum SceneID
	{
		INTRO,
		MENU,
		WORDGAME,
		CARGAME,
		WORDDRAWGAME,
		CARGAME2,
		STARTUP
	}

	private Dictionary<SceneID, string> _sceneDict;

	#endregion

	public static UnityAction OnStartLoading;
	public static UnityAction OnEndLoading;
	public static UnityAction<float> OnLoadingScene;	
	public static UnityAction<SceneID, SceneID> OnSceneChange;

	private SceneID _currentScene;
	private SceneID _prevScene;

	public override void Awake ()
	{
		base.Awake ();
	}

	public void ReloadCurrentScene ()
	{
		SceneManager.LoadScene (SceneManager.GetActiveScene ().buildIndex);
	}

	public void LoadingSceneAsync (SceneID id, float delay = 0f)
	{
		if (id == SceneID.MENU)
		if (!NetworkManager.Instance.HasInternetAvailable ()) {
			GUIController.Instance.OpenDialog ("Internet connection problem!")
				.AddButton ("Retry", UIDialogButton.Anchor.BOTTOM_LEFT, delegate { 
					ReloadCurrentScene();
			})
				.AddButton ("Quit", UIDialogButton.Anchor.BOTTOM_RIGHT, delegate {
				Application.Quit ();
			});
			return;
		}

		StartCoroutine (LoadingOperation (id, delay));
	}

	private IEnumerator LoadingOperation (SceneID id, float delay)
	{
		if (delay > 0f)
			yield return new WaitForSeconds (delay);

		_prevScene = _currentScene;
		_currentScene = id;

		if (OnSceneChange != null)
			OnSceneChange (_prevScene, _currentScene);

		if (OnStartLoading != null)
			OnStartLoading ();
		
		AsyncOperation async = SceneManager.LoadSceneAsync ((int)id);

		async.allowSceneActivation = false;	

		float averagePercent = 0f;
		while (true) {
			if (OnLoadingScene != null)
				OnLoadingScene (averagePercent);	
			
			if (AssetBundleManager.IsInprogress ()) {
				averagePercent = (async.progress + AssetBundleManager.ReturnProgress ()) / 2;
			} else {
				averagePercent = (async.progress + 1f) / 2;
			} 

			if (averagePercent == 0.95f) {
				yield return new WaitForSeconds (2f);

				if (OnEndLoading != null)
					OnEndLoading ();
				
				async.allowSceneActivation = true;
				
				yield break;
			}
			
			yield return null;
		}
	}
}
