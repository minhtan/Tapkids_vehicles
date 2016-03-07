 using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UnityStandardAssets.CrossPlatformInput
{
	[RequireComponent(typeof(Image))]
	public class SteerInput : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
	{
		public string horizontalAxisName = "Horizontal"; // The name given to the horizontal axis for the cross platform input
		public string verticalAxisName = "Vertical";

		CrossPlatformInputManager.VirtualAxis m_HorizontalVirtualAxis; // Reference to the joystick in the cross platform input
		CrossPlatformInputManager.VirtualAxis m_VerticalVirtualAxis; // Reference to the joystick in the cross platform input

		Image image;
		Vector3 center;

		void Start () {
			image = GetComponent<Image> ();
			center = image.transform.position;
		}

		void OnEnable()
		{
			CreateVirtualAxes();	
		}

		void CreateVirtualAxes()
		{
			// create new axes based on axes to use
			m_HorizontalVirtualAxis = new CrossPlatformInputManager.VirtualAxis(horizontalAxisName);
			CrossPlatformInputManager.RegisterVirtualAxis(m_HorizontalVirtualAxis);

			m_VerticalVirtualAxis = new CrossPlatformInputManager.VirtualAxis(verticalAxisName);
			CrossPlatformInputManager.RegisterVirtualAxis(m_VerticalVirtualAxis);
		}

		void UpdateVirtualAxes(Vector3 value)
		{
			value = value.normalized;
			m_HorizontalVirtualAxis.Update (value.x);
			m_VerticalVirtualAxis.Update (1f);
		}


		public void OnPointerDown(PointerEventData data)
		{
			if(data.position.x > center.x)
				UpdateVirtualAxes (Vector3.right);
			else
				UpdateVirtualAxes (Vector3.left);
		}

		public void OnPointerUp(PointerEventData data)
		{
			UpdateVirtualAxes (Vector3.zero);
		}

		void OnDisable()
		{
			if (CrossPlatformInputManager.AxisExists(horizontalAxisName))
				CrossPlatformInputManager.UnRegisterVirtualAxis(horizontalAxisName);
		}
	}
}