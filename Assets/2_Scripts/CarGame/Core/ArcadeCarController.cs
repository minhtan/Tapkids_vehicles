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
internal enum CarDriveType
{
	FrontWheelDrive,
	RearWheelDrive,
	FourWheelDrive
}
internal enum SpeedType
{
	MPH,
	KPH
}
public class ArcadeCarController : MonoBehaviour {
	#region public members
	[SerializeField]
	public Vehicle vehicle;


	public float CurrentSpeed{ get { return mRigidbody.velocity.magnitude*2.23693629f; }}
	public float MaxSpeed{get { return topspeed; }}
	public float Revs { get; private set; }
	public float AccelInput { get; private set; }
	public float BrakeInput { get; private set; }
	#endregion

	#region private members
	[SerializeField] private CarDriveType m_CarDriveType = CarDriveType.FourWheelDrive;
	[SerializeField] private WheelCollider[] wheelColliders = new WheelCollider[4];
	[SerializeField] private GameObject[] wheelMeshes = new GameObject [4];
//	[SerializeField] private WheelEffects[] m_WheelEffects = new WheelEffects[4];
	[SerializeField] private Vector3 centerOfMass;
	[SerializeField] private float maximumSteerAngle = 25f;
	[Range(0, 1)] [SerializeField] private float steerHelper;
	[Range(0, 1)] [SerializeField] private float m_TractionControl; // 0 is no traction control, 1 is full interference
	[SerializeField] private float fullTorqueOverAllWheels;
	[SerializeField] private float reverseTorque;
	[SerializeField] private float maxHandbrakeTorque;
	[SerializeField] private float downforce = 100f;
	[SerializeField] private SpeedType mSpeedType;


	[SerializeField] private float topspeed = 200;
	[SerializeField] private static int NoOfGears = 5;
	[SerializeField] private float revRangeBoundary = 1f;
	[SerializeField] private float m_SlipLimit;
	[SerializeField] private float brakeTorque = 20000f;
	private float oldRotation;
	private float currentTorque;
	private float steerAngle;
	private bool isStart;

	private Rigidbody mRigidbody;
	private Transform mTransform;
	private int m_GearNum;
	private float m_GearFactor;

	#endregion private members

	#region MONO
	void OnEnable () {
		Messenger.AddListener <bool> (EventManager.GameState.START.ToString (), HandleStartGame);
		Messenger.AddListener <int> (EventManager.GameState.GAMEOVER.ToString (), HandleGameOver);
		Messenger.AddListener(EventManager.GameState.RESET.ToString (), HandleResetGame);

	}

	void Awake () {
		mTransform = GetComponent <Transform> ();
		mRigidbody = GetComponent <Rigidbody> ();
		mRigidbody.centerOfMass = centerOfMass;
	}

	void Start () {
		maxHandbrakeTorque = float.MaxValue;
		currentTorque = fullTorqueOverAllWheels - (m_TractionControl*fullTorqueOverAllWheels);
	}


	void OnDisable () {
		Messenger.AddListener <bool> (EventManager.GameState.START.ToString (), HandleStartGame);
		Messenger.RemoveListener <int> (EventManager.GameState.GAMEOVER.ToString (), HandleGameOver);
		Messenger.RemoveListener (EventManager.GameState.RESET.ToString (), HandleResetGame);
	}
	#endregion MONO

	#region public functions
	// handle car movement
	public void Move (float steer, float accel, float footbrake, float handbrake) {
		if (!isStart) return;

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

		wheelColliders[0].steerAngle = steerAngle;
		wheelColliders[1].steerAngle = steerAngle;

		AccelInput = accel = Mathf.Clamp(accel, 0, 1);
		BrakeInput = footbrake = -1*Mathf.Clamp(footbrake, -1, 0);
		handbrake = Mathf.Clamp(handbrake, 0, 1);

		SteerHelper ();
		ApplyDrive (accel, footbrake);
		CapSpeed ();

		//Set the handbrake.
		//Assuming that wheels 2 and 3 are the rear wheels.
		if (handbrake > 0f)
		{
			var hbTorque = handbrake * maxHandbrakeTorque;
			wheelColliders[2].brakeTorque = hbTorque;
			wheelColliders[3].brakeTorque = hbTorque;
		}

		CalculateRevs();
		GearChanging();

		AddDownForce();
//		CheckForWheelSpin();
		TractionControl();
	}
	#endregion public functions

	#region private functions
	private void ApplyDrive (float accel, float footbrake) {
		float thrustTorque;
		switch (m_CarDriveType)
		{
		case CarDriveType.FourWheelDrive:
			thrustTorque = accel * (currentTorque / 4f);
			for (int i = 0; i < 4; i++)
			{
				wheelColliders[i].motorTorque = thrustTorque;
			}
			break;

		case CarDriveType.FrontWheelDrive:
			thrustTorque = accel * (currentTorque / 2f);
			wheelColliders[0].motorTorque = wheelColliders[1].motorTorque = thrustTorque;
			break;

		case CarDriveType.RearWheelDrive:
			thrustTorque = accel * (currentTorque / 2f);
			wheelColliders[2].motorTorque = wheelColliders[3].motorTorque = thrustTorque;
			break;

		}

		for (int i = 0; i < 4; i++)
		{
			if (CurrentSpeed > 5 && Vector3.Angle(transform.forward, mRigidbody.velocity) < 50f)
			{
				wheelColliders[i].brakeTorque = brakeTorque*footbrake;
			}
			else if (footbrake > 0)
			{
				wheelColliders[i].brakeTorque = 0f;
				wheelColliders[i].motorTorque = -reverseTorque*footbrake;
			}
		}	}

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

	private void GearChanging()
	{
		float f = Mathf.Abs(CurrentSpeed/MaxSpeed);
		float upgearlimit = (1/(float) NoOfGears)*(m_GearNum + 1);
		float downgearlimit = (1/(float) NoOfGears)*m_GearNum;

		if (m_GearNum > 0 && f < downgearlimit)
		{
			m_GearNum--;
		}

		if (f > upgearlimit && (m_GearNum < (NoOfGears - 1)))
		{
			m_GearNum++;
		}
	}

	// simple function to add a curved bias towards 1 for a value in the 0-1 range
	private static float CurveFactor(float factor)
	{
		return 1 - (1 - factor)*(1 - factor);
	}

	// unclamped version of Lerp, to allow value to exceed the from-to range
	private static float ULerp(float from, float to, float value)
	{
		return (1.0f - value)*from + value*to;
	}

	private void CalculateGearFactor()
	{
		float f = (1/(float) NoOfGears);
		// gear factor is a normalised representation of the current speed within the current gear's range of speeds.
		// We smooth towards the 'target' gear factor, so that revs don't instantly snap up or down when changing gear.
		var targetGearFactor = Mathf.InverseLerp(f*m_GearNum, f*(m_GearNum + 1), Mathf.Abs(CurrentSpeed/MaxSpeed));
		m_GearFactor = Mathf.Lerp(m_GearFactor, targetGearFactor, Time.deltaTime*5f);
	}

	private void CalculateRevs()
	{
		// calculate engine revs (for display / sound)
		// (this is done in retrospect - revs are not used in force/power calculations)
		CalculateGearFactor();
		var gearNumFactor = m_GearNum/(float) NoOfGears;
		var revsRangeMin = ULerp(0f, revRangeBoundary, CurveFactor(gearNumFactor));
		var revsRangeMax = ULerp(revRangeBoundary, 1f, gearNumFactor);
		Revs = ULerp(revsRangeMin, revsRangeMax, m_GearFactor);
	}

	private void CapSpeed()
	{
		float speed = mRigidbody.velocity.magnitude;
		switch (mSpeedType)
		{
		case SpeedType.MPH:

			speed *= 2.23693629f;
			if (speed > topspeed)
				mRigidbody.velocity = (topspeed/2.23693629f) * mRigidbody.velocity.normalized;
			break;

		case SpeedType.KPH:
			speed *= 3.6f;
			if (speed > topspeed)
				mRigidbody.velocity = (topspeed/3.6f) * mRigidbody.velocity.normalized;
			break;
		}
	}

	// this is used to add more grip in relation to speed
	private void AddDownForce()
	{
		wheelColliders[0].attachedRigidbody.AddForce(-transform.up*downforce*
			wheelColliders[0].attachedRigidbody.velocity.magnitude);
	}

	// checks if the wheels are spinning and is so does three things
	// 1) emits particles
	// 2) plays tiure skidding sounds
	// 3) leaves skidmarks on the ground
	// these effects are controlled through the WheelEffects class
//	private void CheckForWheelSpin()
//	{
//		// loop through all wheels
//		for (int i = 0; i < 4; i++)
//		{
//			WheelHit wheelHit;
//			wheelColliders[i].GetGroundHit(out wheelHit);
//
//			// is the tire slipping above the given threshhold
//			if (Mathf.Abs(wheelHit.forwardSlip) >= m_SlipLimit || Mathf.Abs(wheelHit.sidewaysSlip) >= m_SlipLimit)
//			{
//				m_WheelEffects[i].EmitTyreSmoke();
//
//				// avoiding all four tires screeching at the same time
//				// if they do it can lead to some strange audio artefacts
//				if (!AnySkidSoundPlaying())
//				{
//					m_WheelEffects[i].PlayAudio();
//				}
//				continue;
//			}
//
//			// if it wasnt slipping stop all the audio
//			if (m_WheelEffects[i].PlayingAudio)
//			{
//				m_WheelEffects[i].StopAudio();
//			}
//			// end the trail generation
//			m_WheelEffects[i].EndSkidTrail();
//		}
//	}

	// crude traction control that reduces the power to wheel if the car is wheel spinning too much
	private void TractionControl()
	{
		WheelHit wheelHit;
		switch (m_CarDriveType)
		{
		case CarDriveType.FourWheelDrive:
			// loop through all wheels
			for (int i = 0; i < 4; i++)
			{
				wheelColliders[i].GetGroundHit(out wheelHit);

				AdjustTorque(wheelHit.forwardSlip);
			}
			break;

		case CarDriveType.RearWheelDrive:
			wheelColliders[2].GetGroundHit(out wheelHit);
			AdjustTorque(wheelHit.forwardSlip);

			wheelColliders[3].GetGroundHit(out wheelHit);
			AdjustTorque(wheelHit.forwardSlip);
			break;

		case CarDriveType.FrontWheelDrive:
			wheelColliders[0].GetGroundHit(out wheelHit);
			AdjustTorque(wheelHit.forwardSlip);

			wheelColliders[1].GetGroundHit(out wheelHit);
			AdjustTorque(wheelHit.forwardSlip);
			break;
		}
	}

	private void AdjustTorque(float forwardSlip)
	{
		if (forwardSlip >= m_SlipLimit && currentTorque >= 0)
		{
			currentTorque -= 10 * m_TractionControl;
		}
		else
		{
			currentTorque += 10 * m_TractionControl;
			if (currentTorque > fullTorqueOverAllWheels)
			{
				currentTorque = fullTorqueOverAllWheels;
			}
		}
	}

//	private bool AnySkidSoundPlaying()
//	{
//		for (int i = 0; i < 4; i++)
//		{
//			if (m_WheelEffects[i].PlayingAudio)
//			{
//				return true;
//			}
//		}
//		return false;
//	}
	#endregion private functions

	#region event subscribers
	private void HandleStartGame (bool _state) {
		isStart = _state;
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