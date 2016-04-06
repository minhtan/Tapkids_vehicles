using UnityEngine;
using System.Collections;

public class StartUpScript : MonoBehaviour {

	#region Vars

	#endregion

	#region Mono
	void Start () {
		ArController.Instance.ToggleAR (true);
		ArController.Instance.SetCenterMode (true);
		ArController.Instance.SetArMaxStimTargets (1);
	}
	#endregion


}
