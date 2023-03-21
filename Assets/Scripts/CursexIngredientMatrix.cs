using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Array2DEditor;

public class CursexIngredientMatrix : MonoBehaviour 
{
    public int[,] matrixData = new int[Enum.GetValues(typeof(Curses)).Length, Enum.GetValues(typeof(Ingredients)).Length];
}

[System.Serializable]
public class Data
{
    public Curses curse;
    public Ingredients ingredients;
    public int value;

    
}
