using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class SceneHandler : MonoBehaviour
{
	#region SINGLETON

	private static SceneHandler instance = null;

	public static SceneHandler Instance {
		get {
			return instance;
		}
	}

	#endregion

	#region VAR

	[SerializeField]
	private SceneContainer[] _sceneGroup;

	#endregion

	void Awake ()
	{
		if (instance != null && instance != this)
			Destroy (gameObject);

		instance = this;
		DontDestroyOnLoad (gameObject);

		InitSceneGroup ();
	}

	private void InitSceneGroup ()
	{
		_sceneGroup = new SceneContainer[SceneManager.sceneCount];
		for (int i = 0; i < SceneManager.sceneCount; i++) {
			Scene scene = SceneManager.GetSceneAt (i);
			SceneContainer container = new SceneContainer ();
			container.SceneID = scene.buildIndex;
			container.SceneName = scene.name;
			_sceneGroup [i] = container;
		}
	}

	public void LoadingSceneAsync ()
	{
		
	}

	[System.Serializable]
	public class SceneContainer : System.Object
	{
		[SerializeField]
		private int _sceneID;

		public int SceneID{ set { _sceneID = value; } get { return _sceneID; } }

		[SerializeField]
		private string _name;

		public string SceneName{ set { _name = value; } get { return _name; } }
	}
}
