using UnityEngine;
using System.Collections;
using UnityStandardAssets.CrossPlatformInput;

[RequireComponent (typeof (ArcadeCarController))]
public class ArcadeCarUserController : MonoBehaviour {

	#region public members
	#endregion public members

	#region private members

	private ArcadeCarController car;
	#endregion private members

	#region Mono
	void Awake () {
		car = GetComponent <ArcadeCarController> ();
	}

	void FixedUpdate () {
		// pass the input to the car!
//		float h = CrossPlatformInputManager.GetAxis("Steer");
//		float v = CrossPlatformInputManager.GetAxis("Accelerate");

		float h = CrossPlatformInputManager.GetAxis("Horizontal");
		float v = CrossPlatformInputManager.GetAxis("Vertical");

		car.Move (h, v);
	}
	#endregion Mono

	#region public functions
	#endregion public functions

	#region private functions
	#endregion private functions

}
