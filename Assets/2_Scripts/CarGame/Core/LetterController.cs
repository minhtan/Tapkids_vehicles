using UnityEngine;
using System.Collections;

public class LetterController : MonoBehaviour {

	public string letterName;

	private Camera mainCamera;
	private Transform mTransform;

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

	void Start () {
		mainCamera = Camera.main;
		mTransform = GetComponent <Transform> ();
	}

	void Update () {
		// camera facing billboard effect
		mTransform.LookAt (mTransform.position + mainCamera.transform.rotation * Vector3.forward, mainCamera.transform.rotation * Vector3.up);
	}

	void OnTriggerEnter (Collider other) {
		// collect letter
		Messenger.Broadcast <string> (EventManager.Vehicle.COLLECT_LETTER.ToString (), letterName);

		// disable letter
		gameObject.SetActive (false);
	}

	#endregion MONO
}
