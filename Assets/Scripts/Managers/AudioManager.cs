using UnityEngine;
using System.Collections.Generic;
using System.Collections;

[System.Serializable]
public class Sound
{
    public string name;
    public bool isMusic;
    public AudioClip clip;

    [Range(0f, 1f)]
    public float volume = 1f;

    [Range(-3f, 3f)]
    public float pitch = 1f;

    public bool loop = false;
}

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    public List<Sound> sounds = new List<Sound>();

    private Dictionary<string, AudioSource> soundSources = new Dictionary<string, AudioSource>();
    private AudioSource musicSource;

    [Range(0f, 1f)]
    public float musicVolume = 1f;

    [Range(0f, 1f)]
    public float sfxVolume = 1f;

    private float fadeDuration = 1f;

    private bool isMuted;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        // Load volume levels from PlayerPrefs
        musicVolume = PlayerPrefs.GetFloat("MusicVolume", 1f);
        sfxVolume = PlayerPrefs.GetFloat("SFXVolume", 1f);

        // Create AudioSources for each Sound object
        foreach (Sound sound in sounds)
        {
            AudioSource source = gameObject.AddComponent<AudioSource>();
            source.clip = sound.clip;
            source.volume = sound.volume;
            source.pitch = sound.pitch;
            source.playOnAwake = false;
            source.loop = sound.loop;
            soundSources[sound.name] = source;

            if (sound.isMusic)
            {
                musicSource = source;
                musicSource.loop = true;
            }
        }
    }

    public void PlaySound(string name, Vector3 position)
    {
        if (soundSources.ContainsKey(name))
        {
            AudioSource source = soundSources[name];
            source.transform.position = position;
            source.volume = sfxVolume;
            source.Play();
        }
        else
        {
            Debug.LogWarning("AudioManager: Sound not found - " + name);
        }
    }

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
                StartCoroutine(FadeAudio(musicSource, musicVolume, fadeDuration));
            }
            else if (musicSource.clip != soundSources[name].clip)
            {
                StartCoroutine(FadeOutMusic(() =>
                {
                    musicSource.clip = soundSources[name].clip;
                    musicSource.volume = 0f;
                    musicSource.Play();
                    StartCoroutine(FadeAudio(musicSource, musicVolume, fadeDuration));
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
        float startVolume = musicSource.volume;

        while (musicSource.volume > 0)
        {
            musicSource.volume -= startVolume * Time.deltaTime / fadeDuration;
            yield return null;
        }

        musicSource.Stop();
        musicSource.volume = startVolume;

        if (callback != null) callback();
    }

    IEnumerator FadeAudio(AudioSource source, float duration, float targetVolume)
    {
        float startVolume = source.volume;

        // Fade out current music
        while (source.volume > 0)
        {
            source.volume -= startVolume * Time.deltaTime / duration;
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

    public void SetMusicVolume(float volume)
    {
        musicVolume = volume;

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
        sfxVolume = volume;

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

