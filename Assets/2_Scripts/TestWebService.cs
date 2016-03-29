using UnityEngine;
using System.Collections;

public class TestWebService : MonoBehaviour {

	void Start(){
		StartCoroutine(WebServiceUltility.CheckKey("", "", (returnData) => {
			if(returnData != null){
				Debug.Log(returnData.success);
			}else{
				Debug.Log("Failed to connect");
			}
		}));
	}

}
