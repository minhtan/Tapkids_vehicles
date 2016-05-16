using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using NotificationServices = UnityEngine.iOS.NotificationServices;
using NotificationType = UnityEngine.iOS.NotificationType;

public class NetworkManager : UnitySingletonPersistent<NetworkManager>
{
	public string[] _senderGcmIds;
	public string _getRequestTokenUrlIos = "http:/example.com?token=";
	public float _refreshNotiIosDelay = 3f;

	void Awake ()
	{
		base.Awake ();

		if (Application.platform == RuntimePlatform.IPhonePlayer)
			InitIosPush ();
		else if (Application.platform == RuntimePlatform.Android)
			InitGCM ();
	}

	public bool HasInternetAvailable ()
	{
		if (Application.internetReachability == NetworkReachability.ReachableViaCarrierDataNetwork ||
		    Application.internetReachability == NetworkReachability.ReachableViaLocalAreaNetwork) {
			return true;	
		}

		return false;
	}

	private void InitGCM ()
	{
		// Create receiver game object
		GCM.Initialize ();

		// Set callbacks
		GCM.SetErrorCallback ((string errorId) => {
			GCM.ShowToast ("Error" + errorId);
			Debug.Log (errorId);
		});

		GCM.SetMessageCallback ((Dictionary<string, object> table) => {
			string message = "";

			foreach (var key in  table.Keys) {
				message += key + "=" + table [key] + System.Environment.NewLine;
			}

			GCM.ShowToast ("Message " + message);
			Debug.Log (message);
		});

		GCM.SetRegisteredCallback ((string registrationId) => {
			Debug.Log (registrationId);
			GCM.ShowToast ("Registered " + registrationId);
		});

		GCM.SetUnregisteredCallback ((string registrationId) => {
			GCM.ShowToast ("Unregistered " + registrationId);
		});

		GCM.SetDeleteMessagesCallback ((int total) => {
			GCM.ShowToast ("DeleteMessaged " + total);
		});

		GCM.Register (_senderGcmIds);
	}

	private void InitIosPush ()
	{
		NotificationServices.RegisterForNotifications (NotificationType.Alert | NotificationType.Badge | NotificationType.Sound);
		StartCoroutine (IosRegisterPushCor ());
		StartCoroutine (IosPushReceiver());
	}

	private IEnumerator IosRegisterPushCor ()
	{
		bool isTokenSent = false;
		while (true) {
			byte[] token = NotificationServices.deviceToken;

			if (token != null) {
				string hexToken = "%" + System.BitConverter.ToString (token).Replace ('-', '%');
				new WWW (_getRequestTokenUrlIos + hexToken);
			}

			if (isTokenSent)
				yield break;
			
			yield return null;
		}
	}

	private int _messageCount = 0;
	public static UnityAction<UnityEngine.iOS.RemoteNotification> OnIosNotificationReceived;

	private IEnumerator IosPushReceiver ()
	{
		WaitForSeconds wait = new WaitForSeconds (_refreshNotiIosDelay);
		while (true) {

			if (NotificationServices.remoteNotificationCount != _messageCount) {
				_messageCount = NotificationServices.remoteNotificationCount;
				UnityEngine.iOS.RemoteNotification remoteNoti = NotificationServices.GetRemoteNotification (_messageCount - 1);

				if (OnIosNotificationReceived != null)
					OnIosNotificationReceived (remoteNoti);
			}
			yield return wait;
		}
	}
}
