using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using PDollarGestureRecognizer;

public class TemplateAdder : MonoBehaviour {
	public LeanGestureRecognizer _recognizer;
	public InputField _inputField;

	public void AddTemplate()
	{
		_recognizer.SaveCurrentGesture (_inputField.text);
	}
}
