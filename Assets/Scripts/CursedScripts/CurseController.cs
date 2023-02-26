using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurseController : MonoBehaviour
{
    public GameManager GM;

    public int  CurseCount, CursedLimbCount, PrevLimbInt, SecondLimbCheck, ElementalRando, LimbTarget, ElementalCount;
    public int[] CursedLimbs, WhatCurse;
    public bool FinalCurseCheck, CheckFinished;

    public bool isCured = false;

    public GameObject[] Limbs;

    public Dictionary<int, Color> element_color = new Dictionary<int, Color>();
   

    // Start is called before the first frame update
    void Start()
    {
        GM = GameObject.Find("GameManager").GetComponent<GameManager>();

        element_color.Add(0, Color.black);
        element_color.Add(1, Color.white);
        element_color.Add(2, Color.blue);
        element_color.Add(3, Color.red);
        element_color.Add(4, Color.yellow);
        element_color.Add(5, Color.gray);
        element_color.Add(6, Color.cyan);


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
                color_limb(ElementalRando);
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
                color_limb(ElementalRando);
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
                color_limb(ElementalRando);
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
                color_limb(ElementalRando);
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
                color_limb(ElementalRando);
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
                color_limb(ElementalRando);
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
                color_limb(ElementalRando);
                ElementalRando--;
                return;

            }

        


    }
    


    
    public void cure()
    {
        CurseCount--;

        if (CurseCount <= 0)
            isCured = true;

        if (isCured)
            GM.GoldGain();
    }

    public void color_limb( int element)
    {
        Limbs[LimbTarget - 1].GetComponentInChildren<Renderer>().material.color = element_color[element];
    }
    public void Cure_limb()
    {
        revert_limb_color();
        cure();

        //set the limb color back to white once cured
        void revert_limb_color() => Limbs[LimbTarget - 1].GetComponentInChildren<Renderer>().material.color = Color.white;
    }


}
