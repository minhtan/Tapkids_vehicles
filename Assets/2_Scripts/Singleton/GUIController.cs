using UnityEngine;
using System.Collections;

public class GUIController : UnitySingletonPersistent<GUIController>
{
	[SerializeField]
	private UIDialog _dialog;	

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
