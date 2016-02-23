using UnityEngine;
using System.Collections;

public class TargetControl : MonoBehaviour {

	public virtual void Init(){
		gameObject.SetActive (true);
	}

	public virtual void Reset(){
		gameObject.SetActive (false);
	}
}
