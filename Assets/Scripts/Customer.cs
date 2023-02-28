using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class Customer : MonoBehaviour
{
    //length -1 not to take NONE into account
    public int[] elementLimbMap = new int[Enum.GetValues(typeof(Elements)).Length-1];
    Curse curse;

    private void Awake()
    {
        int curseNumber = UnityEngine.Random.Range(0, Enum.GetValues(typeof(Curses)).Length);
        gameObject.AddComponent(Type.GetType(((Curses)curseNumber).ToString()));

        curse = GetComponent<Curse>();
        curse.numberOfPartsAffected = UnityEngine.Random.Range(1, Enum.GetValues(typeof(LimbsList)).Length);
        curse.InitiateAffectedLimbParts();

        for (int i = 0; i < elementLimbMap.Length; i++)
        {
            bool same = true;
            while (same)
            {
                elementLimbMap[i] = UnityEngine.Random.Range(0, elementLimbMap.Length);

                if (i == 0) same = false;
                for (int j = 0; j < i; j++)
                {
                    if (elementLimbMap[i] == elementLimbMap[j])
                    {
                        same = true;
                        break;
                    }
                    else same = false;
                }
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
