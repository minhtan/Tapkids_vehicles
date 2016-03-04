using UnityEngine;
using System.Collections;

public class SoundController : UnitySingletonPersistent<SoundController>
{
	private AudioSource audioSource;

    public override void Awake()
    {
        base.Awake();
        audioSource = gameObject.GetComponent<AudioSource>();
    }

    public void PlaySound(AudioClip clipToPlay) {
        audioSource.Stop();
        audioSource.clip = clipToPlay;
        audioSource.Play();
    }
}
