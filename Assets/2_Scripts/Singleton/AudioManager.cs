using UnityEngine;
using System.Collections.Generic;

public class AudioManager : UnitySingletonPersistent<AudioManager>
{
	[System.Serializable]
	public class AudioClipInfo : System.Object
	{
		public AudioClip audioClip;
		public AudioKey.UNIQUE_KEY uniqueKey;

		private AudioSource _audioSource;

		public AudioSource audioSource { set { _audioSource = value; } get { return _audioSource; } }
	}

	public enum CLIPTYPE
	{
		BACKGROUND,
		TEMPORARY
	}

	public AudioSource _backgroundSource;
	public AudioSource _tempoSource;
	public AudioSource _uiSource;

	public AudioClipInfo[] _backgroundAudios;
	public AudioClipInfo[] _tmpAudios;
	public AudioClipInfo[] _uiAudios;

	private Dictionary<AudioKey.UNIQUE_KEY, int> _clipInfoDict;

	void Awake ()
	{
		PreProcessingAudioArray ();
	}

	private void PreProcessingAudioArray ()
	{
		_clipInfoDict = new Dictionary<AudioKey.UNIQUE_KEY, int> ();
		AddAudioToDict (_backgroundAudios, _backgroundSource);
		AddAudioToDict (_tmpAudios, _tempoSource);
		AddAudioToDict (_uiAudios, _uiSource);
	}

	private void AddAudioToDict (AudioClipInfo[] audioArray, AudioSource audioSource)
	{
		for (int i = 0; i < audioArray.Length; i++) {
			_clipInfoDict.Add (audioArray [i].uniqueKey, i);
			audioArray [i].audioSource = audioSource;
		}
	}

	#region PLAY AUDIO

	public AudioSource PlayLoop (CLIPTYPE type, AudioKey.UNIQUE_KEY key)
	{
		AudioClipInfo clipInfo = GetAudioClipInfo (type, key);

		if (clipInfo == null)
			return null;

		clipInfo.audioSource.Stop ();
		clipInfo.audioSource.clip = clipInfo.audioClip;
		clipInfo.audioSource.Play ();

		return clipInfo.audioSource;
	}

	public AudioSource PlayOne (CLIPTYPE type, AudioKey.UNIQUE_KEY key)
	{
		AudioClipInfo clipInfo = GetAudioClipInfo (type, key);

		if (clipInfo == null)
			return null;

		clipInfo.audioSource.Stop ();
		clipInfo.audioSource.PlayOneShot (clipInfo.audioClip);
		return clipInfo.audioSource;
	}


	public AudioSource PlayOneCustomSource (CLIPTYPE type, AudioKey.UNIQUE_KEY key, AudioSource customSource)
	{

		AudioClipInfo clipInfo = GetAudioClipInfo (type, key);

		if (clipInfo == null)
			return null;

		clipInfo.audioSource = customSource;
		clipInfo.audioSource.Stop ();
		clipInfo.audioSource.PlayOneShot (clipInfo.audioClip);
		return clipInfo.audioSource;
	}

	#endregion

	#region STOP AUDIO

	public void StopClip (CLIPTYPE type, AudioKey.UNIQUE_KEY key)
	{

		AudioClipInfo clipInfo = GetAudioClipInfo (type, key);

		if (clipInfo == null)
			return;

		clipInfo.audioSource.Stop ();
		clipInfo.audioSource.clip = null;
	}

	public void StopAllSound ()
	{
		for (int i = 0; i < _backgroundAudios.Length; i++) {
			_backgroundAudios [i].audioSource.Stop ();
		}

		for (int i = 0; i < _tmpAudios.Length; i++) {
			_tmpAudios [i].audioSource.Stop ();
		}
	}

	public void PauseAllSound ()
	{
		for (int i = 0; i < _backgroundAudios.Length; i++) {
			_backgroundAudios [i].audioSource.Pause ();
		}

		for (int i = 0; i < _tmpAudios.Length; i++) {
			_tmpAudios [i].audioSource.Pause ();
		}
	}

	public void UnpauseAllSound ()
	{
		for (int i = 0; i < _backgroundAudios.Length; i++) {
			_backgroundAudios [i].audioSource.UnPause ();
		}

		for (int i = 0; i < _tmpAudios.Length; i++) {
			_tmpAudios [i].audioSource.UnPause ();
		}
	}

	public void SetGlobalVolume (float volume)
	{
		if (volume < 0f || volume > 1f)
			return;

		for (int i = 0; i < _backgroundAudios.Length; i++) {
			_backgroundAudios [i].audioSource.volume = volume;
		}

		for (int i = 0; i < _tmpAudios.Length; i++) {
			_tmpAudios [i].audioSource.volume = volume;
		}
	}

	# endregion

	# region GET METHODE

	private AudioClipInfo GetAudioClipInfo (CLIPTYPE type, AudioKey.UNIQUE_KEY key)
	{
		AudioClipInfo[] clipArray = type == CLIPTYPE.BACKGROUND ? _backgroundAudios : _tmpAudios;

		if (!_clipInfoDict.ContainsKey (key))
			return null;
		
		return clipArray [_clipInfoDict [key]];
	}

	public AudioClipInfo GetRandomBackgroundClipInfo ()
	{
		return _backgroundAudios [Random.Range (0, _backgroundAudios.Length)];
	}

	public AudioClipInfo GetRandomTemporaryClipInfo ()
	{
		return _tmpAudios [Random.Range (0, _tmpAudios.Length)];
	}

	# endregion
}
