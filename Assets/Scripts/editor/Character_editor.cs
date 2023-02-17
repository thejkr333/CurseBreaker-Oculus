using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Character_editor : EditorWindow
{
    public event_character character;
    display show;
   
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
        GUILayout.EndArea();

        GUILayout.BeginArea(Righ_side());
        display_content();
        GUILayout.EndArea();

    }

//--------------------METHODS---------------------------
//left part
    private void Scriptable_object_section()
    {
       character  = (event_character) EditorGUILayout.ObjectField(character, typeof(event_character),false);
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

        } 
    }

//right part
    private void Dialogue_display()
    {
        if (character.conversation == null) 
            character.conversation = new Conversation();
        
        GUILayout.Label("start");
        conversation(character.conversation.start);

        GUILayout.Label("Failed");
        conversation(character.conversation.failed);

        GUILayout.Label("Sucess");
        conversation(character.conversation.sucess);


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
        int questions = character.questions.Count;


        for (int i = 0; i < questions; i++)
        {
            GUILayout.Label(character.questions[i].ToString());
            answer(i);
        }

        void answer(int q)
        {
            int answer = character.questions[q].answer.Count;

            for (int i = 0; i < answer; i++)
            {
                character.questions[q].answer[i] = GUILayout.TextArea(character.questions[q].answer[i]);
            }
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
