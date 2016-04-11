using UnityEngine;
using System.Collections;

public class LoadingScene : MonoBehaviour {

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
		StartCoroutine (DelayHide());
	}

	IEnumerator DelayHide()
	{
		yield return new WaitForSeconds (.5f);
		foreach(Transform trans in transform)
		{
			trans.gameObject.SetActive (false);
		}
	}
}
