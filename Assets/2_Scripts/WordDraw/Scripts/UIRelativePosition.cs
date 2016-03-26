using UnityEngine;
using System.Collections;

namespace WordDraw
{
	public class UIRelativePosition : MonoBehaviour
	{

		public RectTransform _targetPivot;
		public float scaleFactor = 1f;

		public bool width;
		public bool height;

		// Use this for initialization
		void Start ()
		{
			RectTransform rectTrans = GetComponent<RectTransform> ();

			Vector2 rectSize = Vector2.zero;

			if (width)
				rectSize.x = _targetPivot.sizeDelta.x * scaleFactor;

			if (height)
				rectSize.y = _targetPivot.sizeDelta.y * scaleFactor;
		
			rectTrans.sizeDelta = rectSize;
		}
	}
}
