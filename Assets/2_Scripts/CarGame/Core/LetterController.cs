using UnityEngine;
using System.Collections;

public class LetterController : MonoBehaviour {

	public string letterName;

	private Camera mainCamera;
	private Transform mTransform;

	#region MONO
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
//		CarGameEventController.OnCollectLetter (letterName);
		Messenger.Broadcast <string> (EventManager.Vehicle.COLLECT.ToString (), letterName);

		// disable letter
		gameObject.SetActive (false);
	}

	#endregion MONO
}
