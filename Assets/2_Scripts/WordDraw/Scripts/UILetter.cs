using UnityEngine;
using System.Collections;

namespace WordDraw
{
	[RequireComponent (typeof(BoxCollider2D))]
	public class UILetter : MonoBehaviour
	{
		[SerializeField]
		private Letters letter;

		[SerializeField]
		private bool _autoConfig = true;

		private Vector2 _explodeDir;
		private Rigidbody2D _body;
		private BoxCollider2D _collider;
		private RectTransform _rectTrans;

		public Letters Letter{ get { return letter; } }


		// Use this for initialization
		void Awake ()
		{
			_collider = GetComponent<BoxCollider2D> ();    
			_rectTrans = GetComponent<RectTransform> ();
			_body = GetComponent<Rigidbody2D> ();
			_explodeDir = new Vector2 (1f, 0f);

			if (_autoConfig)
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

		public void DestroyLetter (int curDifficulty)
		{
			_collider.enabled = false;
			_body.AddForce (_explodeDir * 300f * (curDifficulty + 1), ForceMode2D.Impulse);
			Destroy (gameObject, 3f);
		}
	}
}