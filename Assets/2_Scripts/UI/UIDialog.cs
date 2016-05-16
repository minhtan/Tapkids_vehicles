using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class UIDialog : MonoBehaviour
{
	[SerializeField]
	private GameObject _buttonTemplate;

	private Text _messageText;

	void Awake ()
	{
		_messageText = transform.GetChild (0).GetComponent<Text>();
	}
		
	public UIDialog AddButton (string text, UIDialogButton.Anchor anchor, Callback callback)
	{
		CreateButton (new UIDialogButton (text, anchor, callback));
		return this;
	}

	public UIDialog AddButton (string text, UIDialogButton.Anchor anchor, UIDialogButton.Padding padding, Callback callback)
	{
		CreateButton (new UIDialogButton (text, anchor, padding, callback));
		return this;
	}


	public void SetMessageText (string text)
	{
		_messageText.text = text;
	}

	public void CreateButtons (UIDialogButton[] buttons)
	{
		for (int i = 0; i < buttons.Length; i++) {
			CreateButton (buttons [i]);
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

	public void RefreshDialog ()
	{
		for (int i = 1; i < transform.childCount; i++) {
			Destroy (transform.GetChild (i).gameObject);
		}
	}
}
