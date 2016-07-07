using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using Lean;
using PDollarGestureRecognizer;

public class TemplateEditor : MonoBehaviour
{
	public Transform _canvas;
	public GameObject _pointPrefab;
	public GameObject _circleRoot;
	public Slider _radiusSlider;
	public Slider _pointNumber;
	public Text _radiusText;
	public Text _pointNumberText;
	public Text _currentStrokeIndexText;
	public InputField _gestureName;
	private List<List<Transform>> _strokeList;
	private List<Point> _pointList;
	private string _type = "point";
	private GameObject _currentTarget;
	private int _currentStrokeIndex = 0;

	void OnEnable()
	{
		LeanTouch.OnFingerDown += OnFingerDown;
	}

	void OnDisable()
	{
		LeanTouch.OnFingerDown -= OnFingerDown;
	}

	void Awake ()
	{
		_strokeList = new List<List<Transform>> ();
		_strokeList.Add (new List<Transform> ());
		_pointList = new List<Point> ();
	}

	private void OnFingerDown(LeanFinger finger)
	{
		if (LeanTouch.GuiInUse)
			return;

		RaycastHit hit;
		Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);

		if(Physics.Raycast(ray, out hit, 100f))
		{
			if(hit.collider.name.Contains("Template_"))
			{
				_currentTarget = hit.collider.gameObject;
			}
			return;
		}
	
		if(_type.Contains("circle"))
		{
			GenerateCircle (finger.ScreenPosition, _radiusSlider.value, (int)_pointNumber.value);
		}
		else{
			Transform point = GeneratePoint (finger.ScreenPosition);

		}
	}

	public void UpdateUI()
	{
		_radiusText.text = _radiusSlider.value.ToString();
		_pointNumberText.text = _pointNumber.value.ToString();

		if(_currentTarget != null)
		{
			Vector3 center = _currentTarget.transform.position;
			Destroy (_currentTarget);
			_currentTarget = GenerateCircle (center, _radiusSlider.value, 	(int)_pointNumber.value);
		}
	}

	public void setType(string type)
	{
		_type = type;
	}

	public void NextStroke()
	{
		_currentStrokeIndex++;
		_currentStrokeIndexText.text = "Stroke index: " + _currentStrokeIndex;
		_strokeList.Add(new List<Transform> ());
	}
		
	private GameObject GenerateCircle (Vector3 center, float radius, int pointNumber)
	{
		GameObject circleRoot = Instantiate (_circleRoot, center, Quaternion.identity) as GameObject;
		circleRoot.transform.SetParent (_canvas);

		NextStroke ();

		float angleStep = 360f / pointNumber;

		for (int i = 0; i < pointNumber; i++) {
			float currentAngle = angleStep * i * Mathf.Deg2Rad;
			Vector3 position = Vector3.zero;
			position.x = center.x + radius * Mathf.Cos (currentAngle);
			position.y = center.y + radius * Mathf.Sin (currentAngle);
			Transform point = GeneratePoint (position);
			point.SetParent (circleRoot.transform);
		}
		circleRoot.GetComponent<CircleCollider2D> ().radius = _radiusSlider.value;

		_currentTarget = circleRoot;
		return circleRoot;
	}

	private Transform GeneratePoint (Vector3 position)
	{
		Transform pointTrans = ((GameObject)Instantiate (_pointPrefab, position, Quaternion.identity)).transform;
		pointTrans.SetParent (_canvas);
		_currentTarget = pointTrans.gameObject;
		_strokeList [_currentStrokeIndex].Add (pointTrans);
		return pointTrans;
	}

	public void Undo()
	{
		Destroy (_currentTarget);
	}
		
	public void Export()
	{
		Gesture gesture = new Gesture ();
		gesture.Name = _gestureName.text;
		gesture.StrokeCount = _strokeList.Count;

		for(int i = 0; i < _strokeList.Count; i++)
		{
			for(int k = 0; k < _strokeList[i].Count; k++)
			{
				Vector3 position = _strokeList [i] [k].position;
				_pointList.Add (new Point(position.x, position.y, i));
			}
		}
		gesture.Points = _pointList.ToArray ();
		GestureIO.SaveCustomGestureTemplate (_gestureName.text, gesture);
	}
}
