using UnityEngine;
using System.Collections;
using System;
using System.Linq;

public class Test : MonoBehaviour {

	public enum myList {
		item1,
		item2,
		item3,
		item4,
		item5
	}
	// Use this for initialization
	void Start () {
		Debug.Log (myList.item1);

		Debug.Log (Enum.GetValues(typeof(myList)).Cast<myList>().Max());
		Debug.Log (Enum.GetValues(typeof(myList)).Cast<myList>().Min());
		Debug.Log (myList.item1 + 1);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

}
