using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof(AudioSource))]
public class SoundRepeat : MonoBehaviour
{
    [SerializeField] AudioClip SoundClip;
    [SerializeField] float Repeat;
    AudioSource Source;
    // Start is called before the first frame update
    void Start()
    {
        if(SoundClip != null)
        {
            Source.clip = SoundClip;
            InvokeRepeating("PlaySound", Repeat, 1f);
        }

    }

    void PlaySound()
    {

        Source.Play();
    }
    
}
