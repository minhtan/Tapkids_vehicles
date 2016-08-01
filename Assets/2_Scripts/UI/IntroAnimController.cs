using UnityEngine;
using System.Collections;

public class IntroAnimController : MonoBehaviour {

	public MediaPlayerCtrl scrMedia;
	public LoadingScene loadingCanvas;

	public GameObject[] _enableGO;
	public GameObject[] _disableGO;


	void OnEnable()
	{
//		SceneController.OnEndLoading += OnEndLoading;
		scrMedia.OnEnd += OnMediaEnd;
		Messenger.AddListener (EventManager.GUI.SPRITE_RUN_FINISH.ToString(), OnAnimationEnd);
	}

	void OnDisable()
	{
//		SceneController.OnEndLoading -= OnEndLoading;
	}

	void OnMediaEnd()
	{
		LoadMenu ();
	}

	private void OnEndLoading()
	{
		SetActiveGO (_disableGO, false);
	}
		
	public void OnAnimationEnd()
	{
		OnEndLoading();
		scrMedia.Play ();
	}

	public void LoadMenu(){
		loadingCanvas.ShowLoading ();
		StartCoroutine (Wait ());
	}

	IEnumerator Wait(){
		yield return null;
		SetActiveGO (_enableGO, true);
		AudioManager.Instance.PlayAudio (AudioKey.UNIQUE_KEY.BACKGROUD);
		SceneController.Instance.LoadingSceneAsync (SceneController.SceneID.MENU);
	}

	private void SetActiveGO(GameObject[] gos, bool active)
	{
		for(int i = 0; i < gos.Length; i++)
		{
			if(gos[i].activeSelf != active)
				gos [i].SetActive (active);
		}
	}
}
