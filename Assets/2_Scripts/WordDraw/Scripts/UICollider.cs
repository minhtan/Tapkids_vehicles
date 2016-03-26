using UnityEngine;
using System.Collections;

namespace WordDraw
{
	[RequireComponent (typeof(BoxCollider2D))]
	public class UICollider : MonoBehaviour
	{
		public RectTransform _targetPivot;
		public float scaleFactor = 1f;

		public bool width;
		public bool height;

		private BoxCollider2D _collider;

		// Use this for initialization
		void Start ()
		{
			BoxCollider2D collider = GetComponent<BoxCollider2D> ();
			Vector2 rectSize = collider.size;
			
			if (width)
				rectSize.x = _targetPivot.sizeDelta.x * scaleFactor;

			if (height)
				rectSize.y = _targetPivot.sizeDelta.y * scaleFactor;
		

			collider.size = rectSize;
		}
	}
}
