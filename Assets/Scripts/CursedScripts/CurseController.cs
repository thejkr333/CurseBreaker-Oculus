using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurseController : MonoBehaviour
{
    public int  CurseCount, CursedLimbCount, PrevLimbInt, SecondLimbCheck;
    public int[] CursedLimbs, WhatCurse;
    public bool FinalCurseCheck;
    public LimbElement Allignment;

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
        }
       
    }
}
