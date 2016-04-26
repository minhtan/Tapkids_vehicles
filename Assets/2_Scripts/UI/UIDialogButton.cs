using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UIDialogButton
{
	public enum Anchor
	{
		BOTTOM_RIGHT,
		BOTTOM_LEFT,
		BOTTOM_CENTER
	}

	private string _buttonText;
	private Callback _callback;
	private Anchor _anchor;
	private Padding _padding;


	public void SetAnchor (RectTransform rectTrans)
	{
		if (_anchor == Anchor.BOTTOM_CENTER) {
			rectTrans.anchorMax = new Vector2 (0.5f, 0f);
			rectTrans.anchorMin = rectTrans.anchorMax;
			rectTrans.pivot = rectTrans.anchorMax;
		} else if (_anchor == Anchor.BOTTOM_LEFT) {
			rectTrans.anchorMax = Vector2.zero;
			rectTrans.anchorMin = Vector2.zero;
			rectTrans.pivot = Vector2.zero;
		} else if (_anchor == Anchor.BOTTOM_RIGHT) {
			rectTrans.anchorMax = Vector2.right;
			rectTrans.anchorMin = Vector2.right;
			rectTrans.pivot = Vector2.right;
		}
	}

	public UIDialogButton (string buttonText, Anchor anchor, Callback callback)
	{
		_buttonText = buttonText;
		_callback = callback;
		_anchor = anchor;
	}

	public UIDialogButton (string buttonText, Anchor anchor, Padding padding, Callback callback) : this (buttonText, anchor, callback)
	{
		_padding = padding;
	}

	public void SetPadding (RectTransform rectTrans)
	{
		if (_padding == null)
			return;
		
		Vector3 position = rectTrans.localPosition;

		position.x += _padding.Horizontal;
		position.y += _padding.Vertical;

		rectTrans.localPosition = position;
	}

	public void SetButtonText (Text text)
	{
		text.text = _buttonText;
	}

	public void SetOnClickListener (Button button)
	{	
		button.onClick.AddListener (() => {
			_callback.Invoke ();
		});
		button.onClick.AddListener (() => {
			button.gameObject.transform.parent.gameObject.SetActive (false);
		});
	}

	public class Padding
	{
		public float Vertical;
		public float Horizontal;

		public Padding (float vertical, float horizontal)
		{
			Vertical = vertical;
			Horizontal = horizontal;
		}
	}
}
