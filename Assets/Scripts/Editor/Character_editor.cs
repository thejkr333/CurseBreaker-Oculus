using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Character_editor : EditorWindow
{
    public Event_character character;
    display show = display.dialogue;

    private Vector2 scroll = new Vector2();
   
    [MenuItem("Window/Characters")]
    static void Init()
    {
        Character_editor window = (Character_editor)EditorWindow.GetWindow(typeof(Character_editor));
        window.Show();
    }

    private void OnGUI()
    {
        

        GUILayout.BeginArea(Left_side());
        Scriptable_object_section();
        dialogue();
        Question();
        GUILayout.EndArea();

        GUILayout.BeginArea(Righ_side());
        
        scroll = EditorGUILayout.BeginScrollView(scroll);
        display_content();
        EditorGUILayout.EndScrollView();    

        GUILayout.EndArea();

    }

//--------------------METHODS---------------------------
//left part
    private void Scriptable_object_section()
    {
       character  = (Event_character) EditorGUILayout.ObjectField(character, typeof(Event_character),false);
    }
    
    private void draw_box()
    {
        GUI.Box(Left_side(),"");
    }

    private void dialogue()
    {
        GUILayout.Label("Dialogue");

        if (GUILayout.Button("dialogue"))
        {
            show = display.dialogue;
        } 
    }

    private void Question()
    {
        GUILayout.Label("Question");

        if (GUILayout.Button("Questions"))
        {
            show = display.question;
        }
    }

    //right part
    private void Dialogue_display()
    {
        if (character.Conversation == null) 
            character.Conversation = new Conversation();
        
        GUILayout.Label("start");
        conversation(character.Conversation.start);

        GUILayout.Label("Failed");
        conversation(character.Conversation.failed);

        GUILayout.Label("Sucess");
        conversation(character.Conversation.sucess);


        void conversation(List<string> s)
        {
            for (int i = 0; i < s.Count; i++)
            {
                string words = s[i];
                words = EditorGUILayout.TextField(words);
                character.set_conversation(s,i,words);
            }
            if (GUILayout.Button("+"))
            {
                character.add_conversation(s);
            }
        }
    }

    private void Question_dispaly()
    {
        int questions = character.Questions.Count;


        for (int i = 0; i < questions; i++)
        {
            character.Questions[i].question_text = GUILayout.TextArea(character.Questions[i].question_text);
            character.Questions[i].answer = GUILayout.TextArea(character.Questions[i].answer,GUILayout.Height(40));
            GUILayout.Space(4f);
        }

      

        if (GUILayout.Button("+"))
        {
            character.Questions.Add(new Question());
        }


    }
//--------------------------------------------------------
    public void display_content()
    {
        switch (show)
        {
            case display.question:  Question_dispaly();     break;
            case display.dialogue:  Dialogue_display();     break;
        }

    }


//-----------------RECT SET UP----------------------------
//
    private Rect Left_side()
    {
        return new Rect(0, 0, Screen.width / 1.8f, Screen.height);
    }
    private Rect Righ_side()
    {
        return new Rect(Screen.width / 1.8f, 0, Screen.width / 1.8f, Screen.height);
    }
  
}

enum display
{
    question,
    dialogue
}

/* 
 * some of the methods have parts that repeat themselves, which is something I should avoid
 * perhaps have it get the list. I think i could get the list of strings and use that instead. 
 */


//-----------DISCARDED METHODS
/* void answer(int q)
       {
           int answer = character.questions[q].answer[q].Length;

           for (int i = 0; i < answer; i++)
           {

           }
       }
      */