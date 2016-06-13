using UnityEngine;
using System.Collections;

public class IntroAnimController : MonoBehaviour {

	public GameObject[] _enableGO;
	public GameObject[] _disableGO;


	void OnEnable()
	{
		SceneController.OnEndLoading += OnEndLoading;
	}

	void OnDisable()
	{
		SceneController.OnEndLoading -= OnEndLoading;
	}

	private void OnEndLoading()
	{
		SetActiveGO (_enableGO, true);
		SetActiveGO (_disableGO, false);
		AudioManager.Instance.PlayAudio (AudioKey.UNIQUE_KEY.BACKGROUD);
	}
		
	public void OnAnimationEnd()
	{
		OnEndLoading();
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
