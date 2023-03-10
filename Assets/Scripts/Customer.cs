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
    public Sprite fire, water, dark, light, earth, air;

    public Dictionary<LimbsList, Elements> elementToLimbMapping = new();
    public Dictionary<Elements, LimbsList> limbToElementMapping = new();

    private void Awake()
    {
        //int curseNumber = UnityEngine.Random.Range(0, Enum.GetValues(typeof(Curses)).Length);
        int curseNumber = UnityEngine.Random.Range(2, Enum.GetValues(typeof(Curses)).Length);
        gameObject.AddComponent(Type.GetType(((Curses)curseNumber).ToString()));

        //Create mapping element->limb and limb->element
        for (int i = 0; i < internMapping.Length; i++)
        {
            bool same = true;
            while (same)
            {
                internMapping[i] = UnityEngine.Random.Range(0, internMapping.Length);

                if (i == 0)
                {
                    same = false;
                    continue;
                }

                for (int j = 0; j < i; j++)
                {
                    if (internMapping[i] == internMapping[j])
                    {
                        same = true;
                        break;
                    }
                    else same = false;
                }
            }
            elementToLimbMapping.Add((LimbsList)internMapping[i], (Elements)i);
            limbToElementMapping.Add((Elements)i, (LimbsList)internMapping[i]);
        }

        curse = GetComponent<Curse>();
        //curse.numberOfPartsAffected = UnityEngine.Random.Range(1, Enum.GetValues(typeof(LimbsList)).Length + 1);        
        curse.numberOfPartsAffected = UnityEngine.Random.Range(1, 3);
        curse.InitiateAffectedLimbParts();
        
        customerInfo.text = curse.curse.ToString();
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
