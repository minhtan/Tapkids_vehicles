using UnityEngine;
using System.Collections.Generic;
using PDollarGestureRecognizer;

public class GestureUtils {

	public static int GetRandomStrokeIndex(Gesture gesture)
	{
		return Random.Range (0, gesture.StrokeCount);
	}

	public static int[] GetShuffleStrokeOrder(Gesture gesture)
	{
		int[] indexs = new int[gesture.StrokeCount];

		for (int i = 0; i < indexs.Length; i++) {
			indexs [i] = i;
		}
			
		return  ShuffleArray<int> (indexs);
	}

	public static T[] ShuffleArray<T>(T[] array)
	{
		int pivotIndex = 0;
		T[] outArr = new T[array.Length];

		for (int i = 0; i < outArr.Length; i++) {
			outArr [i] = array [i];
		}

		while(pivotIndex < array.Length - 1)
		{
			int swappedIndex = Random.Range (pivotIndex + 1, array.Length);

			Swap<T> (ref array[pivotIndex], ref array[swappedIndex]);

			pivotIndex++;
		}

		return array;
	}

	private static void Swap<T>(ref T a, ref T b)
	{
		T tmp = a;
		a = b;
		b = tmp;
	}

	public static List<List<Point>> GetStrokeListFromGesture(Gesture gesture)
	{
		Point[] points = gesture.Points;
		List<List<Point>> strokeList = new List<List<Point>> ();

		for (int i = 0; i < gesture.StrokeCount; i++) {
			strokeList.Add (new List<Point>());
		}

		for (int i = 0; i < points.Length; i++) {
			strokeList [points [i].StrokeID].Add (points[i]);
		}

		return strokeList;
	}
}
