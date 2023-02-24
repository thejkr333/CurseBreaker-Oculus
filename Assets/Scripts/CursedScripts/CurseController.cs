using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurseController : MonoBehaviour
{
    public int  CurseCount, CursedLimbCount, PrevLimbInt, SecondLimbCheck, ElementalRando;
    public int[] CursedLimbs, WhatCurse;
    public bool FinalCurseCheck, CheckFinished;
    public GameObject[] Limbs;
   

    // Start is called before the first frame update
    void Start()
    {
        CurseCount = Random.Range(1, 5);
        
    }

    // Update is called once per frame
    void Update()
    {
        if(CursedLimbCount == CurseCount)
        {
            FinalCurseCheck = true;
        }
        while(CursedLimbCount < CurseCount)
        {

            CursedLimbs[CursedLimbCount] = Random.Range(0, 6);
            
            PrevLimbInt = CursedLimbs[CursedLimbCount];
            CursedLimbCount++;

            if (CursedLimbs[CursedLimbCount] == PrevLimbInt)
            {
                CursedLimbs[CursedLimbCount] = Random.Range(0, 6);
            }
        }
        if (FinalCurseCheck)
        {
            for (var i = 0; i < CurseCount; i++)
            {
                for (var j = 0; j < CurseCount; j++)
                {
                    if (i != j)
                    {
                        if (CursedLimbs[i] == CursedLimbs[j])
                        {
                            CursedLimbs[i]++;
                            if (CursedLimbs[i] > 6) { CursedLimbs[i] = 0; }
                        }
                    }
                }
            }
            CheckFinished = true; 
           
        }
        foreach (GameObject Limb in Limbs)
        {
            for (var i = 0; i < CurseCount; i++)
            {
                for (var j = 0; j < CurseCount; j++)
                {
                    if (i != j)
                    {
                        if (Limbs[i] == Limbs[j])
                        {
                            ElementalRando++;
                            if(ElementalRando>6) ElementalRando = 0;
                        }
                    }
                }
            }
        }
    }


    void ElementalLimb()
    {
        for (var i = 0; i < CurseCount; i++)
        {
            ElementalRando = Random.Range(0, 6);
            switch (ElementalRando)
            {
                case 0:
                    Limbs[CursedLimbs[i]].GetComponent<LimbElement>().Void = true;
                    Limbs[CursedLimbs[i]].GetComponent<LimbElement>().Fire = false;
                    Limbs[CursedLimbs[i]].GetComponent<LimbElement>().Light = false;
                    Limbs[CursedLimbs[i]].GetComponent<LimbElement>().Dark = false;
                    Limbs[CursedLimbs[i]].GetComponent<LimbElement>().Wind = false;
                    Limbs[CursedLimbs[i]].GetComponent<LimbElement>().Water = false;
                    Limbs[CursedLimbs[i]].GetComponent<LimbElement>().Ice = false;
                    return;

                case 1:
                    Limbs[CursedLimbs[i]].GetComponent<LimbElement>().Void = false;
                    Limbs[CursedLimbs[i]].GetComponent<LimbElement>().Fire = true;
                    Limbs[CursedLimbs[i]].GetComponent<LimbElement>().Light = false;
                    Limbs[CursedLimbs[i]].GetComponent<LimbElement>().Dark = false;
                    Limbs[CursedLimbs[i]].GetComponent<LimbElement>().Wind = false;
                    Limbs[CursedLimbs[i]].GetComponent<LimbElement>().Water = false;
                    Limbs[CursedLimbs[i]].GetComponent<LimbElement>().Ice = false;
                    return;

                case 2:
                    Limbs[CursedLimbs[i]].GetComponent<LimbElement>().Void = false;
                    Limbs[CursedLimbs[i]].GetComponent<LimbElement>().Fire = false;
                    Limbs[CursedLimbs[i]].GetComponent<LimbElement>().Light = true;
                    Limbs[CursedLimbs[i]].GetComponent<LimbElement>().Dark = false;
                    Limbs[CursedLimbs[i]].GetComponent<LimbElement>().Wind = false;
                    Limbs[CursedLimbs[i]].GetComponent<LimbElement>().Water = false;
                    Limbs[CursedLimbs[i]].GetComponent<LimbElement>().Ice = false;
                    return;

                case 3:
                    Limbs[CursedLimbs[i]].GetComponent<LimbElement>().Void = false;
                    Limbs[CursedLimbs[i]].GetComponent<LimbElement>().Fire = false;
                    Limbs[CursedLimbs[i]].GetComponent<LimbElement>().Light = false;
                    Limbs[CursedLimbs[i]].GetComponent<LimbElement>().Dark = true;
                    Limbs[CursedLimbs[i]].GetComponent<LimbElement>().Wind = false;
                    Limbs[CursedLimbs[i]].GetComponent<LimbElement>().Water = false;
                    Limbs[CursedLimbs[i]].GetComponent<LimbElement>().Ice = false;
                    return;

                case 4:
                    Limbs[CursedLimbs[i]].GetComponent<LimbElement>().Void = false;
                    Limbs[CursedLimbs[i]].GetComponent<LimbElement>().Fire = false;
                    Limbs[CursedLimbs[i]].GetComponent<LimbElement>().Light = false;
                    Limbs[CursedLimbs[i]].GetComponent<LimbElement>().Dark = false;
                    Limbs[CursedLimbs[i]].GetComponent<LimbElement>().Wind = true;
                    Limbs[CursedLimbs[i]].GetComponent<LimbElement>().Water = false;
                    Limbs[CursedLimbs[i]].GetComponent<LimbElement>().Ice = false;
                    return;

                case 5:
                    Limbs[CursedLimbs[i]].GetComponent<LimbElement>().Void = false;
                    Limbs[CursedLimbs[i]].GetComponent<LimbElement>().Fire = false;
                    Limbs[CursedLimbs[i]].GetComponent<LimbElement>().Light = false;
                    Limbs[CursedLimbs[i]].GetComponent<LimbElement>().Dark = false;
                    Limbs[CursedLimbs[i]].GetComponent<LimbElement>().Wind = false;
                    Limbs[CursedLimbs[i]].GetComponent<LimbElement>().Water = true;
                    Limbs[CursedLimbs[i]].GetComponent<LimbElement>().Ice = false;
                    return;

                case 6:
                    Limbs[CursedLimbs[i]].GetComponent<LimbElement>().Void = false;
                    Limbs[CursedLimbs[i]].GetComponent<LimbElement>().Fire = false;
                    Limbs[CursedLimbs[i]].GetComponent<LimbElement>().Light = false;
                    Limbs[CursedLimbs[i]].GetComponent<LimbElement>().Dark = false;
                    Limbs[CursedLimbs[i]].GetComponent<LimbElement>().Wind = false;
                    Limbs[CursedLimbs[i]].GetComponent<LimbElement>().Water = false;
                    Limbs[CursedLimbs[i]].GetComponent<LimbElement>().Ice = true;
                    return;

            }

        }


    }
}
