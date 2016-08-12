using UnityEngine;
using System.Linq;
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

//	private Dictionary <string, Transform> letterToTransform;
//	private Dictionary <string, Vector3> letterToPosition;

	private List<Transform> letters;
	private List<Transform> letterPositions;

	private List <GameObject> obstacleGameObjects;
	private Vehicle currentVehicle;
	private GameObject carGameObject;

	private Vector3 pointOffset = new Vector3 (0f, .5f, 0f);

	private float delayTime = .5f;
	private float scaleFactor = 3f;

	private Transform mTransform;
	#endregion private members

	#region MONO
	void OnEnable () {
		Messenger.AddListener <string, string> (EventManager.GameState.INIT.ToString(), HandleInitGame);
		Messenger.AddListener <bool> (EventManager.GameState.START.ToString (), HandleStartGame);
		Messenger.AddListener (EventManager.GameState.RESET.ToString (), HandleResetGame);
		Messenger.AddListener <string> (EventManager.GUI.REMOVE_LETTER.ToString (), HandleDropLetter);
//		Messenger.AddListener (EventManager.GUI.CORRECTWORD.ToString (), HandleCorrectWord);

	}

	void OnDisable () {
		Messenger.RemoveListener <string, string> (EventManager.GameState.INIT.ToString(), HandleInitGame);
		Messenger.RemoveListener <bool> (EventManager.GameState.START.ToString (), HandleStartGame);
		Messenger.RemoveListener (EventManager.GameState.RESET.ToString (), HandleResetGame);
		Messenger.RemoveListener <string> (EventManager.GUI.REMOVE_LETTER.ToString (), HandleDropLetter);
//		Messenger.AddListener (EventManager.GUI.CORRECTWORD.ToString (), HandleCorrectWord);
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
			SetupCar ();
//			StartCoroutine(SetupCar ());

//			letterToTransform = new Dictionary<string, Transform> ();
//			letterToPosition = new Dictionary<string, Vector3> ();
			letters = new List<Transform> ();
			letterPositions = new List<Vector3> ();

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
//		foreach (KeyValuePair <string, Transform> pair in letterToTransform) {
//			Transform.Destroy (pair.Value);
//		}
//		letterToTransform.Clear ();
		for (int i = 0; i < letters.Count; i++) {
			Transform.Destroy (letters[i]);
		}
		letters.Clear ();
	}

	private IEnumerator HandleGameStartCo () {
//		if (obstacleGameObjects.Count > 0) {
//			for (int i = 0; i < obstacleGameObjects.Count; i++) {
//				if (obstacleGameObjects[i] != null)
//					obstacleGameObjects[i].SetActive (true);
//				yield return new WaitForSeconds (delayTime);
//			}
//		}

//		if (letterToTransform.Count > 0) {
//			foreach (KeyValuePair <string, Transform> pair in letterToTransform) {
//				pair.Value.gameObject.SetActive (true);
//				pair.Value.transform.localScale = scaleFactor.ToVector3 ();
//				yield return new WaitForSeconds (delayTime);
//			}
//		}

		if (letters.Count > 0) {
			for (int i = 0; i < letters.Count; i++) {
				letters[i].gameObject.SetActive (true);
				letters[i].localScale = scaleFactor.ToVector3();
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

	private void SetupCar () {
		cartPoint = GameObject.FindWithTag ("CarPoint").transform;
		carGameObject = Instantiate(Resources.Load("Vehicles/" + PlayerDataController.Instance.mPlayer.vehicleName, typeof(GameObject)), cartPoint.position + pointOffset, Quaternion.identity) as GameObject;
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

	}

	private IEnumerator SetupCarCo () {
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

	private IEnumerator SetupLetter (string _letter, Vector3 _position) {
		yield return new WaitForSeconds (1f);
		StartCoroutine (AssetController.Instance.InstantiateGameObjectAsync (GameConstant.assetBundleName, _letter, (bundle) => {
			GameObject letterGameObject = Instantiate (bundle, _position + pointOffset, Quaternion.identity) as GameObject;
			letterGameObject.AddComponent <LetterController> ();
			letterGameObject.GetComponent <LetterController> ().letterName = _letter;
			letterGameObject.transform.SetParent (mTransform, false);
			letterGameObject.SetActive (false);
			letters.Add(letterGameObject.transform);
			letterPositions.Add(_position);
//			letterToTransform.Add (_letter, letterGameObject.transform);
//			letterToPosition.Add (_letter, _position);
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
//		if (letterToTransform.ContainsKey (_letters)) {
//			System.Random rnd = new System.Random ();
//			int rd;
//			do {
//				rd = rnd.Next (letterPoints.Length);
//			} while (letterToPosition.ContainsValue (letterPoints[rd].transform.position)); // wrong
//
//			Transform letter;
//			if (letterToTransform.TryGetValue (_letters, out letter)) {
//				letter.gameObject.SetActive (true);
//				letter.localPosition = letterPoints[rd].transform.position;
//				letter.localScale = scaleFactor.ToVector3 ();
//
//				letterToTransform [_letters] = letter;
//				letterToPosition [_letters] = letterPoints[rd].transform.position;
//			} else {
//
//			}
//		} else {
//
//		}


		System.Random rnd = new System.Random ();
		int rd;
		do {
			rd = rnd.Next (letterPoints.Length);
		} while (letterPositions.Contains (letterPoints[rd].transform.position)); // wrong

		for (int i = 0; i < letters.Count; i++) {
			if (_letters == letters[i].GetComponent<LetterController> ().letterName) {
				letters[i].gameObject.SetActive (true);
				letters[i].localPosition = letterPoints[rd].transform.position;
				letters[i].localScale = scaleFactor.ToVector3 ();

				letterPositions[i] = letters[i].localPosition;
			}
		}
	}

//	private void HandleCorrectWord () {
//		foreach (var letter in letterToTransform) {
//			if (!letter.Value.gameObject.activeInHierarchy) {
//				System.Random _random = new System.Random();
//				int rand;
//				do {
//					rand = _random.Next (letterPoints.Length);
//				} while (letterToPosition.ContainsValue (letterPoints[rand].transform.position));
//
//				letter.Value.gameObject.SetActive (true);
//				letter.Value.localPosition = letterPoints[rand].transform.position;
//				letter.Value.localScale = scaleFactor.ToVector3 ();
//
//				letterToPosition [letter.Key] = letterPoints[rand].transform.position;
//			} else {
//
//			}
//		}
//	}
	#endregion private functions
}
