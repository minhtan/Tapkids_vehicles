using UnityEngine;
using System.Collections;
using UnityStandardAssets.CrossPlatformInput;

[RequireComponent (typeof (ArcadeCarController))]
public class ArcadeCarUserController : MonoBehaviour {

	#region public members
	#endregion public members

	#region private members

	private ArcadeCarController car;
	private float wheelAngle;
	#endregion private members

	#region Mono


	void Awake () {
		car = GetComponent <ArcadeCarController> ();
	}

	void OnEnable () {
		Messenger.AddListener <float> (EventManager.GUI.WHEEL_FRAME_TURN.ToString (), HandleWheelTurn);
	}

	void OnDisable () {
		Messenger.RemoveListener <float> (EventManager.GUI.WHEEL_FRAME_TURN.ToString (), HandleWheelTurn);
	}

	void FixedUpdate () {
		// pass the input to the car!
//		float h = CrossPlatformInputManager.GetAxis("Steer");
//		float v = CrossPlatformInputManager.GetAxis("Accelerate");

//		float h = CrossPlatformInputManager.GetAxis("Horizontal");
		float v = CrossPlatformInputManager.GetAxis("Vertical");

		car.Move (wheelAngle, v);
	}
	#endregion Mono

	#region public functions
	#endregion public functions

	#region private functions
	void HandleWheelTurn (float angle) {
		wheelAngle = angle/3;

	}
	#endregion private functions

}
