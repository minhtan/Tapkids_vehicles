using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UnityStandardAssets.CrossPlatformInput
{
	public class CarRemoteControl : MonoBehaviour
	{
		[SerializeField] private string steerInputName = "Steer"; // The name given to the horizontal axis for the cross platform input
		[SerializeField] private string accelerateInputName = "Accelerate";

		private CrossPlatformInputManager.VirtualAxis mSteer; // Reference to the joystick in the cross platform input
		private CrossPlatformInputManager.VirtualAxis mAccelerate; // Reference to the joystick in the cross platform input

		private Vector2 centerOfScreen;
		private bool leftTouched;
		private bool rightTouched;

		#region Mono
		void OnEnable()
		{
			CarGameEventController.StartGame += OnStartGame;
			CarGameEventController.PauseGame += OnPauseGame;
			CarGameEventController.ResetGame += OnResetGame;

			CreateVirtualAxes();	
		}

		void Start () {
			centerOfScreen = new Vector3 (Screen.width / 2, Screen.height / 2);
		}

		void Update () { 
			if (Input.touchCount > 0) {
				for (int i = 0; i < Input.touchCount; i++) {
					if (Input.touches[i].position.x > centerOfScreen.x) {
						Touch touch = Input.touches[i];
						switch (touch.phase) {
						case TouchPhase.Began:
							rightTouched = true;
							break;
						case TouchPhase.Ended:
							rightTouched = false;
							break;
						}
					} else {
						Touch touch = Input.touches[i];
						switch (touch.phase) {
						case TouchPhase.Began:
							leftTouched = true;
							break;
						case TouchPhase.Ended:
							leftTouched = false;
							break;
						}
					}
				}
			}

			if (rightTouched == true && leftTouched == false) {
				mSteer.Update (1f);
				mAccelerate.Update (1f);
			} else if (rightTouched == false && leftTouched == true) {
				mSteer.Update (-1f);
				mAccelerate.Update (1f);
			} else if (rightTouched == true && leftTouched == true) {
				mSteer.Update (0f);
				mAccelerate.Update (1f);

			} else {
				mSteer.Update (0f);
				mAccelerate.Update (0f);
			}
		}

		void OnDisable()
		{
			CarGameEventController.StartGame -= OnStartGame;
			CarGameEventController.PauseGame -= OnPauseGame;
			CarGameEventController.ResetGame -= OnResetGame;

			if (CrossPlatformInputManager.AxisExists (steerInputName))
				CrossPlatformInputManager.UnRegisterVirtualAxis (steerInputName);

			if (CrossPlatformInputManager.AxisExists (accelerateInputName))
				CrossPlatformInputManager.UnRegisterVirtualAxis (accelerateInputName);
		}
		#endregion Mono

		#region private methods
		private void OnStartGame () {
			mAccelerate.Update (1f);
		}

		private void OnPauseGame (bool _isPaused) {
			if (_isPaused)
				mAccelerate.Update (0f);
		}

		private void OnResetGame () {
			mAccelerate.Update (0f);
		}

		private void CreateVirtualAxes()
		{
			// create new axes based on axes to use
			mSteer = new CrossPlatformInputManager.VirtualAxis (steerInputName);
			CrossPlatformInputManager.RegisterVirtualAxis (mSteer);

			mAccelerate = new CrossPlatformInputManager.VirtualAxis(accelerateInputName);
			CrossPlatformInputManager.RegisterVirtualAxis (mAccelerate);
		}

		private void UpdateVirtualAxes(Vector3 value)
		{
			value = value.normalized;
			mSteer.Update (value.x);
		}

		#endregion private methods



	}
}