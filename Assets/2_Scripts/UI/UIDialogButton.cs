﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UIDialogButton
{
	public enum Anchor
	{
		BOTTOM_RIGHT,
		BOTTOM_LEFT,
		BOTTOM_CENTER,
		CENTER_LEFT,
		CENTER_RIGHT,
		CENTER
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
		else if(_anchor == Anchor.CENTER_LEFT)
		{	
			rectTrans.anchorMax = Vector2.up / 2;
			rectTrans.anchorMin = Vector2.up / 2;
			rectTrans.pivot = Vector2.up / 2;
		}
		else if (_anchor == Anchor.CENTER_RIGHT)
		{
			Vector2 anchor = new Vector2 (1f, 0.5f);
			rectTrans.anchorMax = anchor;
			rectTrans.anchorMin = anchor;
			rectTrans.pivot = anchor;
		}
		else if(_anchor == Anchor.CENTER)
		{
			rectTrans.anchorMax = Vector2.one / 2;
			rectTrans.anchorMin = Vector2.one / 2;
			rectTrans.pivot = Vector2.one / 2;
		}
	}

	public UIDialogButton (string buttonText, Anchor anchor, Callback callback = null)
	{
		_buttonText = buttonText;
		_anchor = anchor;
		_callback = callback;
	}

	public UIDialogButton (string buttonText, Anchor anchor, float paddingX, float paddingY, Callback callback) : this (buttonText, anchor, callback)
	{
		_padding = new Padding(paddingX, paddingY);
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
		if (_callback != null)
			button.onClick.AddListener (() => {
				_callback.Invoke ();
			});
		
		button.onClick.AddListener (() => {
			button.gameObject.transform.parent.gameObject.SetActive (false);
		});

	}

	private class Padding
	{
		public float Vertical;
		public float Horizontal;

		public Padding (float horizontal, float vertical)
		{
			Vertical = vertical;
			Horizontal = horizontal;
		}
	}
}
