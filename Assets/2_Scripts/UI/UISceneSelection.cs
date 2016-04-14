using UnityEngine;
using System.Collections;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class UISceneSelection : MonoBehaviour {

	public SceneController.SceneID _selectedSceneID;

	private Button _button;

	void Awake()
	{
		_button = GetComponent<Button> ();
		_button.onClick.AddListener (delegate {
			SceneController.Instance.LoadingSceneAsync(_selectedSceneID);
		});
	}
}
