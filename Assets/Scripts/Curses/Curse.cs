using System.Collections.Generic;
using System;
using UnityEngine;
using static OVRPlugin;
using Unity.VisualScripting;

public enum LimbsList { Head, Torso, LeftArm, RightArm, LeftLeg, RightLeg}
[System.Serializable]
public enum Curses { Wolfus, Gassle, Demonitis, Petrification, Porko, Runeblight}

[System.Serializable]
public class Limb
{
    public GameObject AffectedLimbGO;
    public Curses Curse;
    public LimbsList LimbName;
    public bool Cured;
    public bool Cursed;
    public Elements Element;

    public Limb(GameObject _affectedLimbGO, Curses _curse, LimbsList _limbName, Elements _element)
    {
        Element = _element;
        AffectedLimbGO = _affectedLimbGO;
        Curse = _curse;
        LimbName = _limbName;
        Cured = false;
        Cursed = true;
    }

    public Limb(GameObject _affectedLimbGO, LimbsList limbName, Elements _element)
    {
        AffectedLimbGO = _affectedLimbGO;
        Curse = Curses.Wolfus;
        LimbName = limbName;
        Cured = false;
        Cursed = false;
        Element = _element;
    }
}
public class Curse : MonoBehaviour
{
    public Curses CurrentCurse;
    public Material CursedMaterial;
    
   public virtual void ChangeVisuals(GameObject affectedLimb)
   {
        affectedLimb.GetComponentInChildren<MeshRenderer>().material = CursedMaterial;
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        
    }
}
