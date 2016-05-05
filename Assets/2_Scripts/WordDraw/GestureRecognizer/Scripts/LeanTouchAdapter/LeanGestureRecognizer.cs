using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using PDollarGestureRecognizer;
using UnityEngine.Events;
using System;
using UnityEngine.UI;
using UnityEngine.Events;


public abstract class LeanGestureRecognizer : MonoBehaviour
{
	[SerializeField]
	private int _maxStrokeNumber;

	[SerializeField]
	private bool _autoRecognize;

	[SerializeField]
	private bool _autoResetGesture;

	[SerializeField]
	private float _delayThreshold = 0.8f;

	private List<Gesture> _gestureList = new List<Gesture> ();
	private List<Point> _pointList = new List<Point> ();

	private Gesture _currentGesture;

	public Gesture CurrentGesture { get { return _currentGesture; } }

	public List<Gesture> GestureList{ get { return _gestureList; } }

	private Coroutine _delayCor;
	private int _strokeId = -1;

	public int MaxStrokeNumber {
		get {
			return _maxStrokeNumber;
		}
	}

	public int CurrentStrokeID {
		get {
			if (_strokeId <= 0)
				return 0;

			return _strokeId;
		}
	}

	public bool IsReachMaxStroke {
		get {
			if (_strokeId >= _maxStrokeNumber)
				return true;
			else
				return false;
		}
	}

	public static UnityAction<Result> OnGestureDetected;
	public static UnityAction OnGestureReset;
	public static UnityAction<List<Gesture>> OnGestureLoaded;

	protected virtual void OnEnable ()
	{
		UICountDownText.OnEndCountDown += OnStartGame;
	}

	protected virtual void OnDisable ()
	{
		UICountDownText.OnEndCountDown -= OnStartGame;
		GestureDrawing.OnStrokeStart -= OnStrokeStart;
		GestureDrawing.OnStrokeDrag -= OnStrokeDrag;
		GestureDrawing.OnStrokeEnd -= OnStrokeEnd;
	}

	private void OnStartGame()
	{
		RegisterInputHandler ();
	}

	public void RegisterInputHandler()
	{
		GestureDrawing.OnStrokeStart += OnStrokeStart;
		GestureDrawing.OnStrokeDrag += OnStrokeDrag;
		GestureDrawing.OnStrokeEnd += OnStrokeEnd;
	}

	protected virtual void Awake ()
	{
		_currentGesture = new Gesture ();
		_gestureList = new List<Gesture> ();
		_pointList = new List<Point> ();
	}

	protected virtual void Start()
	{
		LoadGesturesList ();
	}

	private void LoadGesturesList ()
	{
		_gestureList.Clear ();

		LoadGestures ();

		if (OnGestureLoaded != null)
			OnGestureLoaded (_gestureList);
	}

	protected abstract void LoadGestures ();

	protected abstract List<Gesture> GetOptimizedGestureSet ();

	private void OnStrokeStart (Lean.LeanFinger finger)
	{
		if (IsReachMaxStroke)
			return;

		if (_delayCor != null)
			StopCoroutine (_delayCor);
		
		++_strokeId;
	}

	private void OnStrokeDrag (Lean.LeanFinger finger)
	{
		if (IsReachMaxStroke)
			return;

		_pointList.Add (new Point (finger.ScreenPosition.x, -finger.ScreenPosition.y, _strokeId));
	}

	private void OnStrokeEnd (Lean.LeanFinger finger)
	{
		if (!_autoRecognize)
			return;

		List<Gesture> optimizedList = GetOptimizedGestureSet ();

		if (optimizedList == null) {
			Debug.Log ("NULL");
			return;
		}

		if (IsReachMaxStroke || _delayThreshold == 0f) { // strokeId from 0
			Recognizing (optimizedList);
		} else { 
			DelayRecognizing (optimizedList);
		}
	}

	private void DelayRecognizing (List<Gesture> gestureSet)
	{
		_delayCor = StartCoroutine (DelayRecognizingCorroutine (gestureSet));
	}

	IEnumerator DelayRecognizingCorroutine (List<Gesture> gestureSet)
	{
		float timer = 0f;

		while (true) {
			timer += Time.deltaTime;

			if (timer >= _delayThreshold) {
				Recognizing (gestureSet);
				_delayCor = null;
				yield break;
			}

			yield return null;
		}
	}

	public void ChangeToNextStroke ()
	{
		_strokeId++;
	}

	public void Recognizing (List<Gesture> gestureSet)
	{
		if (_pointList == null || _pointList.Count == 0)
			return;

		_currentGesture.SetGesture (_pointList.ToArray ());
		Result r = PointCloudRecognizer.Classify (_currentGesture, gestureSet.ToArray ());

		if (OnGestureDetected != null) {
			OnGestureDetected (r);
		}

		if (_autoResetGesture)
			ResetGesture ();
	}

	public void ResetGesture ()
	{
		_pointList.Clear ();
		_strokeId = -1;

		if (OnGestureReset != null)
			OnGestureReset ();
	}

	public List<Point> GetPointList ()
	{
		return _pointList;
	}

	public void SaveCurrentGesture (string newGestureName)
	{
		string fileName = String.Format ("{0}/{1}-{2}.xml", Application.persistentDataPath, newGestureName, DateTime.Now.ToFileTime ());

		#if !UNITY_WEBPLAYER
		_currentGesture.SetGesture (_pointList.ToArray ());
		GestureIO.WriteGesture (_currentGesture.Points, newGestureName, fileName);
		#endif
	}
}
