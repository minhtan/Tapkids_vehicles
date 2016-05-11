using UnityEngine;
using System.Collections;

public enum WHEELMODE {
	MENU,
	CONTROL
}
public class SteeringWheel : MonoBehaviour {

	public bool updateEveryFrame = false;
	public WHEELMODE wheelMode = WHEELMODE.MENU;
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
		switch (wheelMode) {
		case WHEELMODE.MENU:
			if(rectTran.localRotation == Quaternion.identity){
				isPressingOnWheel = state;
			}
			break;
		case WHEELMODE.CONTROL:
			LeanTween.cancelAll ();
			isPressingOnWheel = state;
			break;
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

	void Update(){
		if(updateEveryFrame){
			float angle = rectTran.localRotation.eulerAngles.z;
			angle = angle > 180f ? (angle - 360) * -1 : angle * -1;
			Messenger.Broadcast<float> (EventManager.GUI.WHEEL_FRAME_TURN.ToString (), angle);
		}
	}
	
	void OnFingerDown(Lean.LeanFinger fg){
		if(isPressingOnWheel){
			initAngle = fg.GetDegrees (rectWordPos);
			dragAngle = initAngle;
		}
	}

	void OnFingerDrag(Lean.LeanFinger fg){
		if(isPressingOnWheel){
			dragAngle += fg.GetDeltaDegrees(rectWordPos);
			angleDiff = (dragAngle - initAngle) * -1;

			if(Mathf.Abs(angleDiff) < angleThreshold){
				rectTran.Rotate (Vector3.forward, fg.GetDeltaDegrees(rectWordPos));
				Messenger.Broadcast<float> (EventManager.GUI.MENU_WHEEL_TURN.ToString (), angleDiff);
			}
		}
	}

	void OnFingerUp(Lean.LeanFinger fg){
		if (isPressingOnWheel) {
			float angle = rectTran.localRotation.eulerAngles.z;
			angle = angle > 180f ? (angle - 360) * -1 : angle * -1;
			Messenger.Broadcast<float> (EventManager.GUI.MENU_WHEEL_RELEASE.ToString (), angle);
			LeanTween.rotate (rectTran, angle, 0.5f).setEase(LeanTweenType.easeOutBack);

			isPressingOnWheel = false;
		}
	}
}
