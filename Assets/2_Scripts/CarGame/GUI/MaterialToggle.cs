using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MaterialToggle : MonoBehaviour {

	public int matId;

	private Toggle mToggle;

	void Start () {
		mToggle = GetComponent <Toggle> ();
		if (mToggle != null) {
			mToggle.onValueChanged.AddListener (delegate {
				if (mToggle.isOn)
					Messenger.Broadcast <int> (EventManager.GUI.CHANGE_MATERIAL.ToString (), matId);
			});
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
