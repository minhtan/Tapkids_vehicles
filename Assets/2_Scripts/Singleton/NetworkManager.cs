using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NetworkManager : UnitySingletonPersistent<NetworkManager>
{
	public string[] _senderIds;

	void Awake()
	{
		base.Awake ();
		InitGCM ();	
		GCM.Register (_senderIds);
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
		});

		GCM.SetMessageCallback ((Dictionary<string, object> table) => {
			string message = "";

			foreach (var key in  table.Keys) {
				message += key + "=" + table [key] + System.Environment.NewLine;
			}

			GCM.ShowToast ("Message " + message);
		});

		GCM.SetRegisteredCallback ((string registrationId) => {
			GCM.ShowToast ("Registered " + registrationId);
		});

		GCM.SetUnregisteredCallback ((string registrationId) => {
			GCM.ShowToast ("Unregistered " + registrationId);
		});

		GCM.SetDeleteMessagesCallback ((int total) => {
			GCM.ShowToast ("DeleteMessaged " + total);
		});
	}
}
