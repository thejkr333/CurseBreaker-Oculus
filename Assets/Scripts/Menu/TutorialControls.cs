using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using UnityEngine.Sprites;
using TMPro;

public class TutorialControls : MonoBehaviour
{

    //public bool IsPlaying;

    private VideoPlayer tutorialVideo;
    private AudioSource tutorialSound;


    //public Sprite PlaySprite, PauseSprite;
    public Button PlayPauseButt;//PlayButton, PauseButton;
    private TMP_Text textMeshPro;

    // Start is called before the first frame update
    void Awake()
    {
        tutorialVideo = GetComponent<VideoPlayer>();
        tutorialSound = GetComponent<AudioSource>();
        if (textMeshPro == null)
        {
            textMeshPro = PlayPauseButt.GetComponentInChildren<TMP_Text>();
        }
        //IsPlaying = false;
        //PauseTutorial();
        
        //PlayPauseButt.image.sprite = PlaySprite;
        tutorialVideo.Pause();
        tutorialSound.Pause();
    }

    // Update is called once per frame
    void Update()
    {

    }
    
    public void PlayPause()
    {
        if (tutorialVideo.isPlaying == false)
        {
            //PlayPauseButt.image.sprite = PauseSprite;
            textMeshPro.text = "Pause";
            tutorialVideo.Play();
            tutorialSound.Play();
        }
        else
        {
            //PlayPauseButt.image.sprite = PlaySprite;
            textMeshPro.text = "Play";
            tutorialVideo.Pause();
            tutorialSound.Pause();
        }
    }
    /*
    public void PlayTutorial()
    {
        tutorialVideo.Play();
        tutorialSound.Play();
        PlayButton.gameObject.SetActive(false);
        PauseButton.gameObject.SetActive(true);
    }
    public void PauseTutorial()
    {
        tutorialVideo.Pause();
        tutorialSound.Pause();
        PlayButton.gameObject.SetActive(true);
        PauseButton.gameObject.SetActive(false) ;
    }*/
}
