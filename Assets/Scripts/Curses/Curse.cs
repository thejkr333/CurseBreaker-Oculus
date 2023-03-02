using System.Collections.Generic;
using System;
using UnityEngine;
using static OVRPlugin;

public enum LimbsList { Head, LeftArm, RightArm, LeftLeg, RightLeg , Soul, Torso }
public enum Curses { Wolfus, Gassle, Demonitis, Petrification }
public class Curse : MonoBehaviour
{
    [System.Serializable]
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

    public List<AffectedLimb> affectedLimbs = new();
    public int totalStrength;
    protected bool cured;
    public int chances = 3;
    Customer customer;
    public Potion _potion;

    public void InitiateAffectedLimbParts()
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

            //Populate the affectedLimb list with the correspondant affected limbs
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

        //Calculate total strength of the curse
        foreach (var part in affectedLimbs)
        {
            totalStrength += part.strength;
        }
        customer = GetComponent<Customer>();

        SetUpCurse();
    }
    

    protected virtual void SetUpCurse()
    {

    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    protected virtual void Update()
    {
        //Just for debugging and testing without VR
        if (Input.GetKeyDown(KeyCode.P))
        {
            GetPotion();
        }

        //Check if player failed to cure the customer - James
        if(chances == 0)
        {
           
            gameObject.GetComponent<basicChat>().chatTime = 3;
            gameObject.GetComponent<basicChat>().DespawnOnceDone = true;
        }

        //Check if all affected limbs are cured
        if (cured) return;
        foreach (var limb in affectedLimbs)
        {
            if (!limb.cured) return;
        }
        Cured();
    }

    void GetPotion(Potion potion = null)
    {
        if (potion == null) potion = _potion;

        //Check if the potion type has the correct one
        if (!potion.potionTypes.Contains(curse))
        {
            //Wrong potion
            Debug.Log("Wrong potion");

            //Reducing your chances to cure the customer - James
            chances--;
            gameObject.GetComponent<basicChat>().wrongPotion = true;
            gameObject.GetComponent<basicChat>().chatTime = 3;
        }
        else
        {
            //Go through the potion ingredients getting their elements
            foreach (var ingElement in potion.ingredientsElements)
            {
                //Go throught the affectedLimbs
                foreach (var limb in affectedLimbs)
                {
                    if (ingElement == Elements.None)
                    {
                        if(limb.limbName == LimbsList.Torso)
                        {
                            if (limb.strength <= potion.strength) limb.cured = true;
                            break;
                        }
                    }
                    else
                    {
                        Debug.Log("Limb name: " + limb.limbName + " vs mapping: " + (LimbsList)customer.elementLimbMap[(int)ingElement]);
                        //Check if the limb corresponds with the element in the mapping
                        if (limb.limbName == (LimbsList)customer.elementLimbMap[(int)ingElement])
                        {
                            if (limb.strength <= potion.strength) limb.cured = true;
                            break;
                        }
                    }
                }
            }
        }
    }

    void Cured()
    {
       
        gameObject.GetComponent<basicChat>().chatTime = 3;
        gameObject.GetComponent<basicChat>().DespawnOnceDone = true;
        gameObject.GetComponent<basicChat>().Cured = true;
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
