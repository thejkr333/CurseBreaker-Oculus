using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurseController : MonoBehaviour
{
    public int  CurseCount, CursedLimbCount, PrevLimbInt, SecondLimbCheck, ElementalRando, LimbTarget, ElementalCount;
    public int[] CursedLimbs, WhatCurse;
    public bool FinalCurseCheck, CheckFinished;
    public GameObject[] Limbs;
   

    // Start is called before the first frame update
    void Start()
    {
        CurseCount = Random.Range(1, 5);
        ElementalRando = Random.Range(1, 6);
        ElementalCount = CurseCount;
    }

    // Update is called once per frame
    void Update()
    {
        if (FinalCurseCheck == true&& ElementalCount>0)
        {
            LimbTarget = CursedLimbs[ElementalCount-1];
        
            ElementalLimb();
            ElementalCount--;
            
            
        }
       
        if(CursedLimbCount == CurseCount)
        {
            PrevLimbInt = CursedLimbs[CursedLimbCount-1];
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
       

    }


  public  void ElementalLimb()
    {

        if(LimbTarget-1<0) LimbTarget = 1;

        
        switch (ElementalRando)
            {
                case 0:
                    Limbs[LimbTarget-1].GetComponent<LimbElement>().Void = true;
                    Limbs[LimbTarget-1].GetComponent<LimbElement>().Fire = false;
                    Limbs[LimbTarget-1].GetComponent<LimbElement>().Light = false;
                    Limbs[LimbTarget-1].GetComponent<LimbElement>().Dark = false;
                    Limbs[LimbTarget-1].GetComponent<LimbElement>().Wind = false;
                    Limbs[LimbTarget-1].GetComponent<LimbElement>().Water = false;
                    Limbs[LimbTarget-1].GetComponent<LimbElement>().Ice = false;
                ElementalRando--;
                return;

                case 1:
                    Limbs[LimbTarget-1].GetComponent<LimbElement>().Void = false;
                    Limbs[LimbTarget-1].GetComponent<LimbElement>().Fire = true;
                    Limbs[LimbTarget-1].GetComponent<LimbElement>().Light = false;
                    Limbs[LimbTarget-1].GetComponent<LimbElement>().Dark = false;
                    Limbs[LimbTarget-1].GetComponent<LimbElement>().Wind = false;
                    Limbs[LimbTarget-1].GetComponent<LimbElement>().Water = false;
                    Limbs[LimbTarget-1].GetComponent<LimbElement>().Ice = false;
                ElementalRando--;
                return;

                case 2:
                    Limbs[LimbTarget-1].GetComponent<LimbElement>().Void = false;
                    Limbs[LimbTarget-1].GetComponent<LimbElement>().Fire = false;
                    Limbs[LimbTarget-1].GetComponent<LimbElement>().Light = true;
                    Limbs[LimbTarget-1].GetComponent<LimbElement>().Dark = false;
                    Limbs[LimbTarget-1].GetComponent<LimbElement>().Wind = false;
                    Limbs[LimbTarget-1].GetComponent<LimbElement>().Water = false;
                    Limbs[LimbTarget-1].GetComponent<LimbElement>().Ice = false;
                ElementalRando--;
                return;

                case 3:
                    Limbs[LimbTarget-1].GetComponent<LimbElement>().Void = false;
                    Limbs[LimbTarget-1].GetComponent<LimbElement>().Fire = false;
                    Limbs[LimbTarget-1].GetComponent<LimbElement>().Light = false;
                    Limbs[LimbTarget-1].GetComponent<LimbElement>().Dark = true;
                    Limbs[LimbTarget-1].GetComponent<LimbElement>().Wind = false;
                    Limbs[LimbTarget-1].GetComponent<LimbElement>().Water = false;
                    Limbs[LimbTarget-1].GetComponent<LimbElement>().Ice = false;
                ElementalRando--;
                return;

                case 4:
                    Limbs[LimbTarget-1].GetComponent<LimbElement>().Void = false;
                    Limbs[LimbTarget-1].GetComponent<LimbElement>().Fire = false;
                    Limbs[LimbTarget-1].GetComponent<LimbElement>().Light = false;
                    Limbs[LimbTarget-1].GetComponent<LimbElement>().Dark = false;
                    Limbs[LimbTarget-1].GetComponent<LimbElement>().Wind = true;
                    Limbs[LimbTarget-1].GetComponent<LimbElement>().Water = false;
                    Limbs[LimbTarget-1].GetComponent<LimbElement>().Ice = false;
                ElementalRando--;
                return;

                case 5:
                    Limbs[LimbTarget-1].GetComponent<LimbElement>().Void = false;
                    Limbs[LimbTarget-1].GetComponent<LimbElement>().Fire = false;
                    Limbs[LimbTarget-1].GetComponent<LimbElement>().Light = false;
                    Limbs[LimbTarget-1].GetComponent<LimbElement>().Dark = false;
                    Limbs[LimbTarget-1].GetComponent<LimbElement>().Wind = false;
                    Limbs[LimbTarget-1].GetComponent<LimbElement>().Water = true;
                    Limbs[LimbTarget-1].GetComponent<LimbElement>().Ice = false;
                ElementalRando--;
                return;

                case 6:
                    Limbs[LimbTarget-1].GetComponent<LimbElement>().Void = false;
                    Limbs[LimbTarget-1].GetComponent<LimbElement>().Fire = false;
                    Limbs[LimbTarget-1].GetComponent<LimbElement>().Light = false;
                    Limbs[LimbTarget-1].GetComponent<LimbElement>().Dark = false;
                    Limbs[LimbTarget-1].GetComponent<LimbElement>().Wind = false;
                    Limbs[LimbTarget-1].GetComponent<LimbElement>().Water = false;
                    Limbs[LimbTarget-1].GetComponent<LimbElement>().Ice = true;
                ElementalRando--;
                return;

            }

        


    }
}
