using UnityEngine;
using System.Collections;

public class SteeringWheel : MonoBehaviour {

	float initAngle;
	float dragAngle;
	float angleDiff;
	public static float angleThreshold = 90f;
	public float AngleThreshold {
		get {return angleThreshold;}
	}

	Vector2 rectWordPos;
	RectTransform rectTran;

	bool isPressingOnWheel;

	public void _OnWheelPress(bool state){
		if(rectTran.localRotation == Quaternion.identity){
			isPressingOnWheel = state;
		}
	}

	void OnEnable () {
		Lean.LeanTouch.OnFingerDown += OnFingerDown;
		Lean.LeanTouch.OnFingerUp += OnFingerUp;
		Lean.LeanTouch.OnFingerDrag += OnFingerDrag;
	}

	void OnDisable(){
		Lean.LeanTouch.OnFingerDown -= OnFingerDown;
		Lean.LeanTouch.OnFingerUp -= OnFingerUp;
		Lean.LeanTouch.OnFingerDrag -= OnFingerDrag;
	}

	void Start(){
		rectTran = GetComponent<RectTransform> ();
		rectWordPos = RectTransformExtension.GetScreenWorldPos (rectTran);
	}
	
	void OnFingerDown(Lean.LeanFinger fg){
		if(isPressingOnWheel){
			initAngle = fg.GetDegrees (rectWordPos);
			dragAngle = initAngle;
			angleDiff = 0;
		}
	}

	void OnFingerDrag(Lean.LeanFinger fg){
		if(isPressingOnWheel){
			dragAngle += fg.GetDeltaDegrees(rectWordPos);
			angleDiff = (dragAngle - initAngle) * -1;

			if(Mathf.Abs(angleDiff) < angleThreshold){
				rectTran.Rotate (Vector3.forward, fg.GetDeltaDegrees(rectWordPos));
				Messenger.Broadcast<float> (EventManager.GUI.MENUWHEELTURN.ToString (), angleDiff);
			}
		}
	}

	void OnFingerUp(Lean.LeanFinger fg){
		Messenger.Broadcast<float> (EventManager.GUI.MENUWHEELRELEASE.ToString (), angleDiff);

		float angle = rectTran.localRotation.eulerAngles.z;
		angle = angle > 180f ? (angle - 360) * -1 : angle * -1;
		LeanTween.rotate(rectTran, angle, 0.5f);

		isPressingOnWheel = false;
		angleDiff = 0;
	}
}
