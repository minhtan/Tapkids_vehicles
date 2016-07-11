using UnityEngine;
using System.Collections.Generic;
using Lean;

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

	[System.Serializable]
	public class LocalizedAudioWrapper : System.Object
	{
		public string localizedKey;
		public AudioClipInfo[] backgroundAudios;
		public AudioClipInfo[] tmpAudios;
		public AudioClipInfo[] uiAudios;
	}

	public enum CLIPTYPE
	{
		BACKGROUND,
		TEMPORARY,
		UI
	}

	public AudioSource _backgroundSource;
	public AudioSource _tempSource;
	public AudioSource _uiSource;

	private AudioClipInfo[] _backgroundAudios;
	private AudioClipInfo[] _tmpAudios;
	private AudioClipInfo[] _uiAudios;

	public LocalizedAudioWrapper[] _localizedAudioWrappers;

	private string[] _languages;
	private LocalizedAudioWrapper _currentLocalized;
	private Dictionary<string, Dictionary<AudioKey.UNIQUE_KEY, CLIPTYPE>> _audioLocalizedDict;

	void Awake ()
	{
		base.Awake ();
		PreProcessingAudioArray ();
	}

	void OnEnable()
	{
		LeanLocalization.OnLocalizationChanged += OnLocalizationChanged;
	}

	void OnDisable()
	{
		LeanLocalization.OnLocalizationChanged -= OnLocalizationChanged;
	}

	private void OnLocalizationChanged()
	{
		_currentLocalized = GetCurrentLocalizedWrapper ();
	}

	private void PreProcessingAudioArray ()
	{
		_languages = Lean.LeanLocalization.Instance.Languages.ToArray ();
		_currentLocalized = GetCurrentLocalizedWrapper ();
		_audioLocalizedDict = new Dictionary<string, Dictionary<AudioKey.UNIQUE_KEY, CLIPTYPE>> ();

		for (int i = 0; i < _languages.Length; i++) {
			_audioLocalizedDict.Add (_languages [i], new Dictionary<AudioKey.UNIQUE_KEY, CLIPTYPE> ());
		}
			
		for (int i = 0; i < _localizedAudioWrappers.Length; i++) {
			string localizedKey = _localizedAudioWrappers [i].localizedKey;

			AddAudioToDict (_localizedAudioWrappers [i].backgroundAudios, _backgroundSource, CLIPTYPE.BACKGROUND, _audioLocalizedDict [localizedKey]);
			AddAudioToDict (_localizedAudioWrappers [i].tmpAudios, _tempSource, CLIPTYPE.TEMPORARY, _audioLocalizedDict [localizedKey]);
			AddAudioToDict (_localizedAudioWrappers [i].uiAudios, _uiSource, CLIPTYPE.UI, _audioLocalizedDict [localizedKey]);
		}
	}

	private void AddAudioToDict (AudioClipInfo[] audioArray, AudioSource audioSource, CLIPTYPE type, Dictionary<AudioKey.UNIQUE_KEY, CLIPTYPE> dict)
	{
		for (int i = 0; i < audioArray.Length; i++) {
			audioArray [i].audioSource = audioSource;
			dict.Add (audioArray [i].uniqueKey, type);
		}
	}

	#region PLAY AUDIO

	public void PlayAudio (AudioKey.UNIQUE_KEY key)
	{
		CLIPTYPE type;

		if (!_audioLocalizedDict [GetCurrentLocation ()].ContainsKey (key)) {
			LocalizedAudioWrapper defaultLocal = _localizedAudioWrappers [0];
			type = _audioLocalizedDict [defaultLocal.localizedKey] [key];

			if (type == CLIPTYPE.BACKGROUND)
				PlayLoop (defaultLocal, type, key);
			else
				PlayOne (defaultLocal, type, key);

			return;
		}
		
		type = _audioLocalizedDict [GetCurrentLocation ()] [key];

		if (type == CLIPTYPE.BACKGROUND)
			PlayLoop (_currentLocalized, type, key);
		else
			PlayOne (_currentLocalized, type, key);	
	}

	private AudioSource PlayLoop (LocalizedAudioWrapper localized, CLIPTYPE type, AudioKey.UNIQUE_KEY key)
	{
		AudioClipInfo clipInfo = GetAudioClipInfo (localized, type, key);

		if (clipInfo == null)
			return null;

		clipInfo.audioSource.Stop ();
		clipInfo.audioSource.clip = clipInfo.audioClip;
		clipInfo.audioSource.Play ();

		return clipInfo.audioSource;
	}

	private AudioSource PlayOne (LocalizedAudioWrapper localized, CLIPTYPE type, AudioKey.UNIQUE_KEY key)
	{
		AudioClipInfo clipInfo = GetAudioClipInfo (localized, type, key);

		if (clipInfo == null)
			return null;

		clipInfo.audioSource.Stop ();
		clipInfo.audioSource.PlayOneShot (clipInfo.audioClip);
		return clipInfo.audioSource;
	}

	public void PlayTemp(AudioClip clip, CLIPTYPE type = CLIPTYPE.TEMPORARY){
		_tempSource.Stop ();
		_tempSource.clip = clip;
		_tempSource.Play ();
	}

	public AudioSource PlayOneCustomSource (CLIPTYPE type, AudioKey.UNIQUE_KEY key, AudioSource customSource)
	{

		AudioClipInfo clipInfo = GetAudioClipInfo (_currentLocalized, type, key);

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

		AudioClipInfo clipInfo = GetAudioClipInfo (_currentLocalized, type, key);

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

		for(int i = 0; i < _uiAudios.Length; i++)
		{
			_uiAudios [i].audioSource.volume = volume;
		}
	}

	public void ToggleSound(bool state){
		_backgroundSource.enabled = state;
		_tempSource.enabled = state;
		_uiSource.enabled = state;
	}

	# endregion

	# region GET METHODE

	private AudioClipInfo GetAudioClipInfo (LocalizedAudioWrapper localizedWrapper, CLIPTYPE type, AudioKey.UNIQUE_KEY key)
	{
		AudioClipInfo[] audioArray = null;
		if (type == CLIPTYPE.BACKGROUND)
			audioArray = localizedWrapper.backgroundAudios;
		else if (type == CLIPTYPE.TEMPORARY)
			audioArray = localizedWrapper.tmpAudios;
		else if (type == CLIPTYPE.UI)
			audioArray = localizedWrapper.uiAudios;
		
		for (int i = 0; i < audioArray.Length; i++) {
			if (audioArray [i].uniqueKey == key)
				return audioArray [i];
		}	
		return null;
	}

	public AudioClipInfo GetRandomBackgroundClipInfo ()
	{
		return _backgroundAudios [Random.Range (0, _backgroundAudios.Length)];
	}

	public AudioClipInfo GetRandomTemporaryClipInfo ()
	{
		return _tmpAudios [Random.Range (0, _tmpAudios.Length)];
	}

	private string GetCurrentLocation ()
	{
		return Lean.LeanLocalization.Instance.CurrentLanguage;
	}

	private LocalizedAudioWrapper GetCurrentLocalizedWrapper ()
	{
		for (int i = 0; i < _localizedAudioWrappers.Length; i++) {
			if (_localizedAudioWrappers [i].localizedKey == GetCurrentLocation ()) {
				return _localizedAudioWrappers [i];
			}
		}
		return null;
	}

	# endregion
}
