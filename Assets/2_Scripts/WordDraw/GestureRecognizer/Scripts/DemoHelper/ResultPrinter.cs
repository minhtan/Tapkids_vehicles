using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ResultPrinter : MonoBehaviour {

	private Text _resultText;

	void Awake()
	{
		_resultText = GetComponent <Text>();
	}
		
	void OnEnable()
	{
		LeanGestureRecognizer.OnGestureDetected += LeanGestureRecognizer_OnGestureDetected;
	}
		

	void OnDisable()
	{
		LeanGestureRecognizer.OnGestureDetected -= LeanGestureRecognizer_OnGestureDetected;
	}

	void LeanGestureRecognizer_OnGestureDetected (PDollarGestureRecognizer.Result result)
	{
		_resultText.text = result.GestureClass + " " + result.Score;
	}
}
