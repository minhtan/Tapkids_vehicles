using UnityEngine;
using System.Collections;
using Lean;

public class BtnSelectGame : MonoBehaviour {
	public SceneController.SceneID sceneToLoad;
	MainMenuController3D menu;
	public float swing;

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
				LeanTween.rotateAroundLocal (gameObject, Vector3.forward, swing, 0.1f).setEase (LeanTweenType.linear).setOnComplete (() => {
					LeanTween.rotateAroundLocal (gameObject, Vector3.forward, -swing, 0.9f).setEase (LeanTweenType.easeOutElastic).setOnComplete (() => {
						SceneController.Instance.LoadingSceneAsync (sceneToLoad);				
					});
				});
			}
		}
	}
}
