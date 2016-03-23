using UnityEngine;
using System.Collections;

public class MenuScript : MonoBehaviour {

	#region Vars

	#endregion

	#region Mono
	void Awake(){

	}

	void Start () {
	
	}

	void Update () {
	
	}
	#endregion

	public void _LoadScene(int sceneID){
		SceneController.Instance.LoadingSceneAsync ((SceneController.SceneID)sceneID);
	}
}
