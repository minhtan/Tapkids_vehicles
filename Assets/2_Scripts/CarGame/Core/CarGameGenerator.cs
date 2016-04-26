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

	private string assetBundleName = "car_asset";

	private Transform cartPoint;
	private GameObject[] letterPoints;	// stores letter's positions in environment
	private GameObject[] obstaclePoints;	

	private Dictionary <string, GameObject> letterDictionary;
	private List <GameObject> obstacleGameObjects;
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
		Messenger.AddListener <bool> (EventManager.GameState.STARTGAME.ToString (), HandleStartGame);
		Messenger.AddListener (EventManager.GameState.RESETGAME.ToString (), HandleResetGame);

		Messenger.AddListener <string> (EventManager.GUI.REMOVELETTER.ToString (), HandleDropLetter);
	}
	void OnDisable () {
		Messenger.RemoveListener <string> (EventManager.GameState.INITGAME.ToString(), HandleInitGame);
		Messenger.RemoveListener <bool> (EventManager.GameState.STARTGAME.ToString (), HandleStartGame);
		Messenger.RemoveListener (EventManager.GameState.RESETGAME.ToString (), HandleResetGame);

		Messenger.RemoveListener <string> (EventManager.GUI.REMOVELETTER.ToString (), HandleDropLetter);
	}

	void Start () {
		mTransform = GetComponent <Transform> ();

//		if (letterPoints.Length <= 0) 
//			Debug.Log ("Setup Error, There is no Letter Point in the environment");
//
//		if (obstaclePoints.Length <= 0) 
//			Debug.Log ("Setup Error, There is no Obstacle Point in the environment");
//		
//	
//		if (cartPoint == null) 
//			Debug.Log ("Setup Error, There is no Car Point in the environment");
	}

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
		cartPoint = GameObject.FindWithTag ("CarPoint").transform;
		StartCoroutine(SetupCar ());

		letterDictionary = new Dictionary<string, GameObject> ();
		foreach (KeyValuePair <string, GameObject> pair in letterDictionary) {
			GameObject.Destroy (pair.Value);
		}
		letterDictionary.Clear ();

		letterPoints = GameObject.FindGameObjectsWithTag ("LetterPoint");
		for (int i = 0; i < _word.Length; i++) {
			StartCoroutine(SetupLetter (_word[i].ToString (), letterPoints[i].transform.position));
		}

		// setup obstacles
		obstacleGameObjects = new List<GameObject> ();
		for (int i = 0; i < obstacleGameObjects.Count; i++) {
			GameObject.Destroy (obstacleGameObjects[i]);
		}
		obstacleGameObjects.Clear ();

		obstaclePoints= GameObject.FindGameObjectsWithTag ("ObstaclePoint");
		for (int i = 0; i < obstaclePoints.Length; i++) {
			StartCoroutine (SetupObstacle (obstaclePoints[i].transform.position));
		}
		#endregion demo
	}

	private void HandleStartGame (bool state) {
		if (state) {
			if (carGameObject != null)
				carGameObject.SetActive (true);

			StopAllCoroutines ();
			StartCoroutine (HandleGameStartCo ());
		}
	}

	private void HandleResetGame () {
		// TODO: reset game
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

		if (letterDictionary.Count > 0) {
			foreach (KeyValuePair <string, GameObject> pair in letterDictionary) {
				pair.Value.SetActive (true);
				pair.Value.transform.localScale = new Vector3 (2f, 2f, 2f);
				yield return new WaitForSeconds (delayTime);
			}
		}
	}

	private IEnumerator SetupCar () {
		yield return new WaitForSeconds (1f);
		StartCoroutine (AssetController.Instance.InstantiateGameObjectAsync (assetBundleName, "arcadecar", (bundle) => {
			carGameObject = Instantiate (bundle, cartPoint.position + pointOffset, Quaternion.identity) as GameObject;
			carGameObject.transform.SetParent (mTransform, false);
			carGameObject.SetActive (false);
		}));
	}

	private IEnumerator SetupLetter (string _letter, Vector3 position) {
		yield return new WaitForSeconds (1f);
		StartCoroutine (AssetController.Instance.InstantiateGameObjectAsync (assetBundleName, _letter, (bundle) => {
			GameObject letterGameObject = Instantiate (bundle, position + pointOffset, Quaternion.identity) as GameObject;
			letterGameObject.AddComponent <LetterController> ();
			letterGameObject.GetComponent <LetterController> ().letterName = _letter;
//			letterGameObject.transform.localScale = new Vector3 (2f, 2f, 2f);
			letterGameObject.transform.SetParent (mTransform, false);
			letterGameObject.SetActive (false);
			letterDictionary.Add (_letter, letterGameObject);
		}));
	}

	private IEnumerator SetupObstacle (Vector3 position) {
		yield return new WaitForSeconds (1f);
		StartCoroutine (AssetController.Instance.InstantiateGameObjectAsync (assetBundleName, "tree", (bundle) => {
			GameObject obstacleGameObject = Instantiate (bundle, position, Quaternion.identity) as GameObject;
			obstacleGameObject.AddComponent <ObstacleController> ();
			obstacleGameObject.transform.SetParent (mTransform, false);
			obstacleGameObject.SetActive (false);
			obstacleGameObjects.Add (obstacleGameObject);
		}));
	}

	private void HandleDropLetter (string _letters) {
		if (letterDictionary.ContainsKey (_letters)) {
			GameObject letter;
			letterDictionary.TryGetValue (_letters, out letter);
			letter.SetActive (true);
		}
	}
	#endregion private functions
}
