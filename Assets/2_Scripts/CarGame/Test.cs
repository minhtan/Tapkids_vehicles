using UnityEngine;
using System.Collections;

public class Test : MonoBehaviour {

	// Use this for initialization
	void Start () {
		StartCoroutine (GetCarAssetBundle());
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	private IEnumerator GetCarAssetBundle () {

		yield return StartCoroutine (AssetController.Instance.InstantiateGameObjectAsync ("car_asset", "a", (bundle) => {
			GameObject carGO = Instantiate (bundle) as GameObject;
		}));
	}
}
