using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using TMPro;

public class Customer : MonoBehaviour
{
    //length -1 not to take NONE into account
    int[] internMapping = new int[Enum.GetValues(typeof(Elements)).Length - 1];

    [SerializeField] Material hiddenMat;
    public Sprite Fire, Water, Dark, Light, Earth, Air;

    public Dictionary<LimbsList, Elements> ElementToLimbMapping = new();
    public Dictionary<Elements, LimbsList> LimbToElementMapping = new();

    int numberOfPartsAffected;
    int curseStrength;

    protected bool cured;
    public int Chances = 3;

    public List<AffectedLimb> AffectedLimbs = new();

    private void Awake()
    {
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
        numberOfPartsAffected = UnityEngine.Random.Range(1, Enum.GetValues(typeof(LimbsList)).Length + 1);
        InitiateAffectedLimbParts();
    }

    private void Update()
    {
        //Check if player failed to cure the customer - James
        if (Chances == 0)
        {
            gameObject.GetComponent<BasicChat>().ChatTime = 5;
            gameObject.GetComponent<BasicChat>().DespawnOnceDone = true;
        }

        //Check if all affected limbs are cured
        if (cured) return;
        foreach (var limb in AffectedLimbs)
        {
            if (!limb.Cured) return;
        }
        Cured();
    }

    public void InitiateAffectedLimbParts()
    {
        //Using a list of ints to easily identify duplicates
        List<int> _affectedLimbs = new();
        for (int i = 0; i < numberOfPartsAffected; i++)
        {
            //Avoid duplicates
            bool _same = true;
            while (_same)
            {
                int _limb = UnityEngine.Random.Range(0, Enum.GetValues(typeof(LimbsList)).Length);
                if (_affectedLimbs.Contains(_limb))
                {
                    _same = true;
                }
                else
                {
                    _affectedLimbs.Add(_limb);
                    _same = false;
                }
            }

            //Populate the affectedLimb list with the correspondant affected limbs
            for (int j = 0; j < transform.childCount - 1; j++)
            {
                //if child == limb get GO of child
                if (transform.GetChild(j).name == ((LimbsList)_affectedLimbs[i]).ToString())
                {
                    //Create random curse
                    int _curseNumber = UnityEngine.Random.Range(0, Enum.GetValues(typeof(Curses)).Length);
                    Curses _randomCurse = (Curses)_curseNumber;
                    var curse = Type.GetType(_randomCurse.ToString());
                    
                    //Asssign the limb its correspondant values
                    AffectedLimb _affectedLimb = new AffectedLimb(transform.GetChild(j).gameObject, _randomCurse, (LimbsList)_affectedLimbs[i], ElementToLimbMapping[(LimbsList)_affectedLimbs[i]]);
                    
                    //Add the correspondant curse to the limb gamobject
                    _affectedLimb.AffectedLimbGO.AddComponent(curse);
                    Curse _curse = _affectedLimb.AffectedLimbGO.GetComponent<Curse>();
                    AffectedLimbs.Add(_affectedLimb);
                    _curse.ChangeVisuals(_affectedLimb.AffectedLimbGO);
                    break;
                }
            }
        }

        //Calculate total strength of the curse
        foreach (var part in AffectedLimbs)
        {
            curseStrength += part.Strength;
        }

        SetUpCurse();
    }

    public virtual void SetUpCurse()
    {
        foreach (var limb in AffectedLimbs)
        {
            switch (limb.Element)
            {
                case Elements.Fire:
                    limb.AffectedLimbGO.GetComponentInChildren<SpriteRenderer>().sprite = Fire;
                    break;
                case Elements.Water:
                    limb.AffectedLimbGO.GetComponentInChildren<SpriteRenderer>().sprite = Water;
                    break;
                case Elements.Dark:
                    limb.AffectedLimbGO.GetComponentInChildren<SpriteRenderer>().sprite = Dark;
                    break;
                case Elements.Light:
                    limb.AffectedLimbGO.GetComponentInChildren<SpriteRenderer>().sprite = Light;
                    break;
                case Elements.Air:
                    limb.AffectedLimbGO.GetComponentInChildren<SpriteRenderer>().sprite = Air;
                    break;
                case Elements.Earth:
                    limb.AffectedLimbGO.GetComponentInChildren<SpriteRenderer>().sprite = Earth;
                    break;
            }
        }
    }
    void Cured()
    {
        gameObject.GetComponent<BasicChat>().ChatTime = 5;
        gameObject.GetComponent<BasicChat>().DespawnOnceDone = true;
        gameObject.GetComponent<BasicChat>().Cured = true;
        cured = true;
        GameManager.Instance.GoldGain();
    }

    void CheckPotionStrength(AffectedLimb affectedLimb, Potion potion)
    {
        if (curseStrength <= potion.Strength)
        {
            affectedLimb.Cured = true;
            affectedLimb.AffectedLimbGO.GetComponentInChildren<MeshRenderer>().material.color = Color.white;
        }
    }

    void GetPotion(Potion potion)
    {
        foreach (Elements elements in potion.PotionElements)
        {
            //Check which limb is affected by the elements

            //Do de math for each limb depending on the curse that it has (matrix)
        }

        ////Check if the potion type has the correct one
        //if (!potion.PotionTypes.Contains(CurrentCurse))
        //{
        //    //Wrong potion

        //    //Reducing your chances to cure the customer - James
        //    gameObject.GetComponent<BasicChat>().WrongPotion = true;
        //    gameObject.GetComponent<BasicChat>().ChatTime = 5;
        //}
        //else
        //{
        //    //If player forgot to put elements in the potion double curse strength
        //    if (potion.PotionElements.Count == 0)
        //    {
        //        TotalStrength *= 2;
        //        return;
        //    }

        //    //Go through the potion ingredients getting their elements
        //    foreach (var ingElement in potion.PotionElements)
        //    {
        //        //Go through the affectedLimbs
        //        foreach (var limb in AffectedLimbs)
        //        {
        //            //Check if the limb corresponds with the element in the mapping
        //            if (limb.LimbName == customer.LimbToElementMapping[ingElement])
        //            {
        //                CheckPotionStrength(limb, potion);
        //                break;
        //            }                  
        //        }
        //    }
        //}
        //Chances--;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Potion _potion))
        {
            if (_potion.TryGetComponent(out OVRGrabbable _grabbable))
            {
                if (_grabbable.isGrabbed) return;
            }
            GetPotion(_potion);
            //teleport to parla, as destroying it bugs the grabber
            _potion.transform.position = new Vector3(10000, -10, 10000);
        }
    }
}
