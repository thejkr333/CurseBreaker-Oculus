using UnityEngine;
using System.Collections.Generic;
using System.Collections;

[System.Serializable]
public class Sound
{
    public string Name;
    public bool IsMusic;
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

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    public List<Sound> Sounds = new List<Sound>();

    private Dictionary<string, AudioSource> soundSources = new Dictionary<string, AudioSource>();
    private AudioSource musicSource;

    [Range(0f, 1f)]
    public float MusicVolume = 1f;

    [Range(0f, 1f)]
    public float SfxVolume = 1f;

    private float fadeDuration = 1f;

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
            soundSources[sound.Name] = _source;

            if (sound.IsMusic)
            {
                musicSource = _source;
                musicSource.loop = true;
            }
        }
    }

    public void PlaySoundStatic(string name, Vector3 position)
    {
        if (soundSources.ContainsKey(name))
        {
            AudioSource _source = soundSources[name];
            _source.transform.position = position;
            _source.volume = SfxVolume;
            _source.Play();
        }
        else
        {
            Debug.LogWarning("AudioManager: Sound not found - " + name);
        }
    }

    public void PlaySoundDynamic(string name, GameObject followObject = null)
    {
        if (soundSources.ContainsKey(name))
        {
            Sound _sound = Sounds.Find(s => s.Name == name);

            if (_sound == null)
            {
                Debug.LogWarning("AudioManager: Sound not found - " + name);
                return;
            }

            AudioSource source = followObject != null ? followObject.AddComponent<AudioSource>() : gameObject.AddComponent<AudioSource>();
            source.clip = _sound.Clip;
            source.volume = SfxVolume;
            source.pitch = _sound.Pitch;
            source.loop = _sound.Loop;
            source.spatialBlend = _sound.SpatialBlend;
            source.maxDistance = _sound.MaxDistance;
            source.rolloffMode = _sound.RolloffMode;
            source.Play();

            if (!_sound.Loop)
            {
                Destroy(source, _sound.Clip.length);
            }
        }
        else
        {
            Debug.LogWarning("AudioManager: Sound not found - " + name);
        }
    }

    public void StopSound(string name)
    {
        if (soundSources.ContainsKey(name))
        {
            AudioSource _source = soundSources[name];

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


    #region Music
    public void PlayMusic(string name)
    {
        if (musicSource == null)
        {
            Debug.LogWarning("AudioManager: Music source not found.");
            return;
        }

        if (soundSources.ContainsKey(name))
        {
            if (musicSource.clip == null)
            {
                musicSource.clip = soundSources[name].clip;
                musicSource.volume = 0f;
                musicSource.Play();
                StartCoroutine(FadeAudio(musicSource, MusicVolume, fadeDuration));
            }
            else if (musicSource.clip != soundSources[name].clip)
            {
                StartCoroutine(FadeOutMusic(() =>
                {
                    musicSource.clip = soundSources[name].clip;
                    musicSource.volume = 0f;
                    musicSource.Play();
                    StartCoroutine(FadeAudio(musicSource, MusicVolume, fadeDuration));
                }));
            }
        }
        else
        {
            Debug.LogWarning("AudioManager: Music not found - " + name);
        }
    }

    private IEnumerator FadeOutMusic(System.Action callback = null)
    {
        float _startVolume = musicSource.volume;

        while (musicSource.volume > 0)
        {
            musicSource.volume -= _startVolume * Time.deltaTime / fadeDuration;
            yield return null;
        }

        musicSource.Stop();
        musicSource.volume = _startVolume;

        if (callback != null) callback();
    }

    IEnumerator FadeAudio(AudioSource source, float duration, float targetVolume)
    {
        float _startVolume = source.volume;

        // Fade out current music
        while (source.volume > 0)
        {
            source.volume -= _startVolume * Time.deltaTime / duration;
            yield return null;
        }

        source.Stop();

        // Fade in new music
        source.clip = soundSources[name].clip;
        source.volume = 0f;
        source.Play();

        while (source.volume < targetVolume)
        {
            source.volume += Time.deltaTime / duration;
            yield return null;
        }

        source.volume = targetVolume;
    }
    #endregion

    public void SetMusicVolume(float volume)
    {
        MusicVolume = volume;

        if (musicSource != null)
        {
            musicSource.volume = volume;
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

            foreach (KeyValuePair<string, AudioSource> sound in soundSources)
            {
                sound.Value.Pause();
            }
        }
        else
        {
            musicSource.UnPause();

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

