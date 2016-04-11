using UnityEngine;
using System.Collections;

public class IntroAnimController : MonoBehaviour {

	public GameObject[] _enableGO;
	public GameObject[] _disableGO;


	void OnEnable()
	{
		SceneController.OnStartLoading += OnStartLoading;
	}

	void OnDisable()
	{
		SceneController.OnStartLoading -= OnStartLoading;
	}

	private void OnStartLoading()
	{
		SetActiveGO (_enableGO, true);
		SetActiveGO (_disableGO, false);
	}

	private int LoadingContent()
	{
		SetActiveGO (_enableGO, true);
		SetActiveGO (_disableGO, false);
		return 0;
	}

	public void OnAnimationEnd()
	{
		SceneController.Instance.LoadingSceneAsync (SceneController.SceneID.MENU);
	}

	private void SetActiveGO(GameObject[] gos, bool active)
	{
		for(int i = 0; i < gos.Length; i++)
		{
			gos [i].SetActive (active);
		}
	}
}
