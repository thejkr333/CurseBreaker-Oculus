using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Character : MonoBehaviour
{
    public Event_character values;
    public TMP_Text speech_boble;
    //public GameObject character_model;
    //public Animator animator;
    //public Transform spwan;

    public state s = state.started;
//--------------------------------------------
    private void Awake()
    {
        //  character_model = values.model;
      
    }

    void Start()
    {
        Debug.Log("the dictionary size is: " + values.dic.Count);
        // animator.Play("walk in");
        values.dic.Add(state.started, values.conversation.start);
        values.dic.Add(state.failed, values.conversation.failed);
        values.dic.Add(state.sucess, values.conversation.sucess);

        Debug.Log(values.display_text(state.started));
        Debug.Log(values.display_text(state.failed));
        Debug.Log(values.display_text(state.sucess));
        Debug.Log("the dictionary size is: " + values.dic.Count);
    }

    void Update()
    {
        speech_boble.text = values.display_text (s);
    }
//--------------------------------------------
    public void FAILED()
    {
        s = state.failed;
       // walking_out();
    }
    public void SUCESS()
    {
        s = state.sucess;
       // walking_out();
    }
//--------------------------------------------
    IEnumerator walking_out()
    {
        yield return new WaitForSeconds(4);

        //animator.Play("run out");
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