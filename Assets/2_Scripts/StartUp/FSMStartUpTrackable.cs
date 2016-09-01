/*==============================================================================
Copyright (c) 2010-2014 Qualcomm Connected Experiences, Inc.
All Rights Reserved.
Confidential and Proprietary - Qualcomm Connected Experiences, Inc.
==============================================================================*/

using UnityEngine;
using PlayMaker;
using Lean;
using System.Collections;
using System;
using UnityEngine.UI;

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
		bool isVehicleRunning = false;
		BezierSpline spline;

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
			Debug.Log ("Found " + targetName);
		}

		private void OnTrackingLost(){
			fsm.Fsm.Event("lost");
			Debug.Log ("Lost " + targetName);
		}

		public void Ready(){
			fsm.Fsm.Event ("ready");
		}

		void CallRunVehicle(LeanFinger fg){
			if(!isVehicleRunning && !LeanTouch.GuiInUse && !fg.IsOverGui && !fg.StartedOverGui){
				isVehicleRunning = true;
				GameObject g = transform.GetChild (0).gameObject;
				StartCoroutine (RunVehicle(g.transform, spline, ()=>{
					LeanTween.rotate(g, Vector3.zero, 0.2f);
					LeanTween.move(g, Vector3.zero, 0.2f).setOnComplete(()=>{
						isVehicleRunning = false;
					});
				}, true, 3.0f));
			}
		}
		
		IEnumerator RunVehicle (Transform _trans, BezierSpline _spline, System.Action _callback = null, bool _isGoingForward = true, float _duration = 1f) {
			float progress = _isGoingForward ? 0 : 1;
			while (true) {
				if (_isGoingForward) {
					progress += Time.deltaTime / _duration;
					if (progress >= 1f) {
						if (_callback != null)
							_callback ();
						yield break;
					}
				}else{
					progress -= Time.deltaTime / _duration;
					if (progress < 0f) {
						if (_callback != null)
							_callback ();
						yield break;
					}
				}

				Vector3 position = _spline.GetPoint(progress);
				_trans.localPosition = position;
				_trans.LookAt(_trans.position + _spline.GetDirection(progress));

				yield return null;
			}
		}

		void TriggerAnimTap(LeanFinger fg){
			if(go_anim != null && go_anim.GetCurrentAnimatorStateInfo(0).IsName("idle") && !go_anim.IsInTransition(0) && !LeanTouch.GuiInUse && !fg.IsOverGui && !fg.StartedOverGui){
				go_anim.SetTrigger ("tap");
			}
		}

		void _ShowModel(){
			GameObject.Find ("DebugText").GetComponent<Text>().text = "Found " + targetName;
			if (isLetter) {
				StartCoroutine (AssetController.Instance.InstantiateGameObjectAsync (AssetController.bundleName, targetName, (bundle) => {
					go = GameObject.Instantiate (bundle, transform.position, transform.rotation) as GameObject;
					go.transform.SetParent (transform, true);
					go.AddComponent<SimpleRotateScale> ();
					if (go.GetComponent<Rigidbody> () != null) {
						go.GetComponent<Rigidbody> ().isKinematic = true;
					}

					AudioClip clip = Resources.Load<AudioClip> ("Sounds/" + targetName);
					if (clip != null) {
						AudioManager.Instance.PlayTemp (clip);
					}
					go_anim = go.GetComponentInChildren<Animator> ();

					go.transform.Rotate (0f, -180f, 0f);
					go_anim.enabled = true;

					Messenger.Broadcast<bool, string> (EventManager.AR.LETTER_IMAGE_TRACKING.ToString (), true, targetName);
					LeanTouch.OnFingerTap += TriggerAnimTap;
				}));
			} else {
				spline = GameObject.FindObjectOfType<BezierSpline> ();
				StartCoroutine(LoadFromResource (targetName, (prefab) => {
					go = GameObject.Instantiate (prefab, transform.position, transform.rotation) as GameObject;
					go.transform.SetParent (transform, true);
					go.AddComponent<SimpleRotateScale> ();
					try{
						Destroy(go.GetComponent<Rigidbody> ());
						Destroy(go.GetComponent<ArcadeCarUserController>());
						Destroy(go.GetComponent<ArcadeCarController>());
					}catch(Exception e){
						
					}

					AudioClip clip = Resources.Load<AudioClip> ("Sounds/" + targetName);
					if (clip != null) {
						AudioManager.Instance.PlayTemp (clip);
					}
					go_anim = go.GetComponentInChildren<Animator> ();

					Messenger.Broadcast<bool, string> (EventManager.AR.VEHICLE_IMAGE_TRACKING.ToString (), true, targetName);
					LeanTouch.OnFingerTap += CallRunVehicle;
				}));
			}
		}

		IEnumerator LoadFromResource(string asset, Action<GameObject> callback){
			ResourceRequest rr = Resources.LoadAsync<GameObject> ("Vehicles/" + asset);
			while(!rr.isDone){
				yield return null;
			}
			GameObject go = rr.asset as GameObject;
			if (go != null) {
				callback (go);
			}
		} 

		void _HideModel(){
			GameObject.Find ("DebugText").GetComponent<Text>().text = "Lost " + targetName;
			if(go != null){
				GameObject.DestroyImmediate(go);
				go = null;
				go_anim = null;
				Resources.UnloadUnusedAssets();

				if (isLetter) {
					Messenger.Broadcast<bool, string> (EventManager.AR.LETTER_IMAGE_TRACKING.ToString (), false, targetName);
					LeanTouch.OnFingerTap -= TriggerAnimTap;
				} else {
					Messenger.Broadcast<bool, string> (EventManager.AR.VEHICLE_IMAGE_TRACKING.ToString(), false, targetName);
					LeanTouch.OnFingerTap -= CallRunVehicle;
				}
			}
		}
		#endregion // PRIVATE_METHODS
	}
}
