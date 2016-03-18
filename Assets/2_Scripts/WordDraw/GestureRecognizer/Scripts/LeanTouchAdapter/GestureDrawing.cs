using UnityEngine;
using System.Collections.Generic;
using PDollarGestureRecognizer;

public abstract class GestureDrawing : MonoBehaviour
{
	public LeanGestureRecognizer recognizer;
	public int maxPoints = 1000;
	public int minPixelMove = 5;
	// Must move at least this many pixels per sample for a new segment to be recorded

	public delegate void OnStrokeStartEvent (Lean.LeanFinger finger);

	public static event OnStrokeStartEvent OnStrokeStart;

	public delegate void OnStrokeDragEvent (Lean.LeanFinger finger);

	public static event OnStrokeDragEvent OnStrokeDrag;

	public delegate void OnStrokeEndEvent (Lean.LeanFinger finger);

	public static event OnStrokeEndEvent OnStrokeEnd;

	void OnEnable ()
	{
		Lean.LeanTouch.OnFingerDown += OnFingerDown;
		Lean.LeanTouch.OnFingerUp += OnFingerUp;
		Lean.LeanTouch.OnFingerDrag += OnFingerDrag;
	}

	void OnDisable ()
	{
		Lean.LeanTouch.OnFingerDown -= OnFingerDown;
		Lean.LeanTouch.OnFingerUp -= OnFingerUp;
		Lean.LeanTouch.OnFingerDrag -= OnFingerDrag;
	}

	private void OnFingerDown (Lean.LeanFinger finger)
	{
		if (recognizer.IsReachMaxStroke)
			return;

		if (OnStrokeStart != null)
			OnStrokeStart (finger);
		
		StrokeStart (finger);
	}

	private void OnFingerDrag (Lean.LeanFinger finger)
	{
		if (recognizer.IsReachMaxStroke)
			return;

		if (OnStrokeDrag != null)
			OnStrokeDrag (finger);
		
		StrokeDrag (finger);
	}

	private void OnFingerUp (Lean.LeanFinger finger)
	{
		if (recognizer.IsReachMaxStroke)
			return;
		
		if (OnStrokeEnd != null)
			OnStrokeEnd (finger);
		
		StrokeEnd (finger);
	}

	protected abstract void StrokeStart (Lean.LeanFinger finger);

	protected abstract void StrokeDrag (Lean.LeanFinger finger);

	protected abstract void StrokeEnd (Lean.LeanFinger finger);
}
