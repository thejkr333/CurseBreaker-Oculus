using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Curse_editor : EditorWindow
{
    private int Steps = 5;
    private List<step> s = new List<step>();
    private process p;

    private string a;

    [MenuItem("Window/curse")]
    static void Init()
    {
        Curse_editor window = (Curse_editor)EditorWindow.GetWindow(typeof(Curse_editor));
        window.Show();
    }


    private void OnGUI()
    {
        GUILayout.Label("CURSE");


        for (int i = 0; i < Steps; i++)
        {
            step(i);
        }
    }

    private void step(int i)
    {
        int ii = i + 1;
        int b = 0;
        GUILayout.Label("Step" + ii);
        
        GUILayout.BeginHorizontal();
        p = (process) EditorGUILayout.EnumPopup(p);  a = EditorGUILayout.TextField(a);
        GUILayout.EndHorizontal();
    }

    private void effect()
    {

    }

}



public class Potion
{
    public string tag;

}

public class Gesture
{
    public string tag;
}

enum process
{
    potion,
    gesture
}