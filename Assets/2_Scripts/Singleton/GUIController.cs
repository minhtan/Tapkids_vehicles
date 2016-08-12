using UnityEngine;
using System.Collections;

public class GUIController : UnitySingletonPersistent<GUIController>
{
	[SerializeField]
	private UIDialog _dialog;	

	public UIDialog OpenDialog (string message, params Sprite[] uiSprites)
	{
		if(!_dialog.gameObject.activeInHierarchy)
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
