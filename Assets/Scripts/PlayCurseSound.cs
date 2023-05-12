using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayCurseSound : MonoBehaviour
{
    string soundName;
    [SerializeField] Vector2 MinMaxRepeat;

    public void PlaySoundS(string _soundName)
    {
        soundName = _soundName;
        PlaySound();

        Invoke(nameof(PlaySound), Random.Range(MinMaxRepeat.x, MinMaxRepeat.y));
    }

    void PlaySound()
    {
        AudioManager.Instance.PlaySoundDynamic(soundName, transform.gameObject, Random.Range(1f, 3f));
    }
    
}
