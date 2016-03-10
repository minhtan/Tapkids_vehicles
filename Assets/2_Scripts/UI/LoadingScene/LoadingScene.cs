using UnityEngine;
using System.Collections;

public class LoadingScene : MonoBehaviour {

	// Use this for initialization
	public void StartLoading () {
		SceneController.Instance.LoadingSceneAsync (SceneController.SceneID.MENU);
	}

	void OnEnable()
	{
		SceneController.OnStartLoading += OnStartLoading;
		SceneController.OnEndLoading += OnEndLoading;
	}

	void OnDisable()
	{
		SceneController.OnStartLoading -= OnStartLoading;
		SceneController.OnEndLoading -= OnEndLoading;
	}

	private void OnStartLoading()
	{
		foreach(Transform trans in transform)
		{
			trans.gameObject.SetActive (true);
		}
	}

	private void OnEndLoading()	
	{
		foreach(Transform trans in transform)
		{
			trans.gameObject.SetActive (false);
		}
	}
}
