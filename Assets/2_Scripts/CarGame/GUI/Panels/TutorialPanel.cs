using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class TutorialPanel : MonoBehaviour, IPointerClickHandler {

	#region public members
	public string panelName = "tutorial";
	#endregion public members

	#region private members
	private int currentStep = -1;

	private CanvasGroup mCanvasGroup;
	#endregion private members

	#region Mono
	void OnEnable () {
//		CarGameEventController.ToggleTutorialPanel += OnToggleTutorialPanel;
		Messenger.AddListener <bool> (EventManager.GUI.TOGGLE_TUTORIAL.ToString (), HandleToggleTutorialPanel);
	}

	void Disable () {
//		CarGameEventController.ToggleTutorialPanel -= OnToggleTutorialPanel;
		Messenger.RemoveListener <bool> (EventManager.GUI.TOGGLE_TUTORIAL.ToString (), HandleToggleTutorialPanel);
	}

	void Start () {
		mCanvasGroup = GetComponent <CanvasGroup> ();
	}
	#endregion Mono

	#region IPointerClickHandler implementation

	public void OnPointerClick (PointerEventData eventData)
	{
		RectTransform mRectTransform = GetComponent <RectTransform> ();
		if (mRectTransform != null) {
			Vector2 localCursor;
			if (!RectTransformUtility.ScreenPointToLocalPointInRectangle(GetComponent<RectTransform>(), eventData.position, eventData.pressEventCamera, out localCursor))
				return;

			if (localCursor.x > 0) { // NEXT
				if (currentStep >= 0)
					transform.GetChild (currentStep).gameObject.SetActive (false);

				if ( currentStep < transform.childCount)
					transform.GetChild (++currentStep).gameObject.SetActive (true);
				if (currentStep > 3) {
//					CarGameEventController.OnTogglePanel ("ingame");
					HandleToggleTutorialPanel (false);
					currentStep = -1;
				}
			} else { // SKIP
//				CarGameEventController.OnTogglePanel ("ingame");
				HandleToggleTutorialPanel (false);
				currentStep = -1;
			}
//			Debug.Log("LocalCursor:" + localCursor);
		}
	}
	#endregion

	#region public functions
	#endregion public functions

	#region private functions
	private void HandleToggleTutorialPanel (bool _isToggled) {
		mCanvasGroup.alpha =_isToggled ? 1f : 0f;
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
