using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Linq;

public class CurseUI
{
    public GameObject GO;
    public int Strength;
    public List<Limb> affectedLimbs = new();
}

public class Customer : MonoBehaviour
{
    [SerializeField] Material hiddenMat;
    [SerializeField] Transform hiddenChild, normalChild;

    public Dictionary<LimbsList, Elements> LimbToElementMapping = new();
    public Dictionary<Elements, LimbsList> ElementToLimbMapping = new();

    int numberOfPartsAffected;

    bool cured;
    public int Chances = 3;

    public List<Limb> AffectedLimbs = new();

    [SerializeField] GameObject curseUIprefab;

    GameObject canvas;
    public Dictionary<Curses, CurseUI> CursesStrength = new();
    public Dictionary<LimbsList, bool> affectedLimbsInfo = new();

    private void Awake()
    {
        canvas = GetComponentInChildren<Canvas>().gameObject;

        foreach (LimbsList limb in Enum.GetValues(typeof(LimbsList)))
        {
            affectedLimbsInfo.Add(limb, false);
        }

        LimbElementMapping();

        SetUpElementSprites();

        InitiateAffectedLimbParts();

        SetUpCurseUI();
    }

    private void Update()
    {
        if (Chances < 0) return;

        //Check if player failed to cure the customer - James
        if (Chances == 0)
        {
            DayManager.Instance.NextCustomer();
            Chances--;
            return;
        }

        //Check if all affected limbs are cured
        if (cured) return;
        foreach (var limb in AffectedLimbs)
        {
            if (!limb.Cured) return;
        }
        Cured();
    }

    void LimbElementMapping()
    {
        //length -1 not to take VOID into account
        int[] _internMapping = new int[Enum.GetValues(typeof(Elements)).Length - 1];

        //Create mapping element->limb and limb->element
        for (int i = 0; i < _internMapping.Length; i++)
        {
            //Avoid duplicates
            bool _same = true;
            while (_same)
            {
                _internMapping[i] = UnityEngine.Random.Range(0, _internMapping.Length);

                if (i == 0)
                {
                    _same = false;
                    continue;
                }

                for (int j = 0; j < i; j++)
                {
                    if (_internMapping[i] == _internMapping[j])
                    {
                        _same = true;
                        break;
                    }
                    else _same = false;
                }
            }
            //Populate dictionary with the mapping created
            LimbToElementMapping.Add((LimbsList)_internMapping[i], (Elements)i);
            ElementToLimbMapping.Add((Elements)i, (LimbsList)_internMapping[i]);
        }
    }

    public void InitiateAffectedLimbParts()
    {
        //Select number of parts to affect randomly depending on which day you are
        if (GameManager.Instance.DayCount <= 10) numberOfPartsAffected = UnityEngine.Random.Range(1, 3);
        else if (GameManager.Instance.DayCount <= 20) numberOfPartsAffected = UnityEngine.Random.Range(1, 5);
        else /*All parts*/ numberOfPartsAffected = UnityEngine.Random.Range(1, Enum.GetValues(typeof(LimbsList)).Length + 1);

        //Using a list of ints to easily identify duplicates
        List<LimbsList> _affectedLimbs = new();
        for (int i = 0; i < numberOfPartsAffected; i++)
        {
            //Avoid duplicates
            bool _same = true;
            while (_same)
            {
                LimbsList _limb = (LimbsList)UnityEngine.Random.Range(0, Enum.GetValues(typeof(LimbsList)).Length);
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

            affectedLimbsInfo[_affectedLimbs[i]] = true;
            GiveCurseToLimb(_affectedLimbs[i]);
        }

        InitiateLimbsNotAffected();

        SetUpCurse();
    }

    void InitiateLimbsNotAffected()
    {
        foreach (var limb in affectedLimbsInfo.Keys)
        {
            if (affectedLimbsInfo[limb]) continue;

            normalChild.gameObject.SetActive(true);
            for (int j = 0; j < normalChild.childCount; j++)
            {
                if (normalChild.GetChild(j).name == limb.ToString())
                {
                    normalChild.GetChild(j).gameObject.SetActive(true);
                    break;
                }
            }
        }
    }

    void GiveCurseToLimb(LimbsList limbName, Curses curse = (Curses)(-1))
    {
        //Create random curse from unlocked curses list
        if ((int)curse == -1) curse = CreateRandomUnlockedCurse();

        //Go through the different curses in children
        for (int i = 0; i < transform.childCount - 2; i++)
        {
            //Select the child with the curse that needs to be given
            Transform _child = transform.GetChild(i);
            if(_child.name == curse.ToString())
            {
                _child.gameObject.SetActive(true);
                for (int j = 0; j < _child.childCount; j++)
                {
                    //Select the child with the limb that needs to be affected
                    Transform _grandchild = _child.GetChild(j);
                    if(_grandchild.name == limbName.ToString())
                    {
                        _grandchild.gameObject.SetActive(true);

                        //Asssign the limb its correspondant values
                        Limb _affectedLimb = new Limb(_grandchild.gameObject, curse, limbName, LimbToElementMapping[limbName]);
                        AffectedLimbs.Add(_affectedLimb);

                        //Add the correspondant curse to the limb gamobject
                        _affectedLimb.AffectedLimbGO.AddComponent(Type.GetType(curse.ToString()));

                        //Add curse to the dictionary if it is new and give it a strength
                        CurseUI _curseUI = new();
                        if (!CursesStrength.ContainsKey(curse))
                        {
                            _curseUI.Strength = UnityEngine.Random.Range(3, 9);
                            _curseUI.GO = Instantiate(curseUIprefab, canvas.transform);
                            _curseUI.affectedLimbs.Add(_affectedLimb);
                            CursesStrength.Add(curse, _curseUI);
                        }
                        else CursesStrength[curse].Strength += UnityEngine.Random.Range(1, 4);
                        break;
                    }
                }
                //for (int k = 0; k < normalChild.childCount; k++)
                //{
                //    //Select the child with the limb that needs to be affected
                //    Transform _grandchild = normalChild.GetChild(k);
                //    if (_grandchild.name == limbName.ToString())
                //    {
                //        _grandchild.gameObject.SetActive(false);
                //        break;
                //    }
                //}
                break;
            }
        }

        return;
        //Populate the affectedLimb list with the correspondant affected limbs
        for (int j = 0; j < transform.childCount - 1; j++)
        {
            //if child == limb get GO of child
            if (transform.GetChild(j).name == limbName.ToString())
            {
                switch (limbName)
                {
                    //case LimbsList.Head:
                    //    {
                    //        Curses[] _curses = { Curses.Wolfus, Curses.Demonitis };
                    //        CreateRandomCurseBetweenRange(_curses);
                    //    }
                    //    break;

                    //case LimbsList.Torso:
                    //    {
                    //        Curses[] _curses = { Curses.Wolfus, Curses.Demonitis };
                    //        CreateRandomCurseBetweenRange(_curses);
                    //    }
                    //    break;

                    //case LimbsList.RightArm:
                    //    {
                    //        Curses[] _curses = { Curses.Wolfus, Curses.Demonitis };
                    //        CreateRandomCurseBetweenRange(_curses);
                    //    }
                    //    break;

                    //case LimbsList.LeftArm:
                    //    {
                    //        Curses[] _curses = { Curses.Wolfus, Curses.Demonitis };
                    //        CreateRandomCurseBetweenRange(_curses);
                    //    }
                    //    break;

                    //case LimbsList.RightLeg:
                    //    {
                    //        Curses[] _curses = { Curses.Wolfus, Curses.Demonitis };
                    //        CreateRandomCurseBetweenRange(_curses);
                    //    }
                    //    break;


                    //case LimbsList.LeftLeg:
                    //    {
                    //        Curses[] _curses = { Curses.Wolfus, Curses.Demonitis };
                    //        CreateRandomCurseBetweenRange(_curses);
                    //    }
                    //    break;

                    default:
                        //Create random curse from unlocked curses list
                        if ((int)curse == -1) curse = CreateRandomUnlockedCurse();
                        break;
                }
            }
        }
    }

    Curses CreateRandomUnlockedCurse()
    {
        List<Curses> unlockedCurses = GameManager.Instance.GetUnlockedCurses();
        int _curseNumber = UnityEngine.Random.Range(0, unlockedCurses.Count);
        return unlockedCurses[_curseNumber];
    }

    /// <summary>
    /// Return a random curse from the array of Curses given
    /// </summary>
    /// <param name="curses"></param>
    /// <returns></returns>
    Curses CreateRandomCurseBetweenRange(Curses[] curses)
    {
        Curses curse = curses[UnityEngine.Random.Range(0, curses.Length)];
        return curse;
    }

    void SetUpElementSprites()
    {
        //Set up all the element sprites for the mapping
        foreach (var limb in LimbToElementMapping.Keys)
        {
            for (int i = 0; i < hiddenChild.childCount; i++)
            {
                if (limb.ToString() == hiddenChild.GetChild(i).name)
                {
                    hiddenChild.GetChild(i).GetComponentInChildren<SpriteRenderer>().sprite = Utils.GetElementSprite(LimbToElementMapping[limb]);
                    hiddenChild.GetChild(i).gameObject.SetActive(false);
                    break;
                }
            }
        }
    }

    void SetUpCurse()
    {
        //Activate the elements needed for the curse
        foreach (var limb in affectedLimbsInfo.Keys)
        {      
            for (int i = 0; i < hiddenChild.childCount; i++)
            {
                if (limb.ToString() == hiddenChild.GetChild(i).name)
                {
                    hiddenChild.GetChild(i).gameObject.SetActive(affectedLimbsInfo[limb]);
                    break;
                }
            }
        }
    }
    void Cured()
    {
        cured = true;
        DayManager.Instance.CustomerCured();

        //DayManager.Instance.NextCustomer();
        //GoldManager.Instance.GainGold();
    }

    void GetPotion(Potion potion)
    {
        foreach (Elements element in potion.PotionElements)
        {
            bool _affected = false;
            //Check which limb is affected by the elements
            foreach (var limb in AffectedLimbs)
            {
                if (ElementToLimbMapping[element] == limb.LimbName)
                {
                    _affected = true;
                    if (limb.AffectedLimbGO.TryGetComponent(out Curse _curse))
                    {
                        //Do de math for each limb depending on the curse that it has (matrix)
                        int _potionStrength = CursexIngredientMatrix.CalculatePotionStrenght(_curse.CurrentCurse, potion.PotionIngredients);

                        if (CursesStrength.ContainsKey(_curse.CurrentCurse))
                        {
                            int _strengthDiff = _potionStrength - CursesStrength[_curse.CurrentCurse].Strength;
                            if (_strengthDiff > 0)
                            {
                                //potion too strong
                                CursesStrength[_curse.CurrentCurse].Strength = _strengthDiff;
                                Chances--;
                            }
                            else if (_strengthDiff < 0)
                            {
                                //potion too weak
                                CursesStrength[_curse.CurrentCurse].Strength = -_strengthDiff;
                                Chances--;
                            }
                            else
                            {
                                //perfect potion
                                limb.Cured = true;
                                affectedLimbsInfo[limb.LimbName] = false;
                                limb.AffectedLimbGO.SetActive(false);
                                SetUpCurse();
                            }
                        }
                    }

                    break;
                }
            }
            if (!_affected)
            {
                foreach (var limb in affectedLimbsInfo.Keys)
                {
                    if (affectedLimbsInfo[limb]) continue;

                    if (ElementToLimbMapping[element] == limb)
                    {
                        //Means the targeted limb doesn't have a curse on it. Affect negatively
                        //Give the limb a random curse from the main curses of the ingredients
                        Curses _curseToGive = CreateRandomUnlockedCurse();
                        affectedLimbsInfo[limb] = true;
                        //Curses _curseToGive = CursexIngredientMatrix.GetRandomCurse(potion.PotionIngredients);
                        GiveCurseToLimb(limb, _curseToGive);
                        break;
                    }
                }
            }
        }

        SetUpCurse();
        SetUpCurseUI();
        UpdateCustomerVisuals();
    }

    private void UpdateCustomerVisuals()
    {
        //This should be called each time that the customer is given a potion

        //Update limb visuals


        //Update curses strenght
        foreach (var curse in CursesStrength.Keys)
        {
            CursesStrength[curse].GO.GetComponentInChildren<TMP_Text>().text = CursesStrength[curse].Strength.ToString();
        }     
    }


    private void SetUpCurseUI()
    {
        foreach(var curse in CursesStrength.Keys.ToList())
        {
            bool _totallyCured = true;
            foreach (var limb in CursesStrength[curse].affectedLimbs)
            {
                if (!limb.Cured)
                {
                    _totallyCured = false; 
                    break;
                }
            }

            if (_totallyCured)
            {
                if (CursesStrength.ContainsKey(curse))
                {
                    CursesStrength[curse].GO.SetActive(false);
                    CursesStrength.Remove(curse);
                }

                continue;
            }

            CursesStrength[curse].GO.GetComponentInChildren<Image>().sprite = Utils.GetCurseSprite(curse);
            CursesStrength[curse].GO.GetComponentInChildren<TMP_Text>().text = CursesStrength[curse].Strength.ToString();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.TryGetComponent(out Potion _potion))
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
