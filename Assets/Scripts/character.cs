using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class character : MonoBehaviour
{
    public event_character values;
    public TMP_Text speech_boble;
    public GameObject character_model;
    public Animator animator;

    public state s = state.started;
//--------------------------------------------
    private void Awake()
    {
        character_model = values.model;
    }

    void Start()
    {
        animator.Play("walk in");
    }

    void Update()
    {
        speech_boble.text = values.display_text(s);
    }
//--------------------------------------------
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
//--------------------------------------------
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
 */