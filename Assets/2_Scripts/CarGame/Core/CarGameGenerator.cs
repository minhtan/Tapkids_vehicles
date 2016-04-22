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
//	public GameObject[] letterPrefabs; 	// Todo: should replace this prefab hard references with loading assetbundle method
//
//	public GameObject[] obstaclePrefabs;
//
//	public GameObject carPrefab;

	#endregion public members

	#region private members

	private string assetBundleName = "car_asset";

	private Transform cartPoint;
	private GameObject[] letterPoints;	// stores letter's positions in environment
	private GameObject[] obstaclePoints;

	private Dictionary <string, GameObject> letterPointDictionary = new Dictionary<string, GameObject> ();

	private List<GameObject> letterGameObjects;		// stores game object letter
//	private Dictionary <int, GameObject> IndexToLetterDictionary = new Dictionary<int, GameObject> ();

	private List<GameObject> obstacleGameObjects;
	private Vehicle currentVehicle;
	private GameObject carGameObject;

	private Vector3 pointOffset = new Vector3 (0f, .5f, 0f);

	private float delayTime = .5f;
	private Transform mTransform;
	#endregion private members
	//

	#region MONO

	void OnEnable () {

		Messenger.AddListener <string> (EventManager.GameState.INITGAME.ToString(), HandleInitGame);
		Messenger.AddListener (EventManager.GameState.STARTGAME.ToString (), HandleStartGame);
		Messenger.AddListener (EventManager.GameState.RESETGAME.ToString (), HandleResetGame);

		Messenger.AddListener <string> (EventManager.GUI.REMOVELETTER.ToString (), HandleDropLetter);
	}
	void OnDisable () {
		Messenger.RemoveListener <string> (EventManager.GameState.INITGAME.ToString(), HandleInitGame);
		Messenger.RemoveListener (EventManager.GameState.STARTGAME.ToString (), HandleStartGame);
		Messenger.RemoveListener (EventManager.GameState.RESETGAME.ToString (), HandleResetGame);

		Messenger.RemoveListener <string> (EventManager.GUI.REMOVELETTER.ToString (), HandleDropLetter);
	}

	void Start () {
		mTransform = GetComponent <Transform> ();

		letterGameObjects = new List<GameObject> ();
		letterPoints = GameObject.FindGameObjectsWithTag ("LetterPoint");

		if (letterPoints.Length <= 0) 
			Debug.Log ("Setup Error, There is no Letter Point in the environment");

		obstacleGameObjects = new List<GameObject> ();
		obstaclePoints= GameObject.FindGameObjectsWithTag ("ObstaclePoint");
		if (obstaclePoints.Length <= 0) 
			Debug.Log ("Setup Error, There is no Obstacle Point in the environment");
		
		cartPoint = GameObject.FindWithTag ("CarPoint").transform;
		if (cartPoint == null) 
			Debug.Log ("Setup Error, There is no Car Point in the environment");
	}

//	void Update () {
//		if (Input.GetKeyDown (KeyCode.Space)) {
//			StartCoroutine (AssetController.Instance.InstantiateGameObjectAsync ("car_asset", "c", (bundle) => {
//				GameObject go = Instantiate (bundle) as GameObject;
//			}));
//		}
//	}
	#endregion MONO

	#region private functions
	IEnumerable<int> UniqueRandom(int minInclusive, int maxInclusive)
	{
		List<int> candidates = new List<int>();
		for (int i = minInclusive; i <= maxInclusive; i++)
		{
			candidates.Add(i);
		}
		System.Random rnd = new System.Random();
		while (candidates.Count > 0)
		{
			int index = rnd.Next(candidates.Count);
			yield return candidates[index];
			candidates.RemoveAt(index);
		}
	}

	private void HandleInitGame (string _word) {
		#region demo
		SetupCar ();

		// setup letters
		for (int i = 0; i < letterGameObjects.Count; i++) {
			GameObject.Destroy (letterGameObjects[i]);
		}
		letterGameObjects.Clear ();
		for (int i = 0; i < _word.Length; i++) {
			SetupLetter (_word[i].ToString (), letterPoints[i].transform.position);
		}


		// setup obstacles
		for (int i = 0; i < obstacleGameObjects.Count; i++) {
			GameObject.Destroy (obstacleGameObjects[i]);
		}
		obstacleGameObjects.Clear ();
		for (int i = 0; i < obstaclePoints.Length; i++) {
			SetupObstacle (obstaclePoints[i].transform.position);
		}
		#endregion demo
	}

	private void HandleStartGame () {
		carGameObject.SetActive (true);

		StopAllCoroutines ();
		StartCoroutine (HandleGameStartCo ());
	}

	private void HandleResetGame () {
		// remove obstacle
		// remove letter
	}

	private IEnumerator HandleGameStartCo () {
		if (obstacleGameObjects.Count > 0) {
			for (int i = 0; i < obstacleGameObjects.Count; i++) {
				if (obstacleGameObjects[i] != null)
					obstacleGameObjects[i].SetActive (true);
				yield return new WaitForSeconds (delayTime);
			}
		}

//		if (letterGameObjects.Count > 0) {
//			for (int i = 0; i < letterGameObjects.Count; i++) {
//				if (letterGameObjects [i] != null)
//					letterGameObjects[i].SetActive (true);
//				yield return new WaitForSeconds (delayTime);
//			}
//		}
//
		if (letterPointDictionary.Count > 0) {
			foreach (KeyValuePair <string, GameObject> pair in letterPointDictionary) {
				pair.Value.SetActive (true);
				yield return new WaitForSeconds (delayTime);
			}
		}
	}

	private void SetupCar () {
		StartCoroutine (AssetController.Instance.InstantiateGameObjectAsync (assetBundleName, "arcadecar", (bundle) => {
			carGameObject = Instantiate (bundle, cartPoint.position + pointOffset, Quaternion.identity) as GameObject;
			carGameObject.transform.SetParent (mTransform, false);
			carGameObject.SetActive (false);
		}));
	}

	private void SetupLetter (string _letter, Vector3 position) {
		StartCoroutine (AssetController.Instance.InstantiateGameObjectAsync (assetBundleName, _letter, (bundle) => {
			GameObject letterGameObject = Instantiate (bundle, position + pointOffset, Quaternion.identity) as GameObject;
			letterGameObject.AddComponent <LetterController> ();
			letterGameObject.GetComponent <LetterController> ().letterName = _letter;
			letterGameObject.transform.SetParent (mTransform, false);
			letterGameObject.SetActive (false);
//			letterGameObjects.Add (letterGameObject);
			letterPointDictionary.Add (_letter, letterGameObject);
		}));
	}

	private void SetupObstacle (Vector3 position) {
		StartCoroutine (AssetController.Instance.InstantiateGameObjectAsync (assetBundleName, "tree", (bundle) => {
			GameObject obstacleGameObject = Instantiate (bundle, position, Quaternion.identity) as GameObject;
			obstacleGameObject.AddComponent <ObstacleController> ();
			obstacleGameObject.transform.SetParent (mTransform, false);
			obstacleGameObject.SetActive (false);
			obstacleGameObjects.Add (obstacleGameObject);
		}));
	}

	private void HandleDropLetter (string _letters) {
		if (letterPointDictionary.ContainsKey (_letters)) {
			GameObject letter;
			letterPointDictionary.TryGetValue (_letters, out letter);
			letter.SetActive (true);
		}
	}
	#endregion private functions
}
