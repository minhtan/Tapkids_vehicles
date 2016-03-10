 using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UnityStandardAssets.CrossPlatformInput
{
	[RequireComponent(typeof(Image))]
	public class SteerInput : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
	{
		public string steerInputName = "Steer"; // The name given to the horizontal axis for the cross platform input
		public string accelerateInputName = "Accelerate";
		public string brakeInputName = "Brake";

		private bool isActiveInput;

		CrossPlatformInputManager.VirtualAxis m_Steer; // Reference to the joystick in the cross platform input
		CrossPlatformInputManager.VirtualAxis m_Accelerate; // Reference to the joystick in the cross platform input
		CrossPlatformInputManager.VirtualAxis m_Brake; // Reference to the joystick in the cross platform input

		Image image;
		Vector3 center;

		void Start () {
			image = GetComponent<Image> ();
			center = image.transform.position;
		}

		void OnEnable()
		{
			CarGameEventController.StartGame += OnStartGame;
			CarGameEventController.PauseGame += OnPauseGame;
			CarGameEventController.PauseGame += OnResetGame;

			CreateVirtualAxes();	
		}

		void OnStartGame () {
			isActiveInput = true;
			m_Accelerate.Update (1f);
		}
		void OnPauseGame () {
			isActiveInput = false;
			m_Accelerate.Update (0f);
		}
		void OnResetGame () {
			isActiveInput = true;
			m_Accelerate.Update (1f);

		}
		void CreateVirtualAxes()
		{
			// create new axes based on axes to use
			m_Steer = new CrossPlatformInputManager.VirtualAxis (steerInputName);
			CrossPlatformInputManager.RegisterVirtualAxis (m_Steer);

			m_Accelerate = new CrossPlatformInputManager.VirtualAxis(accelerateInputName);
			CrossPlatformInputManager.RegisterVirtualAxis (m_Accelerate);

			m_Brake = new CrossPlatformInputManager.VirtualAxis (brakeInputName);
			CrossPlatformInputManager.RegisterVirtualAxis (m_Brake);
		}

		void UpdateVirtualAxes(Vector3 value)
		{
			value = value.normalized;
			m_Steer.Update (value.x);
		}


		public void OnPointerDown(PointerEventData data)
		{
			if (!isActiveInput) return;

			if(data.position.x > center.x)
				UpdateVirtualAxes (Vector3.right);
			else
				UpdateVirtualAxes (Vector3.left);
		}

		public void OnPointerUp(PointerEventData data)
		{
			if (!isActiveInput) return;

			UpdateVirtualAxes (Vector3.zero);
		}

		void OnDisable()
		{
			CarGameEventController.StartGame -= OnStartGame;
			CarGameEventController.PauseGame -= OnPauseGame;
			CarGameEventController.PauseGame -= OnResetGame;

			if (CrossPlatformInputManager.AxisExists (steerInputName))
				CrossPlatformInputManager.UnRegisterVirtualAxis (steerInputName);

			if (CrossPlatformInputManager.AxisExists (accelerateInputName))
				CrossPlatformInputManager.UnRegisterVirtualAxis (accelerateInputName);

			if (CrossPlatformInputManager.AxisExists (brakeInputName))
				CrossPlatformInputManager.UnRegisterVirtualAxis (brakeInputName);
		}
	}
}