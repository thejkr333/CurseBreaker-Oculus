using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Utils: MonoBehaviour 
{ 

    [Header("CURSE SPRITES")]
    static Dictionary<Curses, Sprite> curseImage = new();
    [SerializeField] Sprite[] curseSprites;

    [Header("ELEMENT SPRITES")]
    static Dictionary<Elements, Sprite> elementImage = new();
    [SerializeField] Sprite[] elementSprites;

    [Header("CURSE MATERIALS")]
    static Dictionary<Curses, Material> curseMaterial= new();
    [SerializeField] Material[] curseMaterials;

    [Header("POTION")]
    static Dictionary<Elements, Color> elementColor = new();
    [SerializeField] Color[] elementColors;
    [SerializeField] Color potionDoneColor;
    public static Color PotionDoneColor;

    // Start is called before the first frame update
    void Awake()
    {
        if (elementImage.Count > 0) return;

        //We are not using void
        for (int i = 0; i < Enum.GetValues(typeof(Elements)).Length - 1; i++)
        {
            elementImage.Add((Elements)i, elementSprites[i]);
            elementColor.Add((Elements)i, elementColors[i]);
        }
        for (int i = 0; i < Enum.GetValues(typeof(Curses)).Length; i++)
        {
            curseImage.Add((Curses)i, curseSprites[i]);
            curseMaterial.Add((Curses)i, curseMaterials[i]);
        }

        PotionDoneColor = potionDoneColor;
    }

    static public Sprite GetCurseSprite(Curses curse)
    {
        if (!curseImage.ContainsKey(curse))
        {
            Debug.LogError("Curse sprites not assigned");
            return null;
        }
        return curseImage[curse];
    }
    
    static public Sprite GetElementSprite(Elements element)
    {
        if (!elementImage.ContainsKey(element) || element == Elements.Void)
        {
            Debug.LogError("element sprites not assigned");
            return null;
        }
        return elementImage[element];
    }
    
    static public Material GetCurseMaterial(Curses curse)
    {
        if (!curseMaterial.ContainsKey(curse))
        {
            Debug.LogError("Curse material not assigned");
            return null;
        }
        return curseMaterial[curse];
    }

    static public Color GetElementColor(Elements element)
    {
        if (!elementColor.ContainsKey(element) || element == Elements.Void)
        {
            Debug.LogError("element color not assigned");
            return Color.black;
        }
        return elementColor[element];
    }
}