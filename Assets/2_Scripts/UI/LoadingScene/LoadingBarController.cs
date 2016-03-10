using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class LoadingBarController : MonoBehaviour {
	public Image _barForeGround;

	void OnEnable()
	{
		SceneController.OnLoadingScene += OnLoadingScene;
	}

	void OnDisable()
	{
		SceneController.OnLoadingScene -= OnLoadingScene;
	}

	private void OnLoadingScene(float progress)
	{
		_barForeGround.fillAmount = progress;
	}
}
