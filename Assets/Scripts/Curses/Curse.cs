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
    public bool Cured;
    public Elements Element;

    public AffectedLimb(GameObject _affectedLimbGO, Curses _curse, LimbsList _limbName, Elements _element)
    {
        Element = _element;
        AffectedLimbGO = _affectedLimbGO;
        Curse = _curse;
        LimbName = _limbName;
        Cured = false;
    }
}
public class Curse : MonoBehaviour
{
    public Curses CurrentCurse;
    [SerializeField] Material cursedMaterial;
    
   public virtual void ChangeVisuals(GameObject affectedLimb)
   {
        affectedLimb.GetComponentInChildren<MeshRenderer>().material = cursedMaterial;
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        
    }
}
