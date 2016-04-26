using UnityEngine;
using System.Collections;

public class NetworkManager : UnitySingletonPersistent<NetworkManager>
{

	public bool HasInternetAvailable ()
	{
		if (Application.internetReachability == NetworkReachability.ReachableViaCarrierDataNetwork ||
		    Application.internetReachability == NetworkReachability.ReachableViaLocalAreaNetwork) {
			return true;
		}

		return false;
	}
}
