using UnityEngine;
using System.Collections;
using Vuforia;

public class MapTrackableEventHandler : MonoBehaviour, ITrackableEventHandler {

	#region public members
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
//		CarGameEventController.OnMapTracking(true, mTransform);
		Messenger.Broadcast <bool, Transform> (EventManager.AR.MAP_TRACKING.ToString(), true, mTransform);
	}

	void OnTrackingLost () 
	{
//		CarGameEventController.OnMapTracking(false, mTransform);
		Messenger.Broadcast <bool, Transform> (EventManager.AR.MAP_TRACKING.ToString(), false, mTransform);
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
