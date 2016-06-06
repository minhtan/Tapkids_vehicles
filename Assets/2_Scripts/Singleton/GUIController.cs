using UnityEngine;
using System.Collections;

public class GUIController : UnitySingletonPersistent<GUIController>
{
	[SerializeField]
	private UIDialog _dialog;	

	public Sprite _sprite;

	public UIDialog OpenDialog (string message, params Sprite[] uiSprites)
	{
		_dialog.gameObject.SetActive (true);
		_dialog.RefreshDialog ();
		_dialog.SetMessageText (message, uiSprites);
		return _dialog;
	}

	void Awake ()
	{
		base.Awake ();
	}
}
