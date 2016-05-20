using UnityEngine;
using System.Collections;
using Lean;

public class BtnSelectGame : MonoBehaviour {
	public SceneController.SceneID sceneToLoad;
	MainMenuController3D menu;

	void Start(){
		menu = FindObjectOfType<MainMenuController3D> ();
	}

	void OnEnable(){
		Messenger.AddListener<GameObject> (EventManager.GUI.MENU_BTN_TAP.ToString(), OnBtnTap);
	}

	void OnDisable(){
		Messenger.RemoveListener<GameObject> (EventManager.GUI.MENU_BTN_TAP.ToString(), OnBtnTap);
	}

	void OnBtnTap(GameObject go){
		if(menu.IsInMenu){
			if(go.GetInstanceID() == gameObject.GetInstanceID()){
				SceneController.Instance.LoadingSceneAsync (sceneToLoad);
			}
		}
	}
}
