using UnityEngine;
using System.Collections;

public class SteeringWheel : MonoBehaviour {

	float initAngle;
	float dragAngle;
	float angleThreshold = 90f;

	Vector2 rectWordPos;
	RectTransform rectTran;

	bool isPressingOnWheel;

	public void _OnWheelPress(bool state){
		isPressingOnWheel = state;
	}

	void OnEnable () {
		Lean.LeanTouch.OnFingerDown += OnFingerDown;
		Lean.LeanTouch.OnFingerUp += OnFingerUp;
		Lean.LeanTouch.OnFingerDrag += OnFingerDrag;
	}

	void Start(){
		rectTran = GetComponent<RectTransform> ();
		rectWordPos = RectTransformExtension.GetScreenWorldPos (rectTran);
	}
	
	void OnFingerDown(Lean.LeanFinger fg){
		if(!isPressingOnWheel){
			return;
		}
		initAngle = fg.GetDegrees (rectWordPos);
		dragAngle = initAngle;
	}

	void OnFingerDrag(Lean.LeanFinger fg){
		if(!isPressingOnWheel){
			return;
		}
		dragAngle += fg.GetDeltaDegrees(rectWordPos);
		float angleDiff = (dragAngle - initAngle) * -1;

		if(Mathf.Abs(angleDiff) < angleThreshold){
			rectTran.Rotate (Vector3.forward, fg.GetDeltaDegrees(rectWordPos));
		}
	}

	void OnFingerUp(Lean.LeanFinger fg){
		float angle = rectTran.localRotation.z;
		Debug.Log (angle);
		LeanTween.rotate(rectTran, 0f, 0.5f);
		_OnWheelPress (false);
	}
}
