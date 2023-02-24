using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[CreateAssetMenu(fileName = "person", menuName = "Cursebreaker/Character")]
public class event_character : ScriptableObject
{

    public string name;
    public GameObject model;
    public Conversation conversation = new Conversation();
    public List<Question> questions = new List <Question>();

    public Dictionary<state, List<string>> dic = new Dictionary<state, List<string>>();


    private void Awake()
    {
        dic.Add(state.started, conversation.start);
        dic.Add(state.failed, conversation.failed);
        dic.Add(state.sucess, conversation.sucess);

        Debug.Log(display_text(state.started)[0]);
        Debug.Log(display_text(state.failed)[0]);
        Debug.Log(display_text(state.sucess)[0]);
    }

    #region Functions

    public string       get_name()
    {
        return name;
    }
//-------------------------------------------------
    public GameObject   get_model()
    {
        return model;
    }
    public void set_model(GameObject g)
    {
        model = g;
    }
//-------------------------------------------------
    public void add_conversation() => conversation.start.Add ("");
    
    public void add_failed() => conversation.failed.Add ("");
    
    public void add_sucess() => conversation.sucess.Add ("");
    
    public void add_conversation (List <string> s) => s.Add ("");
    
    
    public void remove_conversation(List<string> s) => s.RemoveAt(conversation.start.Count - 1);
    

    

//-------------------------------------------------
    public List<string> get_start_conversation()
    {
        return conversation.start;
    }
    public List<string> get_sucess_conversation()
    {
        return conversation.sucess;
    }
    public List<string> get_failed_conversation()
    {
        return conversation.failed;
    }

    
//--------------------------------------------------
    public void         set_start_conversation(string response, int i)
    {
        conversation.start[i] = response;
    }
    public void         set_sucess_conversation(string response, int i)
    {
        conversation.sucess[i] = response;
    }
    public void         set_failed_conversation(string response, int i)
    {
        conversation.failed[i] = response;
    }

    public void set_conversation(List<string> s, int i, string response)
    {
        s[i] = response;
    }
//-----------------------------------------------------
//-----------------------------------------------------

    public void Start_conversation()
    {
        
    }
    public void Suceed_conversation()
    {

    }
    public void Failed_conversation()
    {

    }

//------------------------------------------------------

    

    public string display_text (state State)
    {
        return dic [State] [0];     
    }

//------------------------------------------------------
    #endregion
}

#region classes
[System.Serializable]
public class Conversation
{
    public List<string> start = new List<string>();
    public List<string> sucess = new List<string>();
    public List<string> failed = new List<string>();
}
[System.Serializable]
public class Question
{
    public string question_text;
    public string answer;
}

public class Answer
{
    public List<string> text = new List<string>();
}
#endregion

#region enums
public enum state
{
    started,
    failed,
    sucess
}
#endregion
