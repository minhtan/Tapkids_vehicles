using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityStandardAssets.ImageEffects;
using UnityEngine.SceneManagement;

public class ImageSequenceSingleSprite : MonoBehaviour
{

    private Sprite sprite;
    private Image img;
    private int frameCounter = 0;

    public string folderName;
    public string imageSequenceName;
    public int numberOfFrames;
    private string baseName;

    void Awake()
    {
        img = GetComponent<Image>();
        this.baseName = this.folderName + "/" + this.imageSequenceName;
    }

    void OnEnable()
    {
        img.sprite = (Sprite)Resources.Load(baseName + "00024", typeof(Sprite));
        frameCounter = 23;
    }


    void Start()
    {
        StartCoroutine("Play", 0.04f);
    }

    //The following methods return a IEnumerator so they can be yielded:  
    //A method to play the animation in a loop  
    IEnumerator PlayLoop(float delay)
    {
        //wait for the time defined at the delay parameter  
        yield return new WaitForSeconds(delay);

        //advance one frame  
        frameCounter = (++frameCounter) % numberOfFrames;

        //load the current frame  
        img.sprite = (Sprite)Resources.Load(baseName + frameCounter.ToString("D5"), typeof(Sprite));

        //Stop this coroutine  
        //		StopCoroutine ("PlayLoop");  
    }

    //A method to play the animation just once  
    IEnumerator Play(float delay)
    {
        //wait for the time defined at the delay parameter  
        //yield return new WaitForSeconds (delay);    
        //if it isn't the last frame  
		float time = 0;
        while (frameCounter < numberOfFrames - 1)
        {
            //Advance one frame  
			time = time + 0.04f;
            ++frameCounter;
            yield return new WaitForSeconds(delay);
            //load the current frame  
            img.sprite = (Sprite)Resources.Load(baseName + frameCounter.ToString("D5"), typeof(Sprite));
            Resources.UnloadUnusedAssets();
        }
        //Stop this coroutine  
        StopCoroutine ("Play");
		OnSequenceEnd ();
    }

    public void SkipSequence()
    {
        StopAllCoroutines();
        img.sprite = (Sprite)Resources.Load(baseName + (numberOfFrames-1).ToString("D5"), typeof(Sprite));
		OnSequenceEnd ();
    }

	void OnSequenceEnd(){

	}
}