using UnityEngine;
using System.Collections;
using System;
using System.Linq;

public class Test : MonoBehaviour {

	void Start(){
		StartCoroutine(RunVehicle(gameObject.transform, GameObject.FindObjectOfType<BezierSpline>(), () => {
			
		}));
	}

	IEnumerator RunVehicle (Transform _trans, BezierSpline _spline, System.Action _callback = null, bool _isGoingForward = true, float _duration = 1f) {
		float progress = _isGoingForward ? 0 : 1;
		while (true) {
			if (_isGoingForward) {
				progress += Time.deltaTime / _duration;
				if (progress >= 1f) {
					if (_callback != null)
						_callback ();
					yield break;
				}
			}else{
				progress -= Time.deltaTime / _duration;
				if (progress < 0f) {
					if (_callback != null)
						_callback ();
					yield break;
				}
			}

			Vector3 position = _spline.GetPoint(progress);
			_trans.localPosition = position;
			_trans.LookAt(position + _spline.GetDirection(progress));
			yield return null;
		}
	}

}
