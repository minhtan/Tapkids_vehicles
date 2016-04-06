using UnityEngine;
using System.Collections;

public class SpinWheel : MonoBehaviour {
	public AnimationCurve animationCurve;
	private bool isSpinning;

	void Start () {
		isSpinning = false;
	}

//	void Update () {
//		if (Input.GetMouseButtonDown (0) && !isSpinning) {
//			StartCoroutine (DoSpin (10.0f, Random.Range (2000, 3000)));
//		}
//	}

	public void Spin () {
		if (Input.GetMouseButtonDown (0) && !isSpinning) {
			StartCoroutine (SpinCo (10.0f, Random.Range (2000, 3000)));
		}
	}

	IEnumerator SpinCo (float time, float angle) {
		isSpinning = true;
		float timer = 0;
		float startAngle = transform.eulerAngles.z;

		while (timer < time) {
			float endAngle = animationCurve.Evaluate (timer/time) * angle;
			transform.eulerAngles = new Vector3 (0.0f, 0.0f, (endAngle + startAngle));
			timer += Time.deltaTime;
			yield return 0;
			isSpinning = false;
		}
	}
} 
