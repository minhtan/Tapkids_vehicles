using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameOverPanel : MonoBehaviour {

	#region public members
	public Text gameOverMessage;
	public Image[] starImages;
	#endregion public members

	#region private members
	private CanvasGroup mCanvasGroup;
	#endregion private members

	#region Mono
	void OnEnable () {
		Messenger.AddListener <int> (EventManager.GameState.GAMEOVER.ToString (), HandleGameOver);
	}

	void OnDisable () {
		Messenger.RemoveListener <int> (EventManager.GameState.GAMEOVER.ToString (), HandleGameOver);
	}

	void Start () {
		mCanvasGroup = GetComponent <CanvasGroup> ();
	}
	#endregion Mono

	#region public functions
	#endregion public functions

	#region private functions
	private void HandleGameOver (int _starNum) {
		mCanvasGroup.alpha = 1f;
		mCanvasGroup.interactable = true;
		mCanvasGroup.blocksRaycasts = true;

		if (gameOverMessage != null) {
			gameOverMessage.text = GameMessages.WinMessage;
		}

		if (starImages.Length > 0 && starImages.Length <= _starNum) {
			int i = 0; 
			while (i < _starNum) {
				starImages[i].enabled = true;
				i++;
			}
		}
//
//		mCanvasGroup.alpha = _isToggled ? 1f : 0f;
//		mCanvasGroup.interactable = _isToggled ? true : false;
//		mCanvasGroup.blocksRaycasts = _isToggled ? true : false;
	}
	#endregion private functions

}
