using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof(AudioSource))]
public class WolfSound : MonoBehaviour
{
    [SerializeField] AudioClip Sound;
    AudioSource Source;
    // Start is called before the first frame update
    void Start()
    {
        if(Sound != null)
        {
            Source.clip = Sound;
            InvokeRepeating("PlaySound", 10f, 1f);
        }

    }

    void PlaySound()
    {

        Source.Play();
    }
    
}
