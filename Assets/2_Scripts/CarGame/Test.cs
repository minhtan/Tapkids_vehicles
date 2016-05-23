using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class Test : MonoBehaviour {

	public Transform[] points;
	private Vector3[] tempVectors;
	void Start () {
		tempVectors = new Vector3 [points.Length]; 
		for (int i = 0; i < points.Length; i++) {
			tempVectors [i] = points[i].localPosition;
		}
		LeanTween.moveSpline (this.gameObject, tempVectors, .5f);//.setEase(LeanTweenType.easeInBack);
//		LeanTween.
	}


}
