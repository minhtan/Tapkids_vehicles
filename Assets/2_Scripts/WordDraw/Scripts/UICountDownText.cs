using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Events;

public class UICountDownText : MonoBehaviour
{
	public float _totalTime;
	private Text _text;

	public static UnityAction OnEndCountDown;

	void Awake ()
	{
		_text = GetComponent<Text> ();
		StartCoroutine (CountDownCor(_totalTime));
	}

	IEnumerator CountDownCor (float totalTime)
	{
		while (true) {
			totalTime -= Time.deltaTime;
			_text.text = "" + (int)totalTime;

			if (totalTime < 0) {
				_text.enabled = false;

				if (OnEndCountDown != null)
					OnEndCountDown ();
				yield break;
			}

			yield return null;
		}
	}
}
