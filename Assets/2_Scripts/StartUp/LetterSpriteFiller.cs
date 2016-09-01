using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class LetterSpriteFiller : MonoBehaviour {

	static Color color = new Color32(255, 180, 234, 255);

	void Start () {
		StartCoroutine (SequenceFill (GetComponentsInChildren<Image> (), ()=> {
			Messenger.Broadcast(EventManager.GUI.LETTER_AUTODRAW_DONE.ToString());
		}));
	}

	IEnumerator SequenceFill(Image[] imgs, System.Action callback){
		int currentIndex = 0;
		int trackingIndex = -1;
		for (int i = 0; i < imgs.Length; i++) {
			imgs [i].fillAmount = 0f;
			imgs [i].color = color;
		}
		while(currentIndex < imgs.Length){
			if(trackingIndex != currentIndex){
				trackingIndex ++;
				StartCoroutine(Fill(imgs[trackingIndex], () => {
					currentIndex ++;
				}));
			}
			yield return null;
		}
		callback ();
	}
	
	IEnumerator Fill(Image img, System.Action callback){
		float fill = 0f;
		while (fill <= 1.1f) {
			img.fillAmount = Mathf.Clamp01(fill);
			yield return null;
			fill += Time.deltaTime * 0.8f;
		}
		callback ();
	}
}
