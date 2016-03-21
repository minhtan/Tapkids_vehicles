using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using PDollarGestureRecognizer;
using UnityEngine.Events;
using System;


public class LeanGestureRecognizer : MonoBehaviour
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
	private bool _isInProgress = false;

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

	public delegate void OnGestureDetectedEvent (Result result);

	public static event OnGestureDetectedEvent OnGestureDetected;

	public delegate void OnGestureResetEvent ();

	public static event OnGestureResetEvent OnGestureReset;

	void OnEnable ()
	{
		GestureLineDrawing.OnStrokeStart += OnStrokeStart;
		GestureLineDrawing.OnStrokeDrag += OnStrokeDrag;
		GestureLineDrawing.OnStrokeEnd += OnStrokeEnd;
	}

	void OnDisable ()
	{
		GestureLineDrawing.OnStrokeStart -= OnStrokeStart;
		GestureLineDrawing.OnStrokeDrag -= OnStrokeDrag;
		GestureLineDrawing.OnStrokeEnd -= OnStrokeEnd;
	}

	void Awake ()
	{
		_currentGesture = new Gesture ();
		_gestureList = new List<Gesture> ();
		_pointList = new List<Point> ();

		LoadGestures ();

	}

	public void LoadGestures ()
	{
		_gestureList.Clear ();
		GestureIO.LoadPremadeGestureTemplates ("GestureTemplates", _gestureList);
		GestureIO.LoadCustomGestureTemplates (_gestureList);
	}

	private void OnStrokeStart (Lean.LeanFinger finger)
	{
		if (IsReachMaxStroke)
			return;
		
		_isInProgress = true;
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
		
		if (IsReachMaxStroke) { // strokeId from 0
			Recognizing ();
		} else { 
			DelayRecognizing ();
		}
	}

	private void DelayRecognizing ()
	{
		_isInProgress = false;
		_delayCor = StartCoroutine (DelayRecognizingCorroutine ());
	}

	IEnumerator DelayRecognizingCorroutine ()
	{
		float timer = 0f;

		while (true) {
			timer += Time.deltaTime;

			if (_isInProgress)
				yield break;

			if (timer >= _delayThreshold) {
				Recognizing ();
				yield break;
			}

			yield return null;
		}
	}

	public void ChangeToNextStroke()
	{
		_strokeId++;
	}

	public void Recognizing ()
	{
		if (_pointList == null || _pointList.Count == 0)
			return;
		
		if (OnGestureDetected != null) {
			_currentGesture.SetGesture (_pointList.ToArray ());
			Result r = PointCloudRecognizer.Classify (_currentGesture, _gestureList.ToArray ());

			OnGestureDetected (r);
		}

		if (_autoResetGesture)
			ResetGesture ();
	}

	public void ResetGesture ()
	{
		_isInProgress = false;
		_pointList.Clear ();
		_strokeId = -1;

		if (OnGestureReset != null)
			OnGestureReset ();
	}

	public List<Point> GetPointList ()
	{
		return _pointList;
	}

	public void SaveCurrentGesture(string newGestureName)
	{
		string fileName = String.Format("{0}/{1}-{2}.xml", Application.persistentDataPath, newGestureName, DateTime.Now.ToFileTime());

		#if !UNITY_WEBPLAYER
		GestureIO.WriteGesture(_currentGesture.Points, newGestureName, fileName);
		#endif
	}

	public Gesture GetGestureByName(string name)
	{
		for(int i = 0; i < _gestureList.Count; i++)
		{
			if (_gestureList [i].Name == name)
				return _gestureList [i];
		}

		return null;
	}
}
