using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using PDollarGestureRecognizer;

public class TemplateAdder : MonoBehaviour {
	public LeanGestureRecognizer _recognizer;
	public InputField _inputField;

	private Result _result;

	void OnEnable()
	{
		LeanGestureRecognizer.OnGestureDetected += OnResult;
	}

	void OnDisable()
	{
		LeanGestureRecognizer.OnGestureDetected -= OnResult;
	}

	void OnResult(Result result)
	{
		_result = result;
	}

	public void AddTemplate()
	{
		_recognizer.SaveCurrentGesture (_inputField.text);
	}
}
