using UnityEngine;
using System.Collections;

public class Test : MonoBehaviour {

	// Use this for initialization
	void Start () {
		ArController.Instance.ToggleAR (true);
		ArController.Instance.SetCenterMode (false);
		ArController.Instance.SetArMaxStimTargets (1);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
