using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum CarColor {
	orange,
	gray,
	yellow,
	blue,
	red,
	pink,
	purple,
	green,
	bluesky
}

public class ArcadeCarController : MonoBehaviour {
	#region public members
	[SerializeField]
	public Vehicle vehicle;

	#endregion

	#region private members
	[SerializeField] private WheelCollider[] wheelColliders = new WheelCollider[4];
	[SerializeField] private GameObject[] wheelMeshes = new GameObject [4];
	[SerializeField] private Vector3 centerOfMass;
	[Range(0, 1)] [SerializeField] private float steerHelper;
	[SerializeField] private float maximumSteerAngle = 25f;
	[SerializeField] private float maxMotorTorque = 200f;
	[SerializeField] private float brakeTorque = 20000f;

	private float oldRotation;
	private float steerAngle;
	private bool isStart;

	private Rigidbody mRigidbody;
	private Transform mTransform;

	#endregion private members

	#region MONO
	void Awake () {
		mTransform = GetComponent <Transform> ();
		mRigidbody = GetComponent <Rigidbody> ();
		mRigidbody.centerOfMass = centerOfMass;
//		isStart = true;
	}

	void OnEnable () {
		Messenger.AddListener <bool> (EventManager.GameState.START.ToString (), HandleStartGame);
		Messenger.AddListener <int> (EventManager.GameState.GAMEOVER.ToString (), HandleGameOver);
		Messenger.AddListener(EventManager.GameState.RESET.ToString (), HandleResetGame);

	}

	void OnDisable () {
		Messenger.AddListener <bool> (EventManager.GameState.START.ToString (), HandleStartGame);
		Messenger.RemoveListener <int> (EventManager.GameState.GAMEOVER.ToString (), HandleGameOver);
		Messenger.RemoveListener (EventManager.GameState.RESET.ToString (), HandleResetGame);
	}
	#endregion MONO

	#region public functions
	// handle car movement
	public void Move (float steer, float accel) {
//		if (!isStart) return;

		for (int i = 0; i < 4; i++) {
			Quaternion quaternion;
			Vector3 position;
			wheelColliders[i].GetWorldPose(out position, out quaternion);
			if (wheelMeshes[i] != null) {
				wheelMeshes[i].transform.position = position;
				wheelMeshes[i].transform.rotation = quaternion;
			}
		}

		// clamp input value
		steer = Mathf.Clamp (steer, -1, 1);
		steerAngle = steer * maximumSteerAngle;
		steerAngle = Mathf.Clamp (steerAngle, maximumSteerAngle * -1, maximumSteerAngle);

		wheelColliders[0].steerAngle = steerAngle;
		wheelColliders[1].steerAngle = steerAngle;

//		accel = Mathf.Clamp (accel, 0, 1);
		ApplyDrive (accel);

		SteerHelper ();
	}
	#endregion public functions

	#region private functions
	private void ApplyDrive (float accel) {
//		if (accel > 0) {
		for (int i = 0; i < 4; i++) {
			wheelColliders[i].brakeTorque = 0f;
			wheelColliders[i].motorTorque = accel * maxMotorTorque;
		}
		if (accel == 0) {
			for (int i = 0; i < 4; i++) 
				wheelColliders[i].brakeTorque = brakeTorque;
		}
//		}
//		else {
//			for (int i = 0; i < 4; i++) 
//				wheelColliders[i].brakeTorque = brakeTorque;
//		}
	}

	private void SteerHelper()
	{
		for (int i = 0; i < 4; i++)
		{
			WheelHit wheelhit;
			wheelColliders[i].GetGroundHit(out wheelhit);
			if (wheelhit.normal == Vector3.zero)
				return; // wheels arent on the ground so dont realign the rigidbody velocity
		}

		// this if is needed to avoid gimbal lock problems that will make the car suddenly shift direction
		if (Mathf.Abs(oldRotation - mTransform.eulerAngles.y) < 10f)
		{
			var turnadjust = (mTransform.eulerAngles.y - oldRotation) * steerHelper;
			Quaternion velRotation = Quaternion.AngleAxis(turnadjust, Vector3.up);
			mRigidbody.velocity = velRotation * mRigidbody.velocity;
		}
		oldRotation = mTransform.eulerAngles.y;
	}


	#endregion private functions

	#region event subscribers
	private void HandleStartGame (bool _state) {
		if (_state) {
			isStart = true;
		} else {

		}
	}

	private void HandleGameOver (int _starAmount) {
		isStart = false;
		for (int i = 0; i < 4; i++) 
			wheelColliders[i].brakeTorque = brakeTorque;
	}

	private void HandleResetGame () {
		for (int i = 0; i < 4; i++) 
			wheelColliders[i].brakeTorque = brakeTorque;
	}


	#endregion event subscribers

}