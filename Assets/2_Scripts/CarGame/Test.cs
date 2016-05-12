using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class Test : MonoBehaviour {

	bool isConfirmed;

	void OnEnable () {

	}

	void Start () {

	}

	void OnDisable () {

	}

	public void ActionExample (Action _onComplete) {
		_onComplete ();
	}


}
