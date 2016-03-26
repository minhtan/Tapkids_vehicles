using UnityEngine;
using System.Collections;

public class AutoRotate : MonoBehaviour {

	#region public members
	#endregion public members

	#region private members
	#endregion private members

	#region Mono
	void Start () {
		LeanTween.rotateAround (gameObject, Vector3.up, 360f, 5f).setRepeat (-1);
	}
	#endregion Mono

	#region public functions
	#endregion public functions

	#region private functions
	#endregion private functions

}
