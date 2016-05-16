using UnityEngine;
using System.Collections;

public class GUIController : UnitySingletonPersistent<GUIController>
{
	[SerializeField]
	private UIDialog _dialog;

	[System.Obsolete ("Obsolete constructor. Use chaining method to create dialog instead.")]
	public void OpenDialog (string message, params UIDialogButton[] buttons)
	{
		_dialog.gameObject.SetActive (true);
		_dialog.RefreshDialog ();
		_dialog.SetMessageText (message);
		_dialog.CreateButtons (buttons);
	}

	public UIDialog OpenDialog (string message)
	{
		_dialog.gameObject.SetActive (true);
		_dialog.RefreshDialog ();
		_dialog.SetMessageText (message);
		return _dialog;
	}

	void Awake ()
	{
		base.Awake ();
	}
}
