using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Checkcode : MonoBehaviour {
	public Text txt;
	public GameObject pnlCheckCode;
	public bool test;

	void OnEnable(){
		if (!PlayerPrefs.HasKey (GameConstant.UNLOCKED) || PlayerPrefs.GetInt (GameConstant.UNLOCKED) != (int)GameConstant.unlockStatus.VALID) {
			pnlCheckCode.SetActive (true);
		} else {
			SceneController.Instance.LoadingSceneAsync (SceneController.SceneID.MENU);
		}
	}

	void OnCodeValid(){
		PlayerPrefs.SetInt(GameConstant.UNLOCKED, (int)GameConstant.unlockStatus.VALID);
		SceneController.Instance.LoadingSceneAsync (SceneController.SceneID.MENU);
	}

	public void _Check(){
		if(test){
			OnCodeValid();
			return;
		}

		StartCoroutine(WebServiceUltility.CheckKey(WebServiceUltility.CHECK_KEY_URL, txt.text, "", (returnData) => {
			if(returnData != null){
				if(returnData.success){
					//Succes
					OnCodeValid();
				}else{
					//Fail
					if(returnData.status_code == 409){
						//Key ard in use
						GUIController.Instance.OpenDialog(returnData.message)
							.AddButton("No", UIDialogButton.Anchor.BOTTOM_LEFT, () => {})
							.AddButton("Yes", UIDialogButton.Anchor.BOTTOM_RIGHT, () => {
								StartCoroutine(WebServiceUltility.CheckKey(WebServiceUltility.OVERRIDE_KEY_URL, txt.text, "", (returnOverRideKeyData) => {
								if(returnOverRideKeyData != null){
									if(returnOverRideKeyData.success){
										OnCodeValid();
									}else{
										GUIController.Instance.OpenDialog(returnOverRideKeyData.message).AddButton("Ok", UIDialogButton.Anchor.BOTTOM_CENTER, () => {});
									}
								}else{
										GUIController.Instance.OpenDialog("Fail to connect").AddButton("Ok", UIDialogButton.Anchor.BOTTOM_CENTER, () => {});
								}
							}));
						});
							
					}
					else{
						//Key invalid

						GUIController.Instance.OpenDialog(returnData.message).AddButton("Ok", UIDialogButton.Anchor.BOTTOM_CENTER, () => {});
					}
				}
			}else{
				//Fail to connect
				GUIController.Instance.OpenDialog("Fail to connect").AddButton("Ok", UIDialogButton.Anchor.BOTTOM_CENTER, () => {});
			}
		}));
	}

}
