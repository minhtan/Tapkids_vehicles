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
	#endregion public members

	#region private members

	private Transform cartPoint;
	private GameObject[] letterPoints;	// stores letter's positions in environment
	private GameObject[] obstaclePoints;	

	private Dictionary <string, Transform> letterDictionary = new Dictionary<string, Transform> ();
	private List <GameObject> obstacleGameObjects;
	private Vehicle currentVehicle;
	private GameObject carGameObject;

	private Vector3 pointOffset = new Vector3 (0f, .5f, 0f);

	private float delayTime = .5f;
	private float scaleFactor = 2f;

	private Transform mTransform;
	#endregion private members

	#region MONO
	void OnEnable () {
		Messenger.AddListener <string, string> (EventManager.GameState.INIT.ToString(), HandleInitGame);
		Messenger.AddListener <bool> (EventManager.GameState.START.ToString (), HandleStartGame);
		Messenger.AddListener (EventManager.GameState.RESET.ToString (), HandleResetGame);

		Messenger.AddListener <string> (EventManager.GUI.REMOVE_LETTER.ToString (), HandleDropLetter);
	}

	void OnDisable () {
		Messenger.RemoveListener <string, string> (EventManager.GameState.INIT.ToString(), HandleInitGame);
		Messenger.RemoveListener <bool> (EventManager.GameState.START.ToString (), HandleStartGame);
		Messenger.RemoveListener (EventManager.GameState.RESET.ToString (), HandleResetGame);

		Messenger.RemoveListener <string> (EventManager.GUI.REMOVE_LETTER.ToString (), HandleDropLetter);
	}

	void Start () {
		mTransform = GetComponent <Transform> ();
	}
	#endregion MONO

	#region private functions
	private void HandleInitGame (string envLetter, string _letters) {
		StartCoroutine (SetupEnvironment(envLetter, () => {

			Renderer[] rendererComponents = GetComponentsInChildren<Renderer>(true);

			// Enable rendering:
			foreach (Renderer component in rendererComponents)
			{
				component.enabled = false;
			}

			StartCoroutine(SetupCar ());

			letterDictionary = new Dictionary<string, Transform> ();
//			foreach (KeyValuePair <string, GameObject> pair in letterDictionary) {
//				GameObject.Destroy (pair.Value);
//			}
//			letterDictionary.Clear ();

			letterPoints = GameObject.FindGameObjectsWithTag ("LetterPoint");
			for (int i = 0; i < _letters.Length; i++) {
				StartCoroutine(SetupLetter (_letters[i].ToString (), letterPoints[i].transform.position));
			}
		}));

		#region demo
		// setup obstacles
//		obstacleGameObjects = new List<GameObject> ();
//		for (int i = 0; i < obstacleGameObjects.Count; i++) {
//			GameObject.Destroy (obstacleGameObjects[i]);
//		}
//		obstacleGameObjects.Clear ();
//
//		obstaclePoints= GameObject.FindGameObjectsWithTag ("ObstaclePoint");
//		for (int i = 0; i < obstaclePoints.Length; i++) {
//			StartCoroutine (SetupObstacle (obstaclePoints[i].transform.position));
//		}
		#endregion demo
	}

	private void HandleStartGame (bool state) {
		if (state) {
			if (carGameObject != null)
				carGameObject.SetActive (true);

//			StopAllCoroutines ();
			StartCoroutine (HandleGameStartCo ());
		}
	}

	private void HandleResetGame () {
		// TODO: reset game
		// remove obstacle

		// remove letter
		foreach (KeyValuePair <string, Transform> pair in letterDictionary) {
//			GameObject.Destroy (pair.Value.gameObject);
			Transform.Destroy (pair.Value);
		}
		letterDictionary.Clear ();
	}

	private IEnumerator HandleGameStartCo () {
//		if (obstacleGameObjects.Count > 0) {
//			for (int i = 0; i < obstacleGameObjects.Count; i++) {
//				if (obstacleGameObjects[i] != null)
//					obstacleGameObjects[i].SetActive (true);
//				yield return new WaitForSeconds (delayTime);
//			}
//		}

		if (letterDictionary.Count > 0) {
			foreach (KeyValuePair <string, Transform> pair in letterDictionary) {
				pair.Value.gameObject.SetActive (true);
				pair.Value.transform.localScale = scaleFactor.ToVector3 ();
				yield return new WaitForSeconds (delayTime);
			}
		}
	}

	private IEnumerator SetupEnvironment (string _letter, System.Action _callback) {
		yield return new WaitForSeconds (1f);
		StartCoroutine (AssetController.Instance.InstantiateGameObjectAsync (GameConstant.assetBundleName, _letter + "_env", (bundle) => {
			GameObject letterEnv = Instantiate (bundle) as GameObject;
			letterEnv.transform.SetParent (mTransform);
			_callback ();
		}));
	}

	private IEnumerator SetupCar () {
		yield return new WaitForSeconds (1f);
		StartCoroutine (AssetController.Instance.InstantiateGameObjectAsync (GameConstant.assetBundleName, PlayerDataController.Instance.mPlayer.vehicleName, (bundle) => {
			cartPoint = GameObject.FindWithTag ("CarPoint").transform;
			carGameObject = Instantiate (bundle, cartPoint.position + pointOffset, Quaternion.identity) as GameObject;
			int carId = carGameObject.GetComponent <ArcadeCarController> ().vehicle.id;
			int matId;
			if (PlayerDataController.Instance.mPlayer.unlockedVehicles.ContainsKey (carId)) {
				if (PlayerDataController.Instance.mPlayer.unlockedVehicles.TryGetValue (carId, out matId)) {
					Renderer [] renderers = carGameObject.GetComponentsInChildren <Renderer> ();
					for (int j = 0; j < renderers.Length; j++) {
						renderers [j].material = carGameObject.GetComponent <ArcadeCarController> ().vehicle.carMats [matId].mat;
					}
				}
			}
			carGameObject.transform.SetParent (mTransform, false);
			carGameObject.SetActive (false);
		}));
	}

	private IEnumerator SetupLetter (string _letter, Vector3 position) {
		yield return new WaitForSeconds (1f);
		StartCoroutine (AssetController.Instance.InstantiateGameObjectAsync (GameConstant.assetBundleName, _letter, (bundle) => {
			GameObject letterGameObject = Instantiate (bundle, position + pointOffset, Quaternion.identity) as GameObject;
			letterGameObject.AddComponent <LetterController> ();
			letterGameObject.GetComponent <LetterController> ().letterName = _letter;
			letterGameObject.transform.SetParent (mTransform, false);
			letterGameObject.SetActive (false);
			letterDictionary.Add (_letter, letterGameObject.transform);
		}));
	}

	private IEnumerator SetupObstacle (Vector3 position) {
		yield return new WaitForSeconds (1f);
		StartCoroutine (AssetController.Instance.InstantiateGameObjectAsync (GameConstant.assetBundleName, "tree", (bundle) => {
			GameObject obstacleGameObject = Instantiate (bundle, position, Quaternion.identity) as GameObject;
			obstacleGameObject.AddComponent <ObstacleController> ();
			obstacleGameObject.transform.SetParent (mTransform, false);
			obstacleGameObject.SetActive (false);
			obstacleGameObjects.Add (obstacleGameObject);
		}));
	}

	private void HandleDropLetter (string _letters) {
		if (letterDictionary.ContainsKey (_letters)) {
			Transform letter;
			letterDictionary.TryGetValue (_letters, out letter);
			System.Random rnd = new System.Random ();
			int rd;
			do {
				rd = rnd.Next (letterPoints.Length);
			} while (letterDictionary.ContainsValue (letterPoints[rd].transform));

			letter.gameObject.SetActive (true);
			letter.localPosition = letterPoints[rd].transform.position;
			letter.localScale = scaleFactor.ToVector3 ();

			letterDictionary [_letters] = letter;
		} else {

		}
	}
	#endregion private functions
}
