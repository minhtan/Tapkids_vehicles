using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIDialog : MonoBehaviour
{
	public Text _messageText;
	public GameObject _buttonTemplate;

	public void SetMessageText (string text)
	{
		_messageText.text = text;
	}

	public void CreateButtons (UIDialogButton[] buttons)
	{
		for (int i = 0; i < buttons.Length; i++) {
			RectTransform buttonTrans = Instantiate (_buttonTemplate).GetComponent<RectTransform> ();
			buttonTrans.SetParent (transform, false);
			buttons [i].SetAnchor (buttonTrans);
			buttons [i].SetPadding (buttonTrans);
			buttons [i].SetOnClickListener (buttonTrans.GetComponent<Button> ());
			buttons [i].SetButtonText (buttonTrans.GetChild (0).GetComponent<Text> ());
		}
	}

	
}
