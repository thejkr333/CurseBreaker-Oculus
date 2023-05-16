using UnityEngine;
using System.Collections.Generic;
using System.Collections;

[System.Serializable]
public class Sound
{
    public string Name;
    public AudioClip Clip;

    [Range(0f, 1f)]
    public float Volume = 1f;

    [Range(-3f, 3f)]
    public float Pitch = 1f;

    public bool Loop = false;

    [Range(0,1)]
    public float SpatialBlend = .5f;

    public float MaxDistance = 500f;
    public AudioRolloffMode RolloffMode = AudioRolloffMode.Logarithmic;
}

[System.Serializable]
public class MusicTrack
{
    public string Name;
    public AudioClip Clip;
}

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    public List<Sound> Sounds = new List<Sound>();
    public List<MusicTrack> Music = new List<MusicTrack>();
    public List<MusicTrack> EasterEggs = new List<MusicTrack>();

    private Dictionary<string, AudioSource> soundSources = new Dictionary<string, AudioSource>();
    private Dictionary<string, AudioClip> musicClips = new Dictionary<string, AudioClip>();
    private Dictionary<string, AudioClip> easterEggClips = new Dictionary<string, AudioClip>();
    private AudioSource musicSource, easterEggSource;

    [Range(0f, 1f)]
    public float MusicVolume = 1f;

    [Range(0f, 1f)]
    public float SfxVolume = 1f;

    [SerializeField] private float fadeDuration = 1f;

    private bool isMuted;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        // Load volume levels from PlayerPrefs
        MusicVolume = PlayerPrefs.GetFloat("MusicVolume", 1f);
        SfxVolume = PlayerPrefs.GetFloat("SFXVolume", 1f);

        // Create AudioSources for each Sound object
        foreach (Sound sound in Sounds)
        {
            AudioSource _source = gameObject.AddComponent<AudioSource>();
            _source.clip = sound.Clip;
            _source.volume = sound.Volume;
            _source.pitch = sound.Pitch;
            _source.playOnAwake = false;
            _source.loop = sound.Loop;
            _source.rolloffMode = sound.RolloffMode;
            _source.maxDistance = sound.MaxDistance;
            _source.spatialBlend = sound.SpatialBlend;
            soundSources[sound.Name] = _source;
        }

        // Initialize musicSource
        foreach (MusicTrack music in Music)
        {
            musicClips[music.Name] = music.Clip;
        }
        musicSource = gameObject.AddComponent<AudioSource>();
        musicSource.loop = true;
        musicSource.volume = 1f;
        musicSource.pitch = 1f;

        // Initialize easterEggSource
        foreach (MusicTrack easterEgg in EasterEggs)
        {
            easterEggClips[easterEgg.Name] = easterEgg.Clip;
        }
        easterEggSource = gameObject.AddComponent<AudioSource>();
        easterEggSource.loop = false;
        easterEggSource.volume = 1f;
        easterEggSource.pitch = 1f;

        string _startingMusic = Music[Random.Range(1, Music.Count)].Name;
        PlayMusic(_startingMusic);
    }

    #region SFX
    public void PlaySoundStatic(string name, Vector3 position, float pitch = 1f)
    {
        if (soundSources.ContainsKey(name))
        {
            Sound _sound = Sounds.Find(s => s.Name == name);

            if (_sound == null)
            {
                Debug.LogWarning("AudioManager: Sound not found - " + name);
                return;
            }

            AudioSource _source = soundSources[name];
            _source.transform.position = position;
            _source.volume = SfxVolume;
            _source.pitch = pitch;
            _source.Play();
        }
        else
        {
            Debug.LogWarning("AudioManager: Sound not found - " + name);
        }
    }

    public void PlaySoundDynamic(string name, GameObject followObject, float pitch = 1)
    {
        if (soundSources.ContainsKey(name))
        {
            if (followObject == null) return;

            if (!followObject.TryGetComponent<AudioSource>(out AudioSource _source))
            {
                _source = followObject.AddComponent<AudioSource>();
            }

            _source.clip = soundSources[name].clip;
            _source.volume = soundSources[name].volume;
            _source.pitch = pitch;
            _source.playOnAwake = false;
            _source.loop = soundSources[name].loop;
            _source.rolloffMode = soundSources[name].rolloffMode;
            _source.maxDistance = soundSources[name].maxDistance;
            _source.spatialBlend = soundSources[name].spatialBlend;
            _source.Play();

            if (!_source.loop)
            {
                Destroy(_source, _source.clip.length);
            }
        }
        else
        {
            Debug.LogWarning("AudioManager: Sound not found - " + name);
        }
    }

    public void StopSound(string name, GameObject sourceObject = null)
    {
        if (soundSources.ContainsKey(name))
        {
            AudioSource _source;
            if (sourceObject != null)
            {
                if (!sourceObject.TryGetComponent<AudioSource>(out _source))
                {
                    //Debug.LogWarning("AudioManager: Object " + sourceObject + " does not contain an AudioSource");
                    return;
                }
            }
            else
            {
                _source = soundSources[name];
            }
            
            if (_source.isPlaying)
            {
                _source.Stop();
            }
        }
        else
        {
            Debug.LogWarning("AudioManager: Sound not found - " + name);
        }
    }
    #endregion

    #region Music
    public void PlayMusic(string name)
    {
        if (musicClips.ContainsKey(name)) 
        {
            if (musicSource.clip == null)
            {
                musicSource.clip = musicClips[name];
                StartCoroutine(FadeAudio(musicSource, MusicVolume, fadeDuration));
                StartCoroutine(PlayNextSong(name, fadeDuration));
            }
            else if (musicSource.clip != musicClips[name])
            {
                StartCoroutine(FadeOutMusic(musicSource, fadeDuration, () =>
                {
                    musicSource.clip = musicClips[name];
                    StartCoroutine(PlayNextSong(name, fadeDuration));
                    StartCoroutine(FadeAudio(musicSource, MusicVolume, fadeDuration));
                }));
            }
        }
        else
        {
            Debug.LogWarning("AudioManager: Music not found - " + name);
        }
    }

    private IEnumerator FadeOutMusic(AudioSource source, float fade, System.Action callback = null)
    {
        float _startVolume = source.volume;

        while (source.volume > 0)
        {
            source.volume -= _startVolume * Time.deltaTime / fade;
            yield return null;
        }

        source.Pause();
        source.volume = _startVolume;

        if (callback != null) callback();
    }

    IEnumerator FadeAudio(AudioSource source, float targetVolume, float fade)
    {
        // Fade in new music
        source.volume = 0f;
        source.Play();
        while (Mathf.Abs(source.volume - targetVolume) > 0.01f)
        {
            source.volume += Time.deltaTime / fade;
            yield return null;
        }

        source.volume = targetVolume;
    }

    IEnumerator PlayNextSong(string currentSong, float fade)
    {
        float timeForNextSong = musicClips[currentSong].length - fade * 2f;

        yield return new WaitForSeconds(timeForNextSong);

        string _newSong = "";
        do
        {
            _newSong = Music[Random.Range(1, Music.Count)].Name;
        }
        while (_newSong == currentSong);

        PlayMusic(_newSong);
    }

    void StopMusic()
    {
        StartCoroutine(FadeOutMusic(musicSource, 1, () =>
        {
            StopCoroutine(nameof(PlayNextSong));
        }));
    }
    #endregion

    #region EasterEggs
    public void PlayEasterEgg(string name)
    {
        StopMusic();

        if (easterEggClips.ContainsKey(name))
        {
            easterEggSource.clip = easterEggClips[name];
            StartCoroutine(FadeAudio(easterEggSource, MusicVolume, 1));
        }
        else
        {
            Debug.LogWarning("AudioManager: EasterEgg not found - " + name);
        }
    }

    public void StopEasterEgg()
    {
        StartCoroutine(FadeOutMusic(easterEggSource, 1, () =>
        {
            musicSource.UnPause();
        }));
    }
    #endregion

    public void SetMusicVolume(float volume)
    {
        MusicVolume = volume;

        if (musicSource != null)
        {
            musicSource.volume = volume;
        }

        if(easterEggSource != null)
        {
            easterEggSource.volume = volume;
        }

        // Save the music volume to PlayerPrefs
        PlayerPrefs.SetFloat("MusicVolume", volume);
        PlayerPrefs.Save();
    }

    public void SetSFXVolume(float volume)
    {
        SfxVolume = volume;

        // Update the volume of all existing sound sources
        foreach (AudioSource source in soundSources.Values)
        {
            source.volume = volume;
        }

        // Save the SFX volume to PlayerPrefs
        PlayerPrefs.SetFloat("SFXVolume", volume);
        PlayerPrefs.Save();
    }

    public void ToggleMute()
    {
        isMuted = !isMuted;

        if (isMuted)
        {
            musicSource.Pause();
            easterEggSource.Pause();

            foreach (KeyValuePair<string, AudioSource> sound in soundSources)
            {
                sound.Value.Pause();
            }
        }
        else
        {
            musicSource.UnPause();
            easterEggSource.UnPause();

            foreach (KeyValuePair<string, AudioSource> sound in soundSources)
            {
                if (sound.Value.isPlaying)
                {
                    sound.Value.UnPause();
                }
            }
        }
    }
}

