using UnityEngine;
using System.Collections;
using Vuforia;

public class LetterTrackableEventHandler : MonoBehaviour, ITrackableEventHandler {

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
//		CarGameEventController.OnLetterTracking(true, letter);
		Messenger.Broadcast <bool, string> (EventManager.AR.LETTER_TRACKING.ToString(), true, letter);
	}

	void OnTrackingLost () 
	{
		// send lost event to game controller
//		CarGameEventController.OnLetterTracking(false, letter);
		Messenger.Broadcast <bool, string> (EventManager.AR.LETTER_TRACKING.ToString(), false, letter);
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
