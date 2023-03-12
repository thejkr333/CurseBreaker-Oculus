using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using TMPro;

public class Customer : MonoBehaviour
{
    //length -1 not to take NONE into account
    int[] internMapping = new int[Enum.GetValues(typeof(Elements)).Length-1];
    Curse curse;
    [SerializeField] TMP_Text customerInfo;

    [SerializeField] Material hiddenMat;
    public Sprite Fire, Water, Dark, Light, Earth, Air;

    public Dictionary<LimbsList, Elements> ElementToLimbMapping = new();
    public Dictionary<Elements, LimbsList> LimbToElementMapping = new();

    private void Awake()
    {
        //select curse randomly and add it to the customer
        //int _curseNumber = UnityEngine.Random.Range(0, Enum.GetValues(typeof(Curses)).Length);
        int _curseNumber = UnityEngine.Random.Range(2, Enum.GetValues(typeof(Curses)).Length);
        gameObject.AddComponent(Type.GetType(((Curses)_curseNumber).ToString()));

        //Create mapping element->limb and limb->element
        for (int i = 0; i < internMapping.Length; i++)
        {
            //Avoid duplicates
            bool _same = true;
            while (_same)
            {
                internMapping[i] = UnityEngine.Random.Range(0, internMapping.Length);

                if (i == 0)
                {
                    _same = false;
                    continue;
                }

                for (int j = 0; j < i; j++)
                {
                    if (internMapping[i] == internMapping[j])
                    {
                        _same = true;
                        break;
                    }
                    else _same = false;
                }
            }
            //Populate dictionary with the mapping created
            ElementToLimbMapping.Add((LimbsList)internMapping[i], (Elements)i);
            LimbToElementMapping.Add((Elements)i, (LimbsList)internMapping[i]);
        }

        //Select which parts to affect randomly
        curse = GetComponent<Curse>();
        //curse.numberOfPartsAffected = UnityEngine.Random.Range(1, Enum.GetValues(typeof(LimbsList)).Length + 1);        
        curse.NumberOfPartsAffected = UnityEngine.Random.Range(1, 3);
        curse.InitiateAffectedLimbParts();
        
        customerInfo.text = curse.CurrentCurse.ToString();
    }
}
