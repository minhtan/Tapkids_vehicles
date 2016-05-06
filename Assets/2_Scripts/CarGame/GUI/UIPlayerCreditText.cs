using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIPlayerCreditText : MonoBehaviour {

	private Text mText;

	void OnEnable () {
		Messenger.AddListener <int> (EventManager.GUI.UPDATE_CREDIT.ToString (), HandleUpdateCredit);
	}
	void OnDisable () {
		Messenger.RemoveListener <int> (EventManager.GUI.UPDATE_CREDIT.ToString (), HandleUpdateCredit);
	}
	void Start () {
		mText = GetComponent <Text> ();
		if (mText != null) 
			mText.text = PlayerDataController.Instance.mPlayer.credit.ToString ();
	}

	private void HandleUpdateCredit (int _credit) {
		mText.text = _credit.ToString ();
	}
}
