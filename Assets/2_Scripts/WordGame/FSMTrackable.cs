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
	public class FSMTrackable : MonoBehaviour,
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
				go = GameObject.Instantiate(bundle);
				go.transform.localScale += new Vector3(49f, 49f, 49f);
				go.transform.SetParent (transform, false);
				go_anim = go.GetComponentInChildren<Animator>();

				if(isLetter){
					Messenger.Broadcast<bool, string>(EventManager.AR.IMAGETRACKING.ToString(), true, targetName);
				}
			}));
		}

		void _HideModel(){
			if(go != null){
				GameObject.Destroy(go);
				Resources.UnloadUnusedAssets();
				go = null;
				go_anim = null;

				if(isLetter){
					Messenger.Broadcast<bool, string>(EventManager.AR.IMAGETRACKING.ToString(), false, targetName);
				}
			}
		}
		#endregion // PRIVATE_METHODS
	}
}
