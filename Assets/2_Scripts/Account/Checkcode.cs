using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Text.RegularExpressions;
using Lean;

public class Checkcode : MonoBehaviour {
	public InputField inputCode;
	public GameObject pnlCheckCode;
	public GameObject btnQRBack;
	public GameObject pnlWait;
	public GameObject garage;
	public bool test;
	public bool activationRequired;

	void OnEnable(){
		if (test) {
			PlayerPrefs.DeleteKey (GameConstant.UNLOCKED);
		}else if (!PlayerPrefs.HasKey (GameConstant.UNLOCKED) || PlayerPrefs.GetInt (GameConstant.UNLOCKED) != (int)GameConstant.unlockStatus.VALID) {
			pnlWait.SetActive (true);

			StartCoroutine(WebServiceUltility.CheckDevice((returnData) => {
				if(returnData != null){
					if(returnData.success){
						OnCodeValid();
					}else{
						PlayerPrefs.SetInt(GameConstant.UNLOCKED, (int)GameConstant.unlockStatus.INVALID);
						pnlCheckCode.SetActive (true);
						pnlWait.SetActive (false);
					}
				}
			}));
		} else {
			
		}

		Messenger.AddListener<string> (EventManager.AR.QR_TRACKING.ToString(), HandleQrTrack);
	}

	void OnDisable(){
		Messenger.RemoveListener<string> (EventManager.AR.QR_TRACKING.ToString(), HandleQrTrack);
	}

	void HandleQrTrack(string qrText){
		_Check (qrText);
		inputCode.text = qrText;
		ToogleQR (false);
	}

	public void ToogleQR(bool state){
		ArController.Instance.ToggleAR (state, state, false);
		pnlCheckCode.SetActive (!state);
		btnQRBack.SetActive (state);
		garage.SetActive (!state);
		Messenger.Broadcast <bool> (EventManager.GUI.TOGGLE_PLAYER_PNL.ToString (), !state);
		Messenger.Broadcast <bool> (EventManager.GUI.TOGGLE_SFX_BTN.ToString (), !state);
	}

	public void ShowActivationPnl(){
		pnlCheckCode.SetActive (true);
	}

	public void OnCodeValid(){
		PlayerPrefs.SetInt(GameConstant.UNLOCKED, (int)GameConstant.unlockStatus.VALID);
		ArController.Instance.ToggleAR (false, false, false);
		pnlCheckCode.SetActive (false);
		btnQRBack.SetActive (false);
		pnlWait.SetActive (false);
		GUIController.Instance.OpenDialog(LeanLocalization.GetTranslation("UnlockedSuccess").Text).AddButton(
			LeanLocalization.GetTranslation("Ok").Text,
			UIDialogButton.Anchor.CENTER, 0, -25,
			() => {inputCode.text = "";}
		);
	}

	public void _Check(string theCode = null){
		if(test){
			OnCodeValid();
			return;
		}

		string textToCheck = (string.IsNullOrEmpty(theCode)) ? inputCode.text : theCode;

		if (!CheckRegex (textToCheck)) {
			GUIController.Instance.OpenDialog (LeanLocalization.GetTranslation("InvalidKey").Text).AddButton (
				LeanLocalization.GetTranslation("Ok").Text, 
				UIDialogButton.Anchor.CENTER, 0, -25,
					() => {inputCode.text = "";}
			);
			return;
		}

		pnlWait.SetActive (true);
		StartCoroutine(WebServiceUltility.CheckKey(WebServiceUltility.CHECK_KEY_URL, textToCheck, "", (returnData) => {
			pnlWait.SetActive (false);
			if(returnData != null){
				if(returnData.success){
					//Succes
					Debug.Log("success");
					OnCodeValid();
				}else{
					//Fail
					if(returnData.status_code == 409){
						//Key ard in use
						GUIController.Instance.OpenDialog(returnData.message)
							.AddButton(
								LeanLocalization.GetTranslation("No").Text, 
								UIDialogButton.Anchor.CENTER, 0, -135,
									() => {inputCode.text = "";}
							)
							.AddButton(
								LeanLocalization.GetTranslation("Yes").Text, 
								UIDialogButton.Anchor.CENTER,  0, -25,
								() => {
									StartCoroutine(WebServiceUltility.CheckKey(WebServiceUltility.OVERRIDE_KEY_URL, textToCheck, "", (returnOverRideKeyData) => {
									if(returnOverRideKeyData != null){
										if(returnOverRideKeyData.success){
											OnCodeValid();
										}else{
											GUIController.Instance.OpenDialog(returnOverRideKeyData.message).AddButton(
												LeanLocalization.GetTranslation("Ok").Text, 
													UIDialogButton.Anchor.CENTER, 0, -25, 
													() => {inputCode.text = "";}
											);
										}
									}else{
										GUIController.Instance.OpenDialog("Fail to connect").AddButton(
											LeanLocalization.GetTranslation("Ok").Text, 
												UIDialogButton.Anchor.CENTER, 0, -25, 
												() => {inputCode.text = "";}
										);
								}
							}));
						});
							
					}
					else{
						//Key invalid
						GUIController.Instance.OpenDialog(returnData.message).AddButton(
							LeanLocalization.GetTranslation("Ok").Text,  
							UIDialogButton.Anchor.CENTER, 0, -25, 
								() => {inputCode.text = "";}
						);
					}
				}
			}else{
				//Fail to connect
				GUIController.Instance.OpenDialog(LeanLocalization.GetTranslation("FailedToConnect").Text).AddButton(
					LeanLocalization.GetTranslation("Ok").Text, 
					UIDialogButton.Anchor.CENTER, 0, -25, 
						() => {inputCode.text = "";}
				);
			}
		}));
	}

	bool CheckRegex(string keyInput){
		if(keyInput.Length < 3){
			return false;
		}

		Regex regex = new Regex("\\W+");
		Match match = regex.Match(keyInput);
		if (match.Success) {
			return false;
		} else {
			return true;
		}
	}

	public bool IsGameActivated(){
		if (!activationRequired)
			return true;
		
		if (PlayerPrefs.HasKey (GameConstant.UNLOCKED) && PlayerPrefs.GetInt (GameConstant.UNLOCKED) == (int)GameConstant.unlockStatus.VALID) {
			return true;
		} else {
			return false;
		}
	}

}
