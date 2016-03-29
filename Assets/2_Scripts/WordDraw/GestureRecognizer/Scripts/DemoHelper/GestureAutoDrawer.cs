using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using PDollarGestureRecognizer;
using Vectrosity;

public class GestureAutoDrawer : MonoBehaviour {
	public LeanGestureRecognizer recognizer;
	public string _gestureName;
	public float _duration;
	public float _scaleFactor = 300f;

	public float lineWidth = 4f;
	public Material lineMaterial;
	public bool useEndCap = false;
	public Texture2D capTex;
	public Material capMaterial;

	private List<VectorLine> lineList;
	private VectorLine currentLine;
	private Vector2 previousPosition;
	private int sqrMinPixelMove;

	private List<Gesture> _gestureList;
	private Gesture _currentGesture;


	void Start()
	{
		lineList = new List<VectorLine> ();

		if (useEndCap) {
			VectorLine.SetEndCap ("cap", EndCap.Mirror, capMaterial, capTex);
			lineMaterial = capMaterial;
		}

		lineList = new List<VectorLine> ();

		_gestureList = new List<Gesture> ();
		GestureIO.LoadPremadeGestureTemplates ("GestureTraining", _gestureList);
		_currentGesture = GestureUtils.GetGestureByName (_gestureList, _gestureName);

		for(int i = 0; i < _currentGesture.StrokeCount; i++)
		{
			AddStroke (i);
		}

		AutoDrawGesture (_currentGesture);
	}

	private void AddStroke (int index)
	{
		VectorLine line = new VectorLine ("GestureStroke" + index, new List<Vector2> (), lineMaterial, lineWidth, LineType.Continuous, Joins.Weld);

		// Optimization for updating only the last point of the currentLine, and the rest is not re-computed
		line.endPointsUpdate = 1;

		if (useEndCap) {
			line.endCap = "cap";
		}

		RectTransform rectTrans = line.rectTransform;
		rectTrans.anchorMin = Vector2.right;
		rectTrans.anchorMax = Vector2.up;
		rectTrans.pivot = Vector2.one / 2;

		rectTrans.localScale = Vector2.one * _scaleFactor;
		Quaternion localRot = rectTrans.localRotation;
		localRot.x = 180f;
		rectTrans.localRotation = localRot;

		lineList.Add (line);
	}
		

	private void AutoDrawGesture(Gesture gesture)
	{
		List<List<Point>> strokeList = GestureUtils.GetStrokeListFromGesture (gesture);
		//StartCoroutine (DrawPointShuffleCor (strokeList, _duration));
		StartCoroutine (DrawPointCor(strokeList, _duration));
	}

	private IEnumerator DrawPointCor(List<List<Point>> points, float duration)
	{
		
		WaitForSeconds wait = new WaitForSeconds (duration);
		int curIndex = 0;
		int curStroke = 0;
		currentLine = lineList [curStroke];
		List<Point> stroke = points [curStroke];

		while(true)
		{
			curIndex++;

			if (curIndex == stroke.Count) {
				curStroke++;


				if (curStroke == points.Count)
					yield break;
				
				curIndex = 0;
				currentLine = lineList [curStroke];
				stroke = points [curStroke];
			}

			currentLine.points2.Add (new Vector2(stroke[curIndex].X, stroke[curIndex].Y));

			currentLine.Draw ();

			yield return wait;
		}
	}

	private IEnumerator DrawPointShuffleCor(List<List<Point>> points, float duration)
	{

		WaitForSeconds wait = new WaitForSeconds (duration);
		int curIndex = 0;
		int strokeCounter = 0;

		int[] strokeShuffle = GestureUtils.GetShuffleStrokeOrder (_currentGesture);
		int curStroke = strokeShuffle[strokeCounter];

		currentLine = lineList [curStroke];
		List<Point> stroke = points [curStroke];

		while(true)
		{
			curIndex++;

			if (curIndex == stroke.Count) {
				strokeCounter++;

				if (strokeCounter == points.Count)
					yield break;

				curStroke = strokeShuffle [strokeCounter];

				curIndex = 0;
				currentLine = lineList [curStroke];
				stroke = points [curStroke];
			}

			currentLine.points2.Add (new Vector2(stroke[curIndex].X, stroke[curIndex].Y));

			currentLine.Draw ();

			yield return wait;
		}
	}
}
