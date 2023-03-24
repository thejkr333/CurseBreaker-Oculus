using System.Collections.Generic;
using System;
using UnityEngine;
using static OVRPlugin;
using Unity.VisualScripting;

public enum LimbsList { Head, LeftArm, RightArm, LeftLeg, RightLeg, Torso }
[System.Serializable]
public enum Curses { Wolfus, Gassle, Demonitis, Petrification,  Porko, Runeblight}

[System.Serializable]
public class AffectedLimb
{
    public GameObject AffectedLimbGO;
    public Curses Curse;
    public LimbsList LimbName;
    public int Strength;
    public bool Cured;
    public Elements Element;

    public AffectedLimb(GameObject _affectedLimbGO, Curses _curse, LimbsList _limbName, Elements _element)
    {
        Element = _element;
        AffectedLimbGO = _affectedLimbGO;
        Curse = _curse;
        LimbName = _limbName;
        Strength = UnityEngine.Random.Range(1, 5);
        Cured = false;
    }
}
public class Curse : MonoBehaviour
{
    public Curses CurrentCurse;
    public int TotalStrength;

    protected Ingredients primaryIngredient, secondaryIngredient, substractiveIngredient;
    
   public virtual void ChangeVisuals(GameObject affectedLimb)
    {

    }

    // Update is called once per frame
    protected virtual void Update()
    {
        
    }
}
