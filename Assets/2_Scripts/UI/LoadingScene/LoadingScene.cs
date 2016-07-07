using UnityEngine;
using System.Collections;

public class LoadingScene : MonoBehaviour {
	public GameObject _loadingBar;
	public GameObject _background;
	public GameObject _logo;

	public float _delayHide = 0.5f;

	void OnEnable()
	{
		SceneController.OnStartLoading += OnStartLoading;
		SceneController.OnEndLoading += OnEndLoading;
		SceneController.OnSceneChange += OnSceneChange;
	}

	void OnDisable()
	{
		SceneController.OnStartLoading -= OnStartLoading;
		SceneController.OnEndLoading -= OnEndLoading;
		SceneController.OnSceneChange -= OnSceneChange;
	}

	private void OnSceneChange(SceneController.SceneID prev, SceneController.SceneID current)
	{
		if(prev == SceneController.SceneID.INTRO && current == SceneController.SceneID.MENU)
		{
			_logo.SetActive (true);
		}
		else
		{
			_logo.SetActive (false);
		}
	}

	private void OnStartLoading()
	{
		_loadingBar.SetActive (true);
		_background.SetActive (true);
	}

	private void OnEndLoading()	
	{
		StartCoroutine (DelayHide());
	}

	IEnumerator DelayHide()
	{
		yield return new WaitForSeconds (_delayHide);
		foreach(Transform trans in transform)
		{
			trans.gameObject.SetActive (false);
		}
	}
}
