using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Events;
using WordDraw;

public class UILetterButton : MonoBehaviour
{

	[SerializeField]
	private Letters _letter;

	[SerializeField]
	private float _flyAfterExplodeTime = 1f;

	private Rigidbody2D _body;
	private RectTransform _rectTrans;
	private Vector3 _originalPosition;
	private Button _button;

	public Letters Letter {
		get{ return _letter; }
	}

	public Letters AlphabetLetter { 
		set { _letter = value; }
		get { return _letter; }
	}

	public int AlphabetIndex {
		get{ return (int)_letter; }
	}

	public Vector3 OriginalPosition{ set { _originalPosition = value; } get { return _originalPosition; } }

	public RectTransform RectTrans{ get { return _rectTrans; } }

	public static UnityAction<UILetterButton> OnLetterClickEvent;
	public static UnityAction<UILetterButton> OnLetterResetDone;

	// Use this for initialization
	void Awake ()
	{
		_rectTrans = GetComponent<RectTransform> ();
		_body = GetComponent<Rigidbody2D> ();
		_button = GetComponent<Button> ();
	}

	void Start ()
	{
		_originalPosition = _rectTrans.localPosition;
	}

	public void OnClick ()
	{
		if (OnLetterClickEvent != null)
			OnLetterClickEvent (this);
	}

	public void Explode (Vector2 direction, float magnitude)
	{
		_body.isKinematic = false;
		_body.AddForce (direction * magnitude, ForceMode2D.Impulse);
		_body.AddTorque (Random.value < 0.5f ? 300f : -300f);

		StartCoroutine (StopFlyCor (_flyAfterExplodeTime));
	}

	public void ReturnToOriginalPosition ()
	{
		_rectTrans.localRotation = Quaternion.identity;
		StartCoroutine (ResetPositionCor (_flyAfterExplodeTime));
	}

	public void DisableButton ()
	{
		_button.enabled = false;
	}

	private IEnumerator StopFlyCor (float duration)
	{
		yield return new WaitForSeconds (duration);
		_body.isKinematic = true;
	}

	private IEnumerator ResetPositionCor (float duration)
	{
		float timer = duration;
		float progress = 0f;

		Vector3 curPos = _rectTrans.localPosition;
		
		while (true) {
			timer -= Time.deltaTime;
			progress = Mathf.InverseLerp (duration, 0f, timer);

			_rectTrans.localPosition = Vector3.Lerp (curPos, _originalPosition, progress);

			if (progress == 1f) {

				if (OnLetterResetDone != null)
					OnLetterResetDone (this);
				
				yield break;
			}
			
			yield return null;
		}
	}
}
