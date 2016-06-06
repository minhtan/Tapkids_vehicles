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
	private Text _sizeCalculator;

	void Awake ()
	{
		_messageText = transform.GetChild (0).GetComponent<Text> ();
		_sizeCalculator = transform.GetChild (1).GetComponent<Text> ();
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

	public void SetMessageText (string text, params Sprite[] uiSprites)
	{
		_messageText.text = text;
	
		if (uiSprites.Length <= 0)
			return;

		int textLength = text.Length;
		int spriteCount = 0;

		for (int i = 0; i < textLength; i++) {
			if (text [i] != '%')
				continue;

			if (i > textLength / 2)
				CreateImage (i, text.Substring (textLength / 2, i - textLength / 2), Vector2.right, uiSprites [spriteCount]);
			else
				CreateImage (i, text.Substring (i, textLength / 2 - i), Vector2.left, uiSprites [spriteCount]);
			
			spriteCount++;
		}
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

		float pixelCenterOffset = GetTextLength (gapStringFromCenter, offsetSide);
		Debug.Log (pixelCenterOffset);
		Vector2 offsetX = pixelCenterOffset * offsetSide;

		Vector2 pos = rectTrans.localPosition;
		pos = pos + offsetX;
		rectTrans.localPosition = pos;
	}

	public void RefreshDialog ()
	{
		for (int i = 2; i < transform.childCount; i++) {
			Destroy (transform.GetChild (i).gameObject);
		}
	}

	private float GetTextLength (string text, Vector2 offset)
	{
		_sizeCalculator.fontSize = _messageText.fontSize;
		_sizeCalculator.text = text;
		ContentSizeFitter sizeFilter = _sizeCalculator.gameObject.GetComponent<ContentSizeFitter> ();
		sizeFilter.SetLayoutHorizontal ();
		sizeFilter.SetLayoutVertical ();
		return _sizeCalculator.rectTransform.sizeDelta.x + offset.x * 15f;
	}
}
