using UnityEngine;
using System.Collections;
using Vuforia;

public class TargetTracker : MonoBehaviour, ITrackableEventHandler {

	public GameObject myGame;

	private TrackableBehaviour trackableBehaviour;
	// Use this for initialization
	void Start () {
		trackableBehaviour = GetComponent<TrackableBehaviour> ();
		if (trackableBehaviour)
			trackableBehaviour.RegisterTrackableEventHandler (this);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	#region ITrackableEventHandler implementation

	public void OnTrackableStateChanged (TrackableBehaviour.Status previousStatus, TrackableBehaviour.Status newStatus)	{
		if(newStatus == TrackableBehaviour.Status.DETECTED ||
			newStatus == TrackableBehaviour.Status.TRACKED ||
			newStatus == TrackableBehaviour.Status.EXTENDED_TRACKED) {
			OnTrackingFound ();
		} else {
			OnTrackingLost ();
		}
			
	}

	#endregion

	#region 
	void OnTrackingFound () {
		if(myGame!= null)
			myGame.SetActive(true);
	}

	void OnTrackingLost () {
		if(myGame!= null)
			myGame.SetActive(false);
	}

	#endregion
}
