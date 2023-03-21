using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEditor;


public class CurseIngredient_Editor : EditorWindow
{
    public CursexIngredientMatrix Matrix = null;
    public int rows, colloms;


    private void OnEnable()
    {
        
        rows = Enum.GetNames(typeof(Curses)).Length;
        colloms = Enum.GetNames(typeof(Ingredients)).Length;
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

        try { Debug.Log(Matrix.matrixData[0,0].value); }

        catch { Debug.Log("could not get the matrix data"); }

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
        int i = 0;
        GUILayout.BeginHorizontal();
        Curses_display();

        foreach (Ingredients ingredient in Enum.GetValues(typeof(Ingredients)))
        {

            GUILayout.BeginVertical();
            GUILayout.Label(ingredient.ToString());
            foreach(Curses c in Enum.GetValues(typeof(Curses)))
                Cell((int)c, (int)ingredient);
            GUILayout.EndVertical();
        }

        GUILayout.EndHorizontal();
    }

    private void Curses_display()
    {
        GUILayout.BeginVertical();

        GUILayout.Space(20f);

        foreach (Curses c in Enum.GetValues(typeof(Curses)))
            GUILayout.Label(c.ToString());

        GUILayout.EndVertical();
    }

    public void Cell(int X, int Y)
    {
        //Matrix.matrixData[X, Y].value = int.Parse(GUILayout.TextField(Matrix.matrixData[X, Y].value.ToString()));
        GUILayout.Label("0");
        Debug.Log("did it");
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