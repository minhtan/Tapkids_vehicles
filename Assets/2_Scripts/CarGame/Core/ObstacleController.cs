using UnityEngine;
using System.Collections;

public class ObstacleController : MonoBehaviour {
	Vector3 originPos;
	Quaternion originRot;
	Vector3 originScale;

	#region MONO
	void Awake () {
		originPos = transform.localPosition;
		originRot = transform.localRotation;
		originScale = transform.localScale;
	}

	void OnEnable () {
		transform.localPosition = new Vector3 (transform.localPosition.x, transform.localPosition.y - 1f, transform.localPosition.z);

		LeanTween.moveLocal (gameObject, originPos, 1f).setEase (LeanTweenType.easeOutBack);
	}

	void OnDisable () {
		transform.localPosition = originPos;
		transform.localRotation = originRot;
		transform.localScale = originScale;
	}
	#endregion MONO
}
