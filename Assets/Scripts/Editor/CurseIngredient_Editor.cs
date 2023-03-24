using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEditor;


public class CurseIngredient_Editor : EditorWindow
{
    public CursexIngredientMatrix Matrix = null;
    public int Rows, Columns;

    int[,] check_values = new int[Enum.GetNames(typeof(Curses)).Length, 4];
    private void OnEnable()
    {      
        Rows = Enum.GetNames(typeof(Curses)).Length;
        Columns = Enum.GetNames(typeof(Ingredients)).Length;
    }


    [MenuItem("Cursebreaker/matrix")]
    static void Init()
    {
        CurseIngredient_Editor window = (CurseIngredient_Editor)EditorWindow.GetWindow(typeof(CurseIngredient_Editor));
        window.Show();
    }

    private void OnGUI()
    {
        Matrix = (CursexIngredientMatrix)EditorGUILayout.ObjectField(Matrix, typeof(CursexIngredientMatrix), true);
        if (Matrix == null) return;

        Ingredients();
        //Curses_display();
        //Grid();
    }

    private void CurseName(int i)
    {
        GUILayout.Label(Curses.Gassle.ToString());
    }

    private void Ingredients()
    {
        check_values = new int[Enum.GetNames(typeof(Curses)).Length, 4];
        EditorGUILayout.BeginHorizontal(GUILayout.ExpandWidth(true));
        CursesDisplay();

        
        foreach (Ingredients ingredient in Enum.GetValues(typeof(Ingredients)))
        {
            EditorGUILayout.BeginVertical();
            GUILayout.Label(ingredient.ToString());
            foreach(Curses curse in Enum.GetValues(typeof(Curses)))
                Cell(curse, ingredient);
            EditorGUILayout.EndVertical();
        }

        EditorGUILayout.EndHorizontal();

        Check();
    }

    private void Check()
    {
        for (int rows = 0; rows < check_values.GetLength(0); rows++)
        {
            for (int i = 1; i < check_values.GetLength(1); i++)
            {
                if(check_values[rows, i] > 1)
                {
                    string _warning = "Multiple values of " + i + " repeated in curse: " + ((Curses)rows).ToString();
                    EditorGUILayout.HelpBox(_warning, MessageType.Warning);
                }
            }
        }
    }

    private void CursesDisplay()
    {
        EditorGUILayout.BeginVertical();

        GUILayout.Space(20f);

        foreach (Curses c in Enum.GetValues(typeof(Curses)))
            GUILayout.Label(c.ToString());

        EditorGUILayout.EndVertical();
    }

    public void Cell(Curses curse, Ingredients ingredient)
    {
        EditorGUI.BeginChangeCheck();
        int _new_value = EditorGUILayout.IntField(Matrix.GetValue(curse, ingredient));
        if(EditorGUI.EndChangeCheck())
        {
            Matrix.SetValue(curse, ingredient, _new_value);
        }

        check_values[(int)curse, Matrix.GetValue(curse, ingredient)]++;
    }


    private void Save()
    {
        
    }

    public Rect GridSize(int rowSize, int collomSize)
    {
        //return new Rect(Screen.width / rowSize * rows, Screen.height / collomSize, Screen.width/ rowSize * rows +1, Screen.height /collomSize + 1);
        return new Rect(Screen.width / (rowSize * 2 + 1), Screen.height / (collomSize * 2 + 1), 20, 20);
    }

    public Rect blank_space()
    {
        return new Rect(Screen.width , Screen.height , 0.1f , 0.05f);
    }

    public Rect IngredientLabel()
    {
        return new Rect();
    }

    public Rect CurseLabels()
    {
        return new Rect();
    }

}

/* collom 
 * separate everything by making a 
 * 
 * 
 * 
 * 
 */