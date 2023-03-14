using System.Collections.Generic;
using System;
using UnityEngine;
using static OVRPlugin;

public enum LimbsList { Head, LeftArm, RightArm, LeftLeg, RightLeg , Soul, Torso }
public enum Curses { Wolfus, Gassle, Demonitis, Petrification }

[System.Serializable]
public class AffectedLimb
{
    public GameObject AffectedLimbGO;
    public LimbsList LimbName;
    public int Strength;
    public bool Cured;
    public Elements Element;

    public AffectedLimb(GameObject _affectedLimbGO, LimbsList _limbName, Elements _element)
    {
        Element = _element;
        AffectedLimbGO = _affectedLimbGO;
        LimbName = _limbName;
        Strength = UnityEngine.Random.Range(1, 5);
        Cured = false;
    }
}
public class Curse : MonoBehaviour
{
    [Range(0, 6)]
    public int NumberOfPartsAffected;

    public Curses CurrentCurse;

    public List<AffectedLimb> AffectedLimbs = new();
    public int TotalStrength;
    protected bool cured;
    public int Chances = 3;
    Customer customer;

    public void InitiateAffectedLimbParts()
    {
        customer = GetComponent<Customer>();

        //Using a list of ints to easily identify duplicates
        List<int> _affectedLimbs = new();
        for (int i = 0; i < NumberOfPartsAffected; i++)
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
            for (int j = 0; j < transform.childCount-1; j++)
            {
                //if child == limb get GO of child
                if (transform.GetChild(j).name == ((LimbsList)_affectedLimbs[i]).ToString())
                {
                    if ((LimbsList)_affectedLimbs[i] == LimbsList.Torso)
                    {
                        AffectedLimbs.Add(new AffectedLimb(transform.GetChild(j).gameObject, (LimbsList)_affectedLimbs[i], Elements.None));
                        break;
                    }
                    AffectedLimbs.Add(new AffectedLimb(transform.GetChild(j).gameObject, (LimbsList)_affectedLimbs[i], customer.ElementToLimbMapping[(LimbsList)_affectedLimbs[i]]));
                    break;
                }
            }
        }

        //Calculate total strength of the curse
        foreach (var part in AffectedLimbs)
        {
            TotalStrength += part.Strength;
        }
           
        SetUpCurse();
    }
    

    protected virtual void SetUpCurse()
    {
        foreach (var limb in AffectedLimbs)
        {
            if (limb.LimbName == LimbsList.Torso) continue;
            switch (limb.Element)
            {
                case Elements.Fire:
                    limb.AffectedLimbGO.GetComponentInChildren<SpriteRenderer>().sprite = customer.Fire;
                    break;
                case Elements.Water:
                    limb.AffectedLimbGO.GetComponentInChildren<SpriteRenderer>().sprite = customer.Water;
                    break;
                case Elements.Dark:
                    limb.AffectedLimbGO.GetComponentInChildren<SpriteRenderer>().sprite = customer.Dark;
                    break;
                case Elements.Light:
                    limb.AffectedLimbGO.GetComponentInChildren<SpriteRenderer>().sprite = customer.Light;
                    break;
                case Elements.Air:
                    limb.AffectedLimbGO.GetComponentInChildren<SpriteRenderer>().sprite = customer.Air;
                    break;
                case Elements.Earth:
                    limb.AffectedLimbGO.GetComponentInChildren<SpriteRenderer>().sprite = customer.Earth;
                    break;
            }

        }
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        //Check if player failed to cure the customer - James
        if(Chances == 0)
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

    void GetPotion(Potion potion)
    {
        //Check if the potion type has the correct one
        if (!potion.potionTypes.Contains(CurrentCurse))
        {
            //Wrong potion

            //Reducing your chances to cure the customer - James
            gameObject.GetComponent<BasicChat>().WrongPotion = true;
            gameObject.GetComponent<BasicChat>().ChatTime = 5;
        }
        else
        {
            //Go through the potion ingredients getting their elements
            foreach (var ingElement in potion.ingredientsElements)
            {
                //Go through the affectedLimbs
                foreach (var limb in AffectedLimbs)
                {
                    //As torso has no element we need to check it on its own, cause there is no mapping for it
                    if (ingElement == Elements.None && limb.LimbName == LimbsList.Torso)
                    {
                        CheckPotionStrength(limb, potion);
                        break;
                    }
                    else
                    {
                        //Check if the limb corresponds with the element in the mapping
                        if (limb.LimbName == customer.LimbToElementMapping[ingElement])
                        {
                            CheckPotionStrength(limb, potion);
                            break;
                        }
                    }
                }
            }
        }
        Chances--;
    }

    void CheckPotionStrength(AffectedLimb affectedLimb, Potion potion)
    {
        if (affectedLimb.Strength <= potion.strength)
        {
            affectedLimb.Cured = true;
            affectedLimb.AffectedLimbGO.GetComponentInChildren<MeshRenderer>().material.color = Color.white;
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

    private void OnTriggerEnter(Collider other)
    {
        Potion _potion = other.GetComponent<Potion>();
        if (_potion == null) return;
        if(_potion.TryGetComponent(out OVRGrabbable _grabbable))
        {
            if (_grabbable.isGrabbed) return;
        }

        GetPotion(_potion);

        //teleport to parla, as destroying it bugs the grabber
        _potion.transform.position = new Vector3(10000, -10, 10000);
    }
}
