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
		Messenger.AddListener<int> (EventManager.GUI.MENU_BTN_TAP.ToString(), OnBtnTap);
	}

	void OnDisable(){
		Messenger.RemoveListener<int> (EventManager.GUI.MENU_BTN_TAP.ToString(), OnBtnTap);
	}

	void OnBtnTap(int _id){
		if(menu.IsInMenu){
			if(_id == gameObject.GetInstanceID()){
				LeanTween.rotateAroundLocal (gameObject, Vector3.forward, 720f, 0.5f).setEase (LeanTweenType.easeOutBack).setOnComplete (() => {
					SceneController.Instance.LoadingSceneAsync (sceneToLoad);				
				});
			}
		}
	}
}
