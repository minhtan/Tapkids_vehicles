/*==============================================================================
Copyright (c) 2010-2014 Qualcomm Connected Experiences, Inc.
All Rights Reserved.
Confidential and Proprietary - Qualcomm Connected Experiences, Inc.
==============================================================================*/

using UnityEngine;
using PlayMaker;

namespace Vuforia
{
	/// <summary>
	/// A custom handler that implements the ITrackableEventHandler interface.
	/// </summary>
	public class FSMStartUpTrackable : MonoBehaviour,
	ITrackableEventHandler
	{
		public string targetName;
		public bool isLetter = false;

		#region PRIVATE_MEMBER_VARIABLES

		private TrackableBehaviour mTrackableBehaviour;
		private PlayMakerFSM fsm;
		private GameObject go;
		private Animator go_anim;
		#endregion // PRIVATE_MEMBER_VARIABLES



		#region UNTIY_MONOBEHAVIOUR_METHODS

		void Awake(){
			fsm = gameObject.GetComponent<PlayMakerFSM>();
			fsm.FsmVariables.GetFsmString("letter").Value = targetName.ToLower ();
		}

		void Start()
		{
			mTrackableBehaviour = GetComponent<TrackableBehaviour>();
			if (mTrackableBehaviour)
			{
				mTrackableBehaviour.RegisterTrackableEventHandler(this);
			}
		}

		#endregion // UNTIY_MONOBEHAVIOUR_METHODS



		#region PUBLIC_METHODS

		/// <summary>
		/// Implementation of the ITrackableEventHandler function called when the
		/// tracking state changes.
		/// </summary>
		public void OnTrackableStateChanged(
			TrackableBehaviour.Status previousStatus,
			TrackableBehaviour.Status newStatus)
		{
			if (newStatus == TrackableBehaviour.Status.DETECTED ||
				newStatus == TrackableBehaviour.Status.TRACKED ||
				newStatus == TrackableBehaviour.Status.EXTENDED_TRACKED)
			{
				OnTrackingFound();
			}
			else
			{
				OnTrackingLost();
			}
		}

		#endregion // PUBLIC_METHODS



		#region PRIVATE_METHODS


		private void OnTrackingFound(){
			fsm.Fsm.Event("found");
		}

		private void OnTrackingLost(){
			fsm.Fsm.Event("lost");
		}

		public void Ready(){
			fsm.Fsm.Event ("ready");
		}

		void _ShowModel(){
			StartCoroutine (AssetController.Instance.InstantiateGameObjectAsync (AssetController.bundleName, targetName, (bundle) => {
				go = GameObject.Instantiate (bundle, transform.position, transform.rotation) as GameObject;
				go.transform.SetParent (transform, true);
//				go.transform.Rotate(0f, -90f, 0f);
				if(go.GetComponent<Rigidbody>() != null){
					go.GetComponent<Rigidbody>().isKinematic = true;
				}

				go_anim = go.GetComponentInChildren<Animator>();

				if(isLetter){
					Messenger.Broadcast<bool, string>(EventManager.AR.LETTER_TRACKING.ToString(), true, targetName);
				}else{
					Messenger.Broadcast<bool, string>(EventManager.AR.VEHICLE_TRACKING.ToString(), true, targetName);
				}
			}));
		}

		void _HideModel(){
			if(go != null){
				GameObject.Destroy(go);
				go = null;
				go_anim = null;
				Resources.UnloadUnusedAssets();

				if (isLetter) {
					Messenger.Broadcast<bool, string> (EventManager.AR.LETTER_TRACKING.ToString (), false, targetName);
				} else {
					Messenger.Broadcast<bool, string>(EventManager.AR.VEHICLE_TRACKING.ToString(), false, targetName);
				}
			}
		}
		#endregion // PRIVATE_METHODS
	}
}
