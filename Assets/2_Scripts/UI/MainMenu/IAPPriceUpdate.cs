using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class IAPPriceUpdate : MonoBehaviour {

	// Use this for initialization
	void OnEnable () {
		Messenger.AddListener<string> (EventManager.GUI.IAP_INIT.ToString (), HandleStoreInit);
	}

	void OnDisable(){
		Messenger.RemoveListener<string> (EventManager.GUI.IAP_INIT.ToString (), HandleStoreInit);
	}
	
	// Update is called once per frame
	void HandleStoreInit (string price) {
		Text txt = gameObject.GetComponent<Text>();
		if(txt != null){
			txt.text = price;
		}
	}
}
