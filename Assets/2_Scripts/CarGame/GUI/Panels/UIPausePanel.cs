using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIPausePanel : MonoBehaviour {

	#region public members
	public string panelName = "pause";
	#endregion public members

	#region private members
	CanvasGroup mCanvasGroup;
	#endregion private members

	#region Mono
	void OnEnable () {
//		CarGameEventController.TogglePanel += OnTogglePanel;
//		CarGameEventController.PauseGame += OnPauseGame;
		Messenger.AddListener <bool> (EventManager.GameState.PAUSEGAME.ToString (), HandlePauseGame);
	}
	void Disable () {
//		CarGameEventController.TogglePanel -= OnTogglePanel;
//		CarGameEventController.PauseGame -= OnPauseGame;
		Messenger.RemoveListener <bool> (EventManager.GameState.PAUSEGAME.ToString (), HandlePauseGame);
	}

	void Start () {
		mCanvasGroup = GetComponent <CanvasGroup> ();
	}

	#endregion Mono

	#region public functions
	#endregion public functions

	#region private functions
	private void HandlePauseGame (bool _isPaused) {
		mCanvasGroup.alpha = _isPaused ? 1f : 0f;
		mCanvasGroup.interactable = _isPaused ? true : false;
		mCanvasGroup.blocksRaycasts = _isPaused ? true : false;
	}

	private void OnTogglePanel (string _name) {
		mCanvasGroup.alpha = panelName.Equals (_name) ? 1f : 0f;
		mCanvasGroup.interactable = panelName.Equals (_name) ? true : false;
		mCanvasGroup.blocksRaycasts = panelName.Equals (_name) ? true : false;
//		LeanTween.value (gameObject, panelName.Equals (_name) ? 0f : 1f, panelName.Equals (_name) ? 1f : 0f, 1f)
//			.setOnUpdate ((float alpha) => mCanvasGroup.alpha = alpha)
//			.setOnComplete (() => { 
//				mCanvasGroup.interactable = panelName.Equals (_name) ? true : false;
//				mCanvasGroup.blocksRaycasts = panelName.Equals (_name) ? true : false;
//			});
	}
	#endregion private functions

}
