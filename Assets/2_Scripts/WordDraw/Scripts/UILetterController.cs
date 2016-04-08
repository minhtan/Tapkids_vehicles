using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections.Generic;

public class UILetterController : MonoBehaviour
{
	public ScrollRect _scrollRect;
	public RectTransform _letterHolderTrans;
	public int _adjacentLetterNum = 3;
	public float _explodeForce = 200f;
	public float _moveToCenterDuration = 1f;

	private UILetterButton[] _uiLetter;
	private List<UILetterButton> _flyLetterList;
	private RectTransform _rectTrans;
	private int _curIndex;

	public UILetterButton CurrentButton {
		get{ return _uiLetter [_curIndex]; }
	}

	public static UnityAction<UILetterButton> OnMoveToCenterDone;

	void OnEnable ()
	{
		UILetterButton.OnLetterClickEvent += OnLetterClickEvent;
	}

	void OnDisable ()
	{
		UILetterButton.OnLetterClickEvent -= OnLetterClickEvent;
	}

	// Use this for initialization
	void Awake ()
	{
		_uiLetter = GetComponentsInChildren<UILetterButton> ();
		_rectTrans = GetComponent<RectTransform> ();
		_flyLetterList = new List<UILetterButton> (_adjacentLetterNum * 2 + 1);
	}

	public void SetActiveScroll(bool active)
	{
		_scrollRect.enabled = active;
	}

	public void ResetLettersPosition ()
	{
		foreach (UILetterButton uiLetter in _flyLetterList) {
			uiLetter.ReturnToOriginalPosition ();
		}

		_flyLetterList.Clear ();
	}

	private void AnimateLetters (int pivotIndex)
	{
		int fromIndex = pivotIndex - _adjacentLetterNum;
		fromIndex = Mathf.Clamp (fromIndex, 0, _uiLetter.Length - 1);

		int endIndex = pivotIndex + _adjacentLetterNum;
		endIndex = Mathf.Clamp (endIndex, 0, _uiLetter.Length - 1);

		for (int i = fromIndex; i <= endIndex; i++) {
			if (i == pivotIndex)
				continue;

			if (i < pivotIndex)
				_uiLetter [i].Explode (new Vector2 (-1f, 2f), _explodeForce);
			else
				_uiLetter [i].Explode (new Vector2 (1f, 2f), _explodeForce);

			_flyLetterList.Add (_uiLetter [i]);
		}
	}

	private void OnLetterClickEvent (UILetterButton clickedLetter)
	{
		_curIndex = clickedLetter.AlphabetIndex;

		AnimateLetters (_curIndex);
		ScrollToCenter (_curIndex);
		LockScrollView (_curIndex);
	}

	private void ScrollToCenter (int index)
	{
		RectTransform targetLetterTrans = _uiLetter [index].RectTrans;
		float difX = targetLetterTrans.position.x - _letterHolderTrans.position.x;

		Vector3 newContentPosition = _rectTrans.position;
		newContentPosition.x -= difX;

		StartCoroutine (MoveToCenterCor (_rectTrans, newContentPosition, _moveToCenterDuration));
	}

	private void LockScrollView (int index)
	{
		_scrollRect.enabled = false;
		_uiLetter [index].DisableButton ();
	}


	private IEnumerator MoveToCenterCor (RectTransform contentRect, Vector3 destination, float duration)
	{
		float timer = duration;
		float progress = 0f;

		Vector3 startPos = contentRect.position;

		while (true) {
			timer -= Time.deltaTime;

			progress = Mathf.InverseLerp (duration, 0f, timer);

			contentRect.position = Vector3.Lerp (startPos, destination, progress);

			if (progress == 1f) {

				if (OnMoveToCenterDone != null)
					OnMoveToCenterDone (_uiLetter [_curIndex]);
				
				yield break;
			}

			yield return null;
		}
	}
}
