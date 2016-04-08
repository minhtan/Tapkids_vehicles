using UnityEngine;
using System.Collections;

public class StartUpScript : MonoBehaviour {

	#region Vars
	CaptureAndSave snapShot;
	#endregion

	#region Mono
	void Awake(){
		snapShot = GameObject.FindObjectOfType<CaptureAndSave>();
	}


	void Start () {
		ArController.Instance.ToggleAR (true);
		ArController.Instance.SetCenterMode (true);
		ArController.Instance.SetArMaxStimTargets (1);
	}
	#endregion

	public void _CaptureAndSave(){
		snapShot.CaptureAndSaveToAlbum();
	}
}
