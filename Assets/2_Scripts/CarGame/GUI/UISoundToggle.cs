using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UISoundToggle : MonoBehaviour {

	#region pivate members
	private CanvasGroup mCanvasGroup;
	private Toggle mToggle;
	#endregion pivate members
	// Use this for initialization
	void Start () {
		mCanvasGroup = GetComponent <CanvasGroup> ();
		mToggle = GetComponent <Toggle> ();

		if (mToggle) {
			mToggle.onValueChanged.AddListener (delegate {
				if (mToggle.isOn) {
					AudioManager.Instance.ToggleSound (true);
					PlayerPrefs.SetInt (GameConstant.SFX_STATE, 1); 
				} else {
					AudioManager.Instance.ToggleSound (false);
					PlayerPrefs.SetInt (GameConstant.SFX_STATE, 0);
				}
			});
		}
		//
		if (PlayerPrefs.HasKey (GameConstant.SFX_STATE)) {
			int volume = PlayerPrefs.GetInt (GameConstant.SFX_STATE);
			if (volume == 0) {
				mToggle.isOn = false;
			} else {
				mToggle.isOn = true;
			}
		} else {
			PlayerPrefs.SetInt (GameConstant.SFX_STATE, 1);
		}	
	}

	void OnEnable () {
		Messenger.AddListener <bool> (EventManager.GUI.TOGGLE_SFX_BTN.ToString (), HandleToggleSFXButton);
	}

	void OnDisable () {
		Messenger.RemoveListener <bool> (EventManager.GUI.TOGGLE_SFX_BTN.ToString (), HandleToggleSFXButton);
	}

	void HandleToggleSFXButton (bool _isOn) {
		mCanvasGroup.alpha = _isOn ? 1f : 0f;
		mCanvasGroup.interactable = _isOn ? true : false;
		mCanvasGroup.blocksRaycasts = _isOn ? true : false;
	}




}
