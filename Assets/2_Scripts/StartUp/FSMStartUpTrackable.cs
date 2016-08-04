/*==============================================================================
Copyright (c) 2010-2014 Qualcomm Connected Experiences, Inc.
All Rights Reserved.
Confidential and Proprietary - Qualcomm Connected Experiences, Inc.
==============================================================================*/

using UnityEngine;
using PlayMaker;
using Lean;

namespace Vuforia
{
	/// <summary>
	/// A custom handler that implements the ITrackableEventHandler interface.
	/// </summary>
	public class FSMStartUpTrackable : MonoBehaviour,
	ITrackableEventHandler
	{
		public string targetName;
		public AudioClip clip;
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

		void TriggerAnimTap(LeanFinger fg){
			if(go_anim != null && go_anim.GetCurrentAnimatorStateInfo(0).IsName("idle") && !go_anim.IsInTransition(0)){
				go_anim.SetTrigger ("tap");
			}
		}

		void _ShowModel(){
			StartCoroutine (AssetController.Instance.InstantiateGameObjectAsync (AssetController.bundleName, targetName, (bundle) => {
				go = GameObject.Instantiate (bundle, transform.position, transform.rotation) as GameObject;
				go.transform.SetParent (transform, true);
				go.AddComponent<SimpleRotateScale>();
//				go.transform.Rotate(0f, -90f, 0f);
				if(go.GetComponent<Rigidbody>() != null){
					go.GetComponent<Rigidbody>().isKinematic = true;
				}
				if(clip != null){
					AudioManager.Instance.PlayTemp(clip);
				}
				go_anim = go.GetComponentInChildren<Animator>();

				if(isLetter){
					go.transform.Rotate(0f, -180f, 0f);
					Messenger.Broadcast<bool, string>(EventManager.AR.LETTER_IMAGE_TRACKING.ToString(), true, targetName);
					go_anim.enabled = true;
					LeanTouch.OnFingerTap += TriggerAnimTap;
				}else{
					Messenger.Broadcast<bool, string>(EventManager.AR.VEHICLE_IMAGE_TRACKING.ToString(), true, targetName);
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
					Messenger.Broadcast<bool, string> (EventManager.AR.LETTER_IMAGE_TRACKING.ToString (), false, targetName);
					LeanTouch.OnFingerTap -= TriggerAnimTap;
				} else {
					Messenger.Broadcast<bool, string>(EventManager.AR.VEHICLE_IMAGE_TRACKING.ToString(), false, targetName);
				}
			}
		}
		#endregion // PRIVATE_METHODS
	}
}
