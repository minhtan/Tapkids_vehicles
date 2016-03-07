using UnityEngine;
using System.Collections;
using Vuforia;

public class TargetTrackableEventHandler : MonoBehaviour, ITrackableEventHandler {

	#region public members
	public string letter = "a";
	#endregion public members

	#region private members
	Transform mTransform;
	private TrackableBehaviour mTrackableBehaviour;
	#endregion private members

	#region ITrackableEventHandler implementation

	public void OnTrackableStateChanged (TrackableBehaviour.Status previousStatus, TrackableBehaviour.Status newStatus)
	{
		if (newStatus == TrackableBehaviour.Status.DETECTED ||
			newStatus == TrackableBehaviour.Status.TRACKED ||
			newStatus == TrackableBehaviour.Status.EXTENDED_TRACKED)
			OnTrackingFound ();
		else
			OnTrackingLost ();
	}

	void OnTrackingFound () 
	{
		// send letter to game controller
		CarGameEventController.OnTargetTracking(true, mTransform, letter);


	}

	void OnTrackingLost () 
	{
		// send lost message pause game controller
		CarGameEventController.OnTargetTracking(false, mTransform, letter);
		// unparent game controller

	}
	#endregion

	#region Mono
	void Awake () {
		mTransform = this.transform;

		mTrackableBehaviour = GetComponent<TrackableBehaviour>();
		if (mTrackableBehaviour)
		{
			mTrackableBehaviour.RegisterTrackableEventHandler(this);
		}
	}
	void Start () {

	}
	#endregion Mono

	#region public functions
	#endregion public functions

	#region private functions
	#endregion private functions

}
