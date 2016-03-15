using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using PDollarGestureRecognizer;
using Vectrosity;

public class GestureAutoDrawer : MonoBehaviour {
	public LeanGestureRecognizer recognizer;

	public float lineWidth = 4f;
	public Material lineMaterial;
	public bool useEndCap = false;
	public Texture2D capTex;
	public Material capMaterial;

	private List<VectorLine> lineList;
	private VectorLine currentLine;
	private Vector2 previousPosition;
	private int sqrMinPixelMove;
	private bool canDraw = false;

	private Gesture _currentGesture;

	void Awake()
	{
		lineList = new List<VectorLine> ();

		if (useEndCap) {
			VectorLine.SetEndCap ("cap", EndCap.Mirror, capMaterial, capTex);
			lineMaterial = capMaterial;
		}

		lineList = new List<VectorLine> ();

		_currentGesture = recognizer.GetGestureByName ("O");

		for(int i = 0; i < _currentGesture.StrokeCount; i++)
		{
			AddStroke (i);
		}
	}

	private void AddStroke (int index)
	{
		VectorLine line = new VectorLine ("GestureStroke" + index, new List<Vector2> (), lineMaterial, lineWidth, LineType.Continuous, Joins.Weld);

		// Optimization for updating only the last point of the currentLine, and the rest is not re-computed
		line.endPointsUpdate = 1;

		if (useEndCap) {
			line.endCap = "cap";
		}

		lineList.Add (line);
	}


	private IEnumerator DrawPoint(Gesture gesture)
	{
		Point[] points = gesture.Points;
		int currentPointIndex = 0;

		while(true)
		{
			currentPointIndex++;


			if(currentPointIndex == points.Length)
				yield break;
			
			yield return null;
		}
	}
}
