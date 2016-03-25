using UnityEngine;
using System.Collections;

namespace WordDraw
{
	[RequireComponent (typeof(BoxCollider2D))]
	public class UILetter : MonoBehaviour
	{
		[SerializeField]
		private Letters letter;

		private BoxCollider2D _collider;
		private RectTransform _rectTrans;

		public Letters Letter{ get { return letter; } }


		// Use this for initialization
		void Awake ()
		{
			_collider = GetComponent<BoxCollider2D> ();    
			_rectTrans = GetComponent<RectTransform> ();
			SetupLetter ();
		}


		private void SetupLetter ()
		{
			Vector2 size = Vector2.zero;
			size.x = _rectTrans.rect.width;
			size.y = _rectTrans.rect.height;

			_collider.size = size;
		}

		public void SetOffset (Vector2 offset)
		{
			_collider.offset = offset;
		}
	}
}