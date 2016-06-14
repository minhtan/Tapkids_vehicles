using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Text.RegularExpressions;
using Lean;

public class Checkcode : MonoBehaviour {
	public Text txt;
	public GameObject pnlCheckCode;
	public bool test;
	public bool activationRequired;

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
//			SceneController.Instance.LoadingSceneAsync (SceneController.SceneID.MENU);
		}
	}

	public void ShowActivationPnl(){
		pnlCheckCode.SetActive (true);
	}

	void OnCodeValid(){
		PlayerPrefs.SetInt(GameConstant.UNLOCKED, (int)GameConstant.unlockStatus.VALID);
		pnlCheckCode.SetActive (false);

//		SceneController.Instance.LoadingSceneAsync (SceneController.SceneID.MENU);
	}

	public void _Check(){
		if(test){
			OnCodeValid();
			return;
		}

		if (!CheckRegex (txt.text)) {
			GUIController.Instance.OpenDialog (LeanLocalization.GetTranslation("InvalidKey").Text).AddButton (
				LeanLocalization.GetTranslation("Ok").Text, 
				UIDialogButton.Anchor.BOTTOM_CENTER, 
				() => {});
			return;
		}

		StartCoroutine(WebServiceUltility.CheckKey(WebServiceUltility.CHECK_KEY_URL, txt.text, "", (returnData) => {
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
									StartCoroutine(WebServiceUltility.CheckKey(WebServiceUltility.OVERRIDE_KEY_URL, txt.text, "", (returnOverRideKeyData) => {
									if(returnOverRideKeyData != null){
										if(returnOverRideKeyData.success){
											OnCodeValid();
										}else{
											GUIController.Instance.OpenDialog(returnOverRideKeyData.message).AddButton(
													LeanLocalization.GetTranslation("Ok").Text, 
													UIDialogButton.Anchor.BOTTOM_CENTER, 
													() => {});
										}
									}else{
											GUIController.Instance.OpenDialog("Fail to connect").AddButton(
												LeanLocalization.GetTranslation("Ok").Text, 
												UIDialogButton.Anchor.BOTTOM_CENTER, 
												() => {});
								}
							}));
						});
							
					}
					else{
						//Key invalid
						GUIController.Instance.OpenDialog(returnData.message).AddButton(
							LeanLocalization.GetTranslation("Ok").Text,  
							UIDialogButton.Anchor.BOTTOM_CENTER, 
							() => {});
					}
				}
			}else{
				//Fail to connect
				GUIController.Instance.OpenDialog(LeanLocalization.GetTranslation("FailedToConnect").Text).AddButton(
					LeanLocalization.GetTranslation("Ok").Text, 
					UIDialogButton.Anchor.BOTTOM_CENTER, 
					() => {});
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
