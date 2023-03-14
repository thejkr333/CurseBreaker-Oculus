using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class character : MonoBehaviour
{
    public Event_character values;
    public TMP_Text speech_boble;
    public GameObject character_model;
    public Animator animator;
    public Transform spwan;

    public state s = state.started;
//---------------CONSTANT METHODS-----------------------------
    private void Awake()
    {
       
    }

    void Start()
    {
        animator.Play("walk in");
        character_model = values.Model;
    }

    void Update()
    {
        speech_boble.text = values.display_text(s);
    }
//---------------CHANGE MESSAGE---------------------------
    public void FAILED()
    {
        s = state.failed;
        walking_out();
    }
    public void SUCESS()
    {
        s = state.sucess;
        walking_out();
    }
//-----------------I ENUMERATOR---------------------------
    IEnumerator walking_out()
    {
        yield return new WaitForSeconds(4);

        animator.Play("run out");
    }
    

}
/*
 * prtals and alterations
 * strange storage units
 * don't use the
 * new places to go
 * 
 * make hand gestures to the selling point
 * 
 */

/*
 * I need to make the character speak when I spawn them. by spawning them I have control over when and what they say.
 * 
 * ISSUES:
 * spawning the model
 *      to spawn the character I will have to have a spot I am always spawning them on, and I have this spwan point, I could give it a tag or an script to spawn them there,
 *      parenting the character to there when I 
 * 
 * 
 * 
 */