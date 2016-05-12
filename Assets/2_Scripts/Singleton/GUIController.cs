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
		base.Awake ();
	}
}
