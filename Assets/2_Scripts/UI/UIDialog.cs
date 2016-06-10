using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class UIDialog : MonoBehaviour
{
	[SerializeField]
	private GameObject _buttonTemplate;

	[SerializeField]
	private GameObject _imageTemplate;

	private Text _messageText;
	/// <summary>
	/// The Text that has ContentSizeFitter used for messure text size in pixel.
	/// </summary>
	private Text _sizeCalculator;

	private static char _characterImage = '*';
	private float _charImageWith;
	private float _textSizePixelX;

	public static char CharacterImage {
		get{ return _characterImage; }
	}

	void Awake ()
	{
		_messageText = transform.GetChild (0).GetComponent<Text> ();
		_sizeCalculator = transform.GetChild (1).GetComponent<Text> ();

		_charImageWith = GetTextLength (_characterImage.ToString ());
	}

	public UIDialog AddButton (string text, UIDialogButton.Anchor anchor, Callback callback = null)
	{
		CreateButton (new UIDialogButton (text, anchor, callback));
		return this;
	}

	public UIDialog AddButton (string text, UIDialogButton.Anchor anchor, float paddingX, float paddingY, Callback callback = null)
	{
		CreateButton (new UIDialogButton (text, anchor, paddingX, paddingY, callback));
		return this;
	}

	public void setCharacterImage (char characterImage)
	{
		_characterImage = characterImage;
	}

	/// <summary>
	/// Sets the message text and replace ImageChar with image.
	/// </summary>
	/// <param name="text">Text.</param>
	/// <param name="uiSprites">Array of sprites that replace ImageChar characters.</param>
	public void SetMessageText (string text, params Sprite[] uiSprites)
	{
		_messageText.text = text;
	
		if (uiSprites.Length <= 0)
			return;

		int textHalfLength = text.Length / 2;
		int spriteCount = 0;

		_textSizePixelX = GetTextLength (text);

		for (int i = 0; i < text.Length; i++) {
			if (text [i] != _characterImage)
				continue;

			if (i > textHalfLength)
				CreateImage (i, text.Substring (textHalfLength + 1, i - textHalfLength), Vector2.right, uiSprites [spriteCount]);
			else
				CreateImage (i, text.Substring (i + 1, textHalfLength - i), Vector2.left, uiSprites [spriteCount]);
			
			spriteCount++;
		}

		//_messageText.text = text.Replace (CharacterImage, ' ');
	}

	private void CreateButton (UIDialogButton dialogBut)
	{
		RectTransform buttonTrans = Instantiate (_buttonTemplate).GetComponent<RectTransform> ();
		buttonTrans.SetParent (transform, false);
		dialogBut.SetAnchor (buttonTrans);
		dialogBut.SetPadding (buttonTrans);
		dialogBut.SetOnClickListener (buttonTrans.GetComponent<Button> ());
		dialogBut.SetButtonText (buttonTrans.GetChild (0).GetComponent<Text> ());
	}

	/// <summary>
	/// Creates the image ui object and set its size, position.
	/// </summary>
	/// <param name="insertIndex">The position index of image in string.</param>
	/// <param name="gapStringFromCenter">String that is between center char and image char.</param>
	/// <param name="offsetSide">Determine the image is left or right of center.</param>
	/// <param name="offsetScalar">Determine how much the image is move from position.</param>
	/// <param name="sprite">Sprite that replaces image char.</param>
	private void CreateImage (int insertIndex, string gapStringFromCenter, Vector2 offsetSide, Sprite sprite)
	{
		GameObject imageGO = Instantiate (_imageTemplate) as GameObject;

		Image image = imageGO.GetComponent<Image> ();
		image.sprite = sprite;

		RectTransform rectTrans = imageGO.GetComponent<RectTransform> ();
		rectTrans.SetParent (_messageText.transform, false);
		rectTrans.localPosition = Vector2.zero;

		int imageSize = _messageText.fontSize;
		rectTrans.sizeDelta = new Vector2 (imageSize, imageSize);

		float pixelCenterOffset = GetTextLength (gapStringFromCenter);
		Vector2 offsetX = pixelCenterOffset * offsetSide;

		string firstHalf = _messageText.text.Substring (0, _messageText.text.Length / 2 + 1);

		float halfLengthPixel = GetTextLength (firstHalf);

		offsetX.x = offsetX.x - _textSizePixelX / 2 + halfLengthPixel;

		Vector2 pos = Vector2.zero;
		pos = pos + offsetX;
		pos.x = pos.x - _charImageWith / 2;
		
		rectTrans.localPosition = pos;
	}

	/// <summary>
	/// Remove buttons that were created before
	/// </summary>
	public void RefreshDialog ()
	{
		for (int i = 2; i < transform.childCount; i++) {
			Destroy (transform.GetChild (i).gameObject);
		}
	}

	/// <summary>
	/// Gets the length of a text using ContentSizeFitter component.
	/// </summary>
	/// <returns>The text length.</returns>
	/// <param name="text">Text.</param>
	/// <param name="offset">Offset vector position to left (-1,0) or right (1,0).</param>
	/// <param name="scalar">Offset scalar.</param>
	private float GetTextLength (string text)
	{
		_sizeCalculator.fontSize = _messageText.fontSize;
		_sizeCalculator.text = text;
		ContentSizeFitter sizeFilter = _sizeCalculator.gameObject.GetComponent<ContentSizeFitter> ();
		sizeFilter.SetLayoutHorizontal ();
		sizeFilter.SetLayoutVertical ();
		return _sizeCalculator.rectTransform.sizeDelta.x;
	}
}
