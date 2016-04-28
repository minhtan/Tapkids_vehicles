using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GaragaController : MonoBehaviour {

	// TODO: 
	// => need a list string of vehicles
	// 
	// pre setup 26 vehicles slot
	// get car model based on player unlocked list
	// 

	#region public members
	public int currentSelectedCar;
	#endregion public members

	#region private members
	private GameObject[] vehicleDemos;
	private Transform mTransform;
//	private Vehicle[] vehicles;
	#endregion private members

	#region Mono
	void Awake () {
		mTransform = GetComponent <Transform> ();
	}
	void OnEnable () {
		Messenger.AddListener <int> (EventManager.GUI.SELECTCAR.ToString (), OnSelectCar);
	}

	void OnDisable () {
		Messenger.RemoveListener <int> (EventManager.GUI.SELECTCAR.ToString (), OnSelectCar);
	}

	void Start () {
		// get player unlocked list

		// compare with avaiable vehicle list

		// then setup garage
		for (int i = 0; i < GameConstant.vehicles.Count; i++) {
			StartCoroutine (SetupCar (GameConstant.vehicles[i]));
		}

		// demo setup garage
//		vehicleDemos = new GameObject[transform.childCount];
//		for (int i = 0; i < vehicleDemos.Length; i++) {
//			vehicleDemos[i] = transform.GetChild (i).gameObject;
//			if (PlayerDataController.Instance.mPlayer.currentVehicle.id == i) 
//				vehicleDemos[i].SetActive (true);
//			else
//				vehicleDemos[i].SetActive (false);
//		}
	}
	private IEnumerator SetupCar (string vehicleName) {
		yield return new WaitForSeconds (1f);
		StartCoroutine (AssetController.Instance.InstantiateGameObjectAsync (GameConstant.assetBundleName, vehicleName, (bundle) => {
			GameObject carGameObject = Instantiate (bundle) as GameObject;
			carGameObject.transform.localPosition = Vector3.zero;
			Destroy (carGameObject.GetComponent <ArcadeCarUserController> ());
			Destroy (carGameObject.GetComponent <ArcadeCarController> ());
			Destroy (carGameObject.GetComponent <Rigidbody> ());
			carGameObject.transform.SetParent (mTransform, false);
			carGameObject.SetActive (false);
		}));
	}
	void Update () {
		if (Input.GetKeyDown (KeyCode.Space)) {
			Debug.Log (AssetController.Instance.GetTotalLoadedAssetBundle ().ToString ());
		}
	}
	#endregion Mono

	#region public functions
	#endregion public functions

	#region private functions
	private void OnSelectCar (int _index) {

		// handle car modle
		if (currentSelectedCar + _index >= 0 && currentSelectedCar + _index < vehicleDemos.Length)
		{
			vehicleDemos [currentSelectedCar].SetActive (false);
			currentSelectedCar += _index;
			vehicleDemos [currentSelectedCar].SetActive (true);
		}

		// TODO: update car stats in GUI
		// is this unlocked vehicle ?
	}

	// TODO: load vehicle only it is unlocked one
	private IEnumerator LoadVehicleFromAssetBundle (string _assetBundleName, string _vehicleName) {
		yield return StartCoroutine (AssetController.Instance.InstantiateGameObjectAsync (_assetBundleName, _vehicleName, (bundle) => {
			// GameObject vehicle = Instantiate (bundle) as GameObject;
			// add vehicle to garage vehicles list
		}));
	}
	#endregion private functions

}
