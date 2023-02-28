using System.Collections.Generic;
using System;
using UnityEngine;

public enum LimbsList { Torso, Head, LeftArm, RightArm, LeftLeg, RightLeg , Soul}
public enum Curses { Wolfus, Gassle, Demonitis, Petrification }
public class Curse : MonoBehaviour
{
    public class AffectedLimb
    {
        public GameObject affectedLimbGO;
        public LimbsList limbName;
        public int strength;
        public bool cured;

        public AffectedLimb(GameObject _affectedLimbGO, LimbsList _limbName)
        {
            affectedLimbGO = _affectedLimbGO;
            limbName = _limbName;
            strength = UnityEngine.Random.Range(1, 5);
            cured = false;
        }
    }

    [Range(0, 6)]
    public int numberOfPartsAffected;

    public Curses curse;

    protected List<AffectedLimb> affectedLimbs = new();
    protected int totalStrength;
    protected bool cured;

    Customer customer;

    protected virtual void Awake()
    {
        List<int> _affectedLimbs = new();
        for (int i = 0; i < numberOfPartsAffected; i++)
        {
            //Avoid duplicates
            bool same = true;
            while (same)
            {
                int limb = UnityEngine.Random.Range(0, Enum.GetValues(typeof(LimbsList)).Length);
                if (_affectedLimbs.Contains(limb))
                {
                    same = true;
                }
                else
                {
                    _affectedLimbs.Add(limb);
                    same = false;
                }
            }

            for (int j = 0; j < transform.childCount; j++)
            {
                //if child == limb get GO of child
                if (transform.GetChild(j).name == ((LimbsList)_affectedLimbs[i]).ToString())
                {
                    affectedLimbs.Add(new AffectedLimb(transform.GetChild(j).gameObject, (LimbsList)_affectedLimbs[i]));
                    break;
                }
            }
        }

        customer = GetComponent<Customer>();
    }

    // Start is called before the first frame update
    void Start()
    {
        totalStrength = 0;
        foreach (var part in affectedLimbs)
        {
            totalStrength += part.strength;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (cured) return;
        foreach (var limb in affectedLimbs)
        {
            if (!limb.cured) return;
        }
        Cured();
    }

    void GetPotion(Potion potion)
    {
        if (potion.potionType != curse)
        {
            //Wrong potion
        }
        else
        {
            foreach (var ingElement in potion.ingredientsElements)
            {
                for (int i = 0; i < customer.elementLimbMap.Length; i++)
                {
                    //elements are equal
                    if ((int)ingElement == i)
                    {
                        foreach (var limb in affectedLimbs)
                        {
                            if (limb.limbName == (LimbsList)customer.elementLimbMap[i])
                            {
                                if (limb.strength <= potion.strength) limb.cured = true;
                                break;
                            }
                        }
                    }
                }
            }
        }
    }

    void Cured()
    {
        cured = true;
        GameManager.Instance.GoldGain();
    }

    private void OnTriggerEnter(Collider other)
    {
        Potion potion = other.GetComponent<Potion>();
        if (potion == null) return;

        GetPotion(potion);
    }
}
