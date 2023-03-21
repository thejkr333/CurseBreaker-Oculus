using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

[CreateAssetMenu(fileName = "Cursebreaker", menuName = "matrix")]
public class CursexIngredientMatrix : ScriptableObject
{
    public Data[,] matrixData = new Data[Enum.GetValues(typeof(Curses)).Length, Enum.GetValues(typeof(Ingredients)).Length];
}

[System.Serializable]
public class Data
{
    public Curses curse = new Curses();
    public Ingredient ingredients = new Ingredient();
    public int value = 0;
}
