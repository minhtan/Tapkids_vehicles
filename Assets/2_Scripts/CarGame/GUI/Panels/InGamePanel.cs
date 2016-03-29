using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class InGamePanel : MonoBehaviour {

	#region public members
	public string panelName = "ingame";
	#endregion public members

	#region private members
	private CanvasGroup mCanvasGroup;
	#endregion private members

	#region Mono
	void OnEnable () {
		CarGameEventController.ToggleInGamePanel += OnToggleInGamePanel;
	}

	void OnDisable () {
		CarGameEventController.ToggleInGamePanel -= OnToggleInGamePanel;
	}

	void Start () {
		mCanvasGroup = GetComponent <CanvasGroup> ();
	}
	#endregion Mono

	#region public functions
	#endregion public functions

	#region private functions
	private void OnToggleInGamePanel (bool _isToggled) {
		mCanvasGroup.alpha = _isToggled ? 1f : 0f;
		mCanvasGroup.interactable = _isToggled ? true : false;
		mCanvasGroup.blocksRaycasts = _isToggled ? true : false;
	}

//	private void OnTogglePanel (string _name) {
//		mCanvasGroup.alpha = panelName.Equals (_name) ? 1f : 0f;
//		mCanvasGroup.interactable = panelName.Equals (_name) ? true : false;
//		mCanvasGroup.blocksRaycasts = panelName.Equals (_name) ? true : false;
//		LeanTween.value (gameObject, panelName.Equals (_name) ? 0f : 1f, panelName.Equals (_name) ? 1f : 0f, 1f)
//			.setOnUpdate ((float alpha) => mCanvasGroup.alpha = alpha)
//			.setOnComplete (() => { 
//				mCanvasGroup.interactable = panelName.Equals (_name) ? true : false;
//				mCanvasGroup.blocksRaycasts = panelName.Equals (_name) ? true : false;
//			});
//	}
	#endregion private functions

}
