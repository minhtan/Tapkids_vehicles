using UnityEngine;
using System.Collections;
using WordDraw;

public class DestroyOnLoad : MonoBehaviour {

	public void DestroyGO() {
		Destroy(GameObject.Find ("VectorCanvas"));
	}
}
