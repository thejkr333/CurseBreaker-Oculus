using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using TMPro;
using UnityEngine.UI;



public class CurseUI
{
    public GameObject GO;
    public int Strength;
}

public class Customer : MonoBehaviour
{
    [SerializeField] Material hiddenMat;

    

    public Dictionary<LimbsList, Elements> LimbToElementMapping = new();
    public Dictionary<Elements, LimbsList> ElementToLimbMapping = new();

    int numberOfPartsAffected;

    //public Dictionary<Curses, int> CursesStrength = new();

    bool cured;
    public int Chances = 3;

    public List<AffectedLimb> AffectedLimbs = new();

    Material defaultMaterial;

    [SerializeField] GameObject curseUIprefab;


    GameObject canvas;
    public Dictionary<Curses, CurseUI> CursesStrength = new();

    private void Awake()
    {
        defaultMaterial = GetComponentInChildren<MeshRenderer>().material;
        canvas = GetComponentInChildren<Canvas>().gameObject;

        LimbElementMapping();

        InitiateAffectedLimbParts();

        SetUpCurseUI();

    }

    private void Update()
    {
        if (Chances < 0) return;

        //Check if player failed to cure the customer - James
        if (Chances == 0)
        {
            gameObject.GetComponent<BasicChat>().ChatTime = 5;
            gameObject.GetComponent<BasicChat>().DespawnOnceDone = true;

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

            GiveCurseToLimb((LimbsList)_affectedLimbs[i]);
        }

        SetUpCurse();
    }

    void GiveCurseToLimb(LimbsList limbName, Curses curse = (Curses)(-1))
    {
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

                //Create random curse from unlocked curses list
                if ((int)curse == -1) curse = CreateRandomUnlockedCurse();

                //Add curse to the dictionary if it is new and give it a strength
                CurseUI _curseUI = new();
                if (!CursesStrength.ContainsKey(curse))
                {

                    _curseUI.Strength = UnityEngine.Random.Range(3, 9);
                    _curseUI.GO = Instantiate(curseUIprefab, canvas.transform);
                    CursesStrength.Add(curse, _curseUI);
                }
                else CursesStrength[curse].Strength += UnityEngine.Random.Range(1, 4);


                //Asssign the limb its correspondant values
                AffectedLimb _affectedLimb = new AffectedLimb(transform.GetChild(j).gameObject, curse, limbName, LimbToElementMapping[limbName]);
                AffectedLimbs.Add(_affectedLimb);

                //Add the correspondant curse to the limb gamobject
                _affectedLimb.AffectedLimbGO.AddComponent(Type.GetType(curse.ToString()));

                //Change visuals of the affected limb
                Curse _curse = _affectedLimb.AffectedLimbGO.GetComponent<Curse>();
                _curse.CursedMaterial = Utils.GetCurseMaterial(curse);
                _curse.ChangeVisuals(_affectedLimb.AffectedLimbGO);
                break;
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

    void SetUpCurse()
    {
        foreach (var limb in AffectedLimbs)
        {

            limb.AffectedLimbGO.GetComponentInChildren<SpriteRenderer>().sprite = Utils.GetElementSprite(limb.Element);

            //switch (limb.Element)
            //{
            //    case Elements.Fire:
            //        limb.AffectedLimbGO.GetComponentInChildren<SpriteRenderer>().sprite = Fire;
            //        break;
            //    case Elements.Water:
            //        limb.AffectedLimbGO.GetComponentInChildren<SpriteRenderer>().sprite = Water;
            //        break;
            //    case Elements.Dark:
            //        limb.AffectedLimbGO.GetComponentInChildren<SpriteRenderer>().sprite = Dark;
            //        break;
            //    case Elements.Light:
            //        limb.AffectedLimbGO.GetComponentInChildren<SpriteRenderer>().sprite = Light;
            //        break;
            //    case Elements.Air:
            //        limb.AffectedLimbGO.GetComponentInChildren<SpriteRenderer>().sprite = Air;
            //        break;
            //    case Elements.Earth:
            //        limb.AffectedLimbGO.GetComponentInChildren<SpriteRenderer>().sprite = Earth;
            //        break;
            //}
        }
    }
    void Cured()
    {
        gameObject.GetComponent<BasicChat>().ChatTime = 5;
        gameObject.GetComponent<BasicChat>().DespawnOnceDone = true;
        gameObject.GetComponent<BasicChat>().Cured = true;
        cured = true;

        DayManager.Instance.NextCustomer();
        GoldManager.Instance.GainGold();
    }

    void GetPotion(Potion potion)
    {
        foreach (Elements element in potion.PotionElements)
        {
            //Check which limb is affected by the elements
            foreach (var limb in AffectedLimbs)
            {
                if (ElementToLimbMapping[element] == limb.LimbName)
                {
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
                                limb.AffectedLimbGO.GetComponentInChildren<MeshRenderer>().material = defaultMaterial;
                            }
                        }
                    }
                    else
                    {
                        //Means the targeted limb doesn't have a curse on it. Affect negatively
                        //Give the limb a random curse from the main curses of the ingredients
                        Curses _curseToGive = CursexIngredientMatrix.GetRandomCurse(potion.PotionIngredients);
                        GiveCurseToLimb(ElementToLimbMapping[element], _curseToGive);
                        SetUpCurseUI();
                        Chances--;
                    }
                    break;
                }
            }
        }
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
        foreach(var curse in CursesStrength.Keys)
        {
            CursesStrength[curse].GO.GetComponentInChildren<Image>().sprite = Utils.GetCurseSprite(curse);
            CursesStrength[curse].GO.GetComponentInChildren<TMP_Text>().text = CursesStrength[curse].Strength.ToString();

            //switch (curse)
            //{
            //    case Curses.Wolfus:
            //        CursesStrength[curse].GO.GetComponentInChildren<Image>().sprite = WolfusImage;
            //            break;
            //    case Curses.Gassle:
            //        CursesStrength[curse].GO.GetComponentInChildren<Image>().sprite = GassleImage;
            //        break;
            //    case Curses.Demonitis:
            //        CursesStrength[curse].GO.GetComponentInChildren<Image>().sprite = DemonitisImage;
            //        break;
            //    case Curses.Petrification:
            //        CursesStrength[curse].GO.GetComponentInChildren<Image>().sprite = PetrificationImage;
            //        break;
            //    case Curses.Porko:
            //        CursesStrength[curse].GO.GetComponentInChildren<Image>().sprite = PorkoImage;
            //        break;
            //    case Curses.Runeblight:
            //        CursesStrength[curse].GO.GetComponentInChildren<Image>().sprite = RuneBlightImage;
            //        break;
            //}

            

        }
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
