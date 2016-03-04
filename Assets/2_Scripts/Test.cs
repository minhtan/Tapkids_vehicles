using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class Test : MonoBehaviour {

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

	public void Load(){
		SceneManager.LoadScene ("WordGame");
	}
}
