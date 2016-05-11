using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using PDollarGestureRecognizer;

public class EditorRecognizer : LeanGestureRecognizer {
	/// <summary>
	/// Loads the gestures.
	/// </summary>
	protected override void LoadGestures ()
	{
		GestureIO.LoadPremadeGestureTemplates ("GestureTemplates", GestureList);
	}

	protected override List<Gesture> GetOptimizedGestureSet ()
	{
		return GestureList;
	}
}
