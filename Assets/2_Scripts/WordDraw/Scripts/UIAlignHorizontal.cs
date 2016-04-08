using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UIAlignHorizontal : MonoBehaviour {

	public float _distance;

	// Use this for initialization
	void Awake () {
		RectTransform rootRectTrans = GetComponent<RectTransform> ();

		Vector3 tmp = rootRectTrans.localPosition;
		tmp.y = Screen.height / 4;

		transform.position = tmp;

		RectTransform[] rectTrans = new RectTransform[transform.childCount];

		for(int i = 0; i < transform.childCount; i++)
		{
			rectTrans [i] = transform.GetChild (i).GetComponent<RectTransform>();
		}

		for(int i = 0; i < rectTrans.Length - 1; i++)
		{
			Vector3 pos = rectTrans [i].localPosition;
			pos.x += _distance;
			rectTrans [i + 1].localPosition = pos;
		}
	}
}
