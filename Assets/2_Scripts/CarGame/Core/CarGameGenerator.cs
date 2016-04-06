using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CarGameGenerator : MonoBehaviour {

	// techniques: 
	// - asset bundle 
	// - object pool 
	// handles:
	// - remove old letters then genrate new letters at init game state
	// - re generate missing letters after player gather word, whether true or false

	#region public members
	public GameObject[] letterPrefabs; 	// Todo: should replace this prefab hard references with loading assetbundle method

	public GameObject carPrefab;
	#endregion public members

	#region private members

	private string assetBundleName = "car_asset";

	private Transform cartPoint;
	private GameObject[] letterPoints;	// stores letter's positions in environment

	private List<GameObject> letterGameObjects;		// stores game object letter
//	private Dictionary <int, GameObject> IndexToLetterDictionary = new Dictionary<int, GameObject> ();

	private Vehicle currentVehicle;
	private GameObject car;

	private Vector3 pointOffset = new Vector3 (0f, 2f, 0f);


	private Transform mTransform;
	#endregion private members
	//

	#region MONO

	void OnEnable () {

		CarGameEventController.InitGame += OnInitGame;
		CarGameEventController.ValidateWord += OnValidateWord;
	}
	void OnDisable () {

		CarGameEventController.InitGame -= OnInitGame;
		CarGameEventController.ValidateWord -= OnValidateWord;
	}

	void Start () {
		mTransform = GetComponent <Transform> ();

		letterGameObjects = new List<GameObject> ();
		letterPoints = GameObject.FindGameObjectsWithTag ("LetterPoint");
		if (letterPoints.Length <= 0) 
			Debug.Log ("Setup Error, There is no Letter Point in the environment");


		cartPoint = GameObject.FindWithTag ("CarPoint").transform;
		if (cartPoint == null) 
			Debug.Log ("Setup Error, There is no Car Point in the environment");
	}

	#endregion MONO

	#region private functions
	private IEnumerator GetCarAssetBundle () {
		currentVehicle = PlayerDataController.Instance.mPlayer.currentVehicle;

		yield return StartCoroutine (AssetController.Instance.InstantiateGameObjectAsync (assetBundleName, currentVehicle.name, (bundle) => {
			GameObject carGO = Instantiate (bundle) as GameObject;
			carGO.transform.SetParent (mTransform);
		}));
	}

	// get letter from asset bundle then put it into a list
	private IEnumerator GetLetterAssetBundle (string _letter) {
		yield return StartCoroutine (AssetController.Instance.InstantiateGameObjectAsync (assetBundleName, _letter, (bundle) => {
			for (int i = 0; i < _letter.Length; i++) {
				letterGameObjects.Add (Instantiate (bundle) as GameObject);
			}
		}));
	}

	private void OnInitGame (string _letter) {
		// demo
		if (car == null)
		{
			car = Instantiate (carPrefab, cartPoint.position + pointOffset, Quaternion.identity) as GameObject;
			car.transform.SetParent (mTransform);
		} else {
			car.transform.position = cartPoint.position + pointOffset;
			car.transform.rotation = Quaternion.identity;
		}

		// demo
		for (int i = 0; i < letterGameObjects.Count; i++) {
			GameObject.Destroy (letterGameObjects[i]);
		}
		letterGameObjects.Clear ();
		for (int i = 0; i < _letter.Length; i++) {
			for (int j = 0; j < letterPrefabs.Length; j++) {
				if (_letter[i].ToString ().Equals (letterPrefabs[j].name)) {
					GameObject letterGameObject = (GameObject) Instantiate (letterPrefabs [j], letterPoints [i].transform.position + pointOffset, Quaternion.identity);
					letterGameObject.transform.SetParent (mTransform);
					letterGameObjects.Add (letterGameObject);
				}
			}
		}

		//TODO: pending for real asset bundle server
		// StartCoroutine (GetAssetBundle ());
		// StartCoroutine (GetLetterAssetBundle ());
	}

	private void OnValidateWord () {
		// re-enable letters
		for (int i = 0; i < letterGameObjects.Count; i++) {
			if(!letterGameObjects[i].activeInHierarchy) 
				letterGameObjects[i].SetActive (true);
		}
	}
	#endregion private functions
}
