using UnityEngine;
using System.Collections;

public class TestHeavyLoading : MonoBehaviour {

	// Use this for initialization
	void Hello () {
		GameObject heavyPrefab = new GameObject ("HeavyPrefab");

		for(int i = 0; i < 10000; i++)
		{
			GameObject.CreatePrimitive (PrimitiveType.Sphere).transform.parent = heavyPrefab.transform;
		}
	}
}
