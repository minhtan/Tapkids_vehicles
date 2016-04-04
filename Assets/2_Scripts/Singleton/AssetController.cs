using UnityEngine;
using System.Collections;
using AssetBundles;
using System;

public class AssetController : UnitySingletonPersistent<AssetController> {

	#region Vars
	private string url = "http://103.27.239.161/Upload/";
	public const string AssetBundlesOutputPath = "/AssetBundles/";
	public static string bundleName = "car_asset";
	#endregion

	#region Mono
	IEnumerator Start (){
		yield return StartCoroutine( Initialize() );
	}
	#endregion

	// Initialize the downloading url and AssetBundleManifest object.
	protected IEnumerator Initialize()
	{
		// Don't destroy this gameObject as we depend on it to run the loading script.
		DontDestroyOnLoad(gameObject);

		// With this code, when in-editor or using a development builds: Always use the AssetBundle Server
		// (This is very dependent on the production workflow of the project. 
		// 	Another approach would be to make this configurable in the standalone player.)
		#if DEVELOPMENT_BUILD || UNITY_EDITOR
		AssetBundleManager.SetDevelopmentAssetBundleServer ();
		//AssetBundleManager.SetSourceAssetBundleURL(url);
		#else
		// Use the following code if AssetBundles are embedded in the project for example via StreamingAssets folder etc:
		//AssetBundleManager.SetSourceAssetBundleURL(Application.dataPath + "/");
		//Or customize the URL based on your deployment or configuration
		AssetBundleManager.SetSourceAssetBundleURL(url);
		#endif

		// Initialize AssetBundleManifest which loads the AssetBundleManifest object.
		var request = AssetBundleManager.Initialize();
		if (request != null) {
			yield return StartCoroutine (request);
		}
	}

	public IEnumerator InstantiateGameObjectAsync (string assetBundleName, string assetName, Action<GameObject> callback)
	{
		// This is simply to get the elapsed time for this phase of AssetLoading.
		float startTime = Time.realtimeSinceStartup;

		// Load asset from assetBundle.
		AssetBundleLoadAssetOperation request = AssetBundleManager.LoadAssetAsync(assetBundleName, assetName, typeof(GameObject) );
		if (request == null)
			yield break;
		yield return StartCoroutine(request);

		// Get the asset.
		GameObject prefab = request.GetAsset<GameObject> ();

		if (prefab != null) {
			callback (prefab);
		}

		// Calculate and display the elapsed time.
		float elapsedTime = Time.realtimeSinceStartup - startTime;
		Debug.Log(assetName + (prefab == null ? " was not" : " was")+ " loaded successfully in " + elapsedTime + " seconds" );
	}

	/* use as follow
	 private IEnumerator mymethod ()
		{
			yield return StartCoroutine (InstantiateGameObjectAsync (assetBundleName, assetName, (bundle) => {
				
			}));
		}
	*/

}
