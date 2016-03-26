using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

public class SceneController : UnitySingletonPersistent<SceneController>
{
	#region VAR

	[SerializeField]
	private SceneContainer[] _sceneGroup;

	public enum SceneID{
		INTRO = 0, 
		AR = 1,
		MENU = 2, 
		WORDGAME = 3,
		CARGAME = 4
	}
	#endregion

	public static UnityAction OnStartLoading;
	public static UnityAction OnEndLoading;
	public static UnityAction<float> OnLoadingScene;

	public override void Awake ()
	{
		base.Awake ();
		InitSceneGroup ();
	}

	public void ReloadCurrentScene()
	{
		SceneManager.LoadScene (SceneManager.GetActiveScene().buildIndex);
	}

	public void LoadingSceneAsync (SceneID id)
	{
		StartCoroutine (LoadingOperation(id, 0f));
	}

	private IEnumerator LoadingOperation(SceneID id, float delay = 0f){
		if (delay > 0f)
			yield return new WaitForSeconds (delay);

		if (OnStartLoading != null)
			OnStartLoading ();
		
		AsyncOperation async = SceneManager.LoadSceneAsync ((int)id);
	
		async.allowSceneActivation = false;
		
		while(!async.isDone)
		{
			if (OnLoadingScene != null)
				OnLoadingScene (async.progress);	

			if (async.progress == 0.9f) {
				if(OnEndLoading != null)
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
		_sceneGroup = new SceneContainer[SceneManager.sceneCount];
		for (int i = 0; i < SceneManager.sceneCount; i++) {
			SceneContainer container = new SceneContainer ();

			container.SceneData = SceneManager.GetSceneAt (i);
			_sceneGroup [i] = container;
		}
	}

	private Scene GetSceneByID(SceneID id)
	{
		for(int i = 0; i< _sceneGroup.Length; i++)
		{
			if (id == _sceneGroup [i].ID)
				return _sceneGroup [i].SceneData;
		}
		return _sceneGroup[0].SceneData;
	}
	#endregion

	[System.Serializable]
	public class SceneContainer : System.Object
	{
		[SerializeField]
		private SceneID _sceneID;

		public SceneID ID{ set { _sceneID = value; } get { return _sceneID; } }

		[SerializeField]
		private Scene _scene;

		public Scene SceneData{ set { _scene = value; } get { return _scene; } }
	}
}
