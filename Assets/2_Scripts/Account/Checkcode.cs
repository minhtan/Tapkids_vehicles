using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Text.RegularExpressions;
using Lean;

public class Checkcode : MonoBehaviour {
	public Text txt;
	public GameObject pnlCheckCode;
	public GameObject btnQRBack;
	public bool test;
	public bool activationRequired;
	public bool isProcessing = false;

	void OnEnable(){
//		PlayerPrefs.DeleteKey (GameConstant.UNLOCKED);
		if (!PlayerPrefs.HasKey (GameConstant.UNLOCKED) || PlayerPrefs.GetInt (GameConstant.UNLOCKED) != (int)GameConstant.unlockStatus.VALID) {
			pnlCheckCode.SetActive (true);

			StartCoroutine(WebServiceUltility.CheckDevice((returnData) => {
				if(returnData != null){
					if(returnData.success){
						OnCodeValid();
					}else{
						PlayerPrefs.SetInt(GameConstant.UNLOCKED, (int)GameConstant.unlockStatus.INVALID);
						pnlCheckCode.SetActive (true);
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
		Debug.Log (qrText);
		if(!isProcessing){
			isProcessing = true;
			_Check (qrText);
		}
	}

	public void ToogleQR(bool state){
		ArController.Instance.ToggleAR (state, state);
		pnlCheckCode.SetActive (!state);
		btnQRBack.SetActive (state);
	}

	public void ShowActivationPnl(){
		pnlCheckCode.SetActive (true);
	}

	void OnCodeValid(){
		PlayerPrefs.SetInt(GameConstant.UNLOCKED, (int)GameConstant.unlockStatus.VALID);
		ArController.Instance.ToggleAR (false);
		pnlCheckCode.SetActive (false);
		btnQRBack.SetActive (false);
		isProcessing = true;
	}

	public void _Check(string theCode = null){
		if(test){
			OnCodeValid();
			return;
		}

		string textToCheck = (string.IsNullOrEmpty(theCode)) ? txt.text : theCode;

		if (!CheckRegex (textToCheck)) {
			GUIController.Instance.OpenDialog (LeanLocalization.GetTranslation("InvalidKey").Text).AddButton (
				LeanLocalization.GetTranslation("Ok").Text, 
				UIDialogButton.Anchor.BOTTOM_CENTER, 
				() => {isProcessing = false;}
			);
			return;
		}

		StartCoroutine(WebServiceUltility.CheckKey(WebServiceUltility.CHECK_KEY_URL, textToCheck, "", (returnData) => {
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
								UIDialogButton.Anchor.BOTTOM_LEFT, 
								() => {}
							)
							.AddButton(
								LeanLocalization.GetTranslation("Yes").Text, 
								UIDialogButton.Anchor.BOTTOM_RIGHT, 
								() => {
									StartCoroutine(WebServiceUltility.CheckKey(WebServiceUltility.OVERRIDE_KEY_URL, textToCheck, "", (returnOverRideKeyData) => {
									if(returnOverRideKeyData != null){
										if(returnOverRideKeyData.success){
											OnCodeValid();
										}else{
											GUIController.Instance.OpenDialog(returnOverRideKeyData.message).AddButton(
												LeanLocalization.GetTranslation("Ok").Text, 
												UIDialogButton.Anchor.BOTTOM_CENTER, 
												() => {isProcessing = false;}
											);
										}
									}else{
										GUIController.Instance.OpenDialog("Fail to connect").AddButton(
											LeanLocalization.GetTranslation("Ok").Text, 
											UIDialogButton.Anchor.BOTTOM_CENTER, 
											() => {isProcessing = false;}
										);
								}
							}));
						});
							
					}
					else{
						//Key invalid
						GUIController.Instance.OpenDialog(returnData.message).AddButton(
							LeanLocalization.GetTranslation("Ok").Text,  
							UIDialogButton.Anchor.BOTTOM_CENTER, 
							() => {isProcessing = false;}
						);
					}
				}
			}else{
				//Fail to connect
				GUIController.Instance.OpenDialog(LeanLocalization.GetTranslation("FailedToConnect").Text).AddButton(
					LeanLocalization.GetTranslation("Ok").Text, 
					UIDialogButton.Anchor.BOTTOM_CENTER, 
					() => {}
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
