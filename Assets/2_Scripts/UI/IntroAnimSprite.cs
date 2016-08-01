using UnityEngine;
using System.Collections;

public class IntroAnimSprite : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	public void OnAnimationEnd(){
		Messenger.Broadcast (EventManager.GUI.SPRITE_RUN_FINISH.ToString());
	}
}