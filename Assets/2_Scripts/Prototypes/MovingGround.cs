using UnityEngine;
using System.Collections;

public class MovingGround : MonoBehaviour {
	Renderer mRenderer;
	// Use this for initialization
	void Start () {
		mRenderer = GetComponent<Renderer>();
	}
	//Material texture offset rate
	float speed = .5f;

	//Offset the material texture at a constant rate
	void Update () {
		float offset = Time.time * speed;                             
		mRenderer.material.mainTextureOffset = new Vector2(0, -offset); 

	}
}
