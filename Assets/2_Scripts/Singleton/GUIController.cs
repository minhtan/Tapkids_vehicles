using UnityEngine;
using System.Collections;

public class GUIController : UnitySingletonPersistent<GUIController> {
	public void ToggleGUI(bool state){
		for(int i = 0; i < transform.childCount; i++){
			transform.GetChild (i).gameObject.SetActive (state);
		}
	}
}
