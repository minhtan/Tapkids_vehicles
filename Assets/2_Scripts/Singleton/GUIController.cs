using UnityEngine;
using System.Collections;

public class GUIController : UnitySingletonPersistent<GUIController>
{

	public UIDialog _dialog;

	public void OpenDialog (string message, params UIDialogButton[] buttons)
	{
		_dialog.gameObject.SetActive (true);
		_dialog.SetMessageText (message);
		_dialog.CreateButtons (buttons);
	}

	void Awake ()
	{
		OpenDialog ("Mot loi vo cung nghiem trong da xay ra. De nghi nap tien de khac phuc!!!", new UIDialogButton ("Yes", UIDialogButton.Anchor.BOTTOM_CENTER, new Callback (delegate {
			Debug.Log ("Say Yes");
		})), new UIDialogButton ("Yes", UIDialogButton.Anchor.BOTTOM_LEFT, new Callback (delegate {
			Debug.Log ("Say Yes");
		})), new UIDialogButton ("Yes", UIDialogButton.Anchor.BOTTOM_RIGHT, new Callback (delegate {
			Debug.Log ("Say Yes");
		})));
	}
}
