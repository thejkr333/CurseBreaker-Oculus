using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public bool InMenu;

    public Transform Parla;

    public int DayCount;

    public event Action OnNewDay;
    public event Action<Ingredients[]> CreateShop;

    public bool VRTracking;


    //True == unlocked ---- false == locked
    Dictionary<Curses, bool> cursesLockInfo = new();
    Dictionary<Ingredients, bool> ingredientsLockInfo = new();

    //Just list for debug and visualize changes in unlockedInfo
    [SerializeField] List<CursexBool> _cursesLockInfo;
    [SerializeField] List<IngredientxBool> _ingredientsLockInfo;

    private void Awake()
    {
        
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }

        InitializeLockInfo();
        this.OnNewDay += NewDay;

        foreach (var curses in cursesLockInfo)
        {
            CursexBool c = new();
            c.curse = curses.Key;
            c.isLocked = curses.Value;
            _cursesLockInfo.Add(c);
        }

        foreach (var ingredients in ingredientsLockInfo)
        {
            IngredientxBool i = new();
            i.curse = ingredients.Key;
            i.isLocked = ingredients.Value;
            _ingredientsLockInfo.Add(i);
        }
    }

    private void Start()
    {
        //if there is saved data load game
        if (PlayerPrefs.HasKey("DayCount"))
        {
            LoadGame();
            DayCount--;
        }

        NextDay();

        OVRManager.TrackingLost += TrackingFalse;
        OVRManager.TrackingAcquired += TrackingTrue;
    }

    private void TrackingTrue()
    {
        VRTracking = true;
    }

    private void TrackingFalse()
    {
        VRTracking = false;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.O)) PlayerPrefs.DeleteAll();

        if (Input.GetKeyDown(KeyCode.P)) SaveGame();
    }

    void InitializeLockInfo()
    {
        //Populate the dictionary putting all the curses as locked
        foreach (Curses curse in Enum.GetValues(typeof(Curses)))
        {
            cursesLockInfo.Add(curse, false);
        }

        //Populate the dictionary putting all the ingredients as locked
        foreach (Ingredients ingredients in Enum.GetValues(typeof(Ingredients)))
        {
            ingredientsLockInfo.Add(ingredients, false);
        }

        //Starting unlocked curses
        cursesLockInfo[Curses.Wolfus] = true;
        cursesLockInfo[Curses.Gassle] = true;

        //Starting unlocked ingredients
        ingredientsLockInfo[Ingredients.WolfsBane] = true;
        ingredientsLockInfo[Ingredients.DragonsTongue] = true;
        ingredientsLockInfo[Ingredients.Mandrake] = true;
        ingredientsLockInfo[Ingredients.CorkWood] = true;
    }

    public List<Curses> GetUnlockedCurses()
    {
        List<Curses> unlockedCurses = new();
        foreach (var curse in cursesLockInfo.Keys)
        {
            if (cursesLockInfo[curse]) unlockedCurses.Add(curse);
        }

        return unlockedCurses;
    }

    public List<Ingredients> GetUnlockedIngredients()
    {
        List<Ingredients> unlockedIngredients = new();
        foreach (var ingredient in ingredientsLockInfo.Keys)
        {
            if (ingredientsLockInfo[ingredient]) unlockedIngredients.Add(ingredient);
        }

        return unlockedIngredients;
    }

    public void DestroyGrabbedThings(GameObject obj)
    {
        obj.transform.position = Parla.position;
        Destroy(obj, 1f);
    }

    public void NextDay()
    {
        OnNewDay?.Invoke();
    }

    void NewDay()
    {
        DayCount++;
        SaveGame();
    }

    //private Ingredients[] CreateCustomers()
    //{
    //    Ingredients[] _ingredientsMoreUsed = new Ingredients[Shop.NUMBEROFITEMS];
    //    Dictionary<Ingredients, int> _ingredients = new();

    //    for (int i = 0; i < NUMBEROFCUSTOMERSPERDAY; i++)
    //    {
    //        customersToday[i] = Instantiate(customerPrefab);
    //        customersToday[i].transform.position = Parla.position;
    //        Customer _customer = customersToday[i].GetComponent<Customer>();
    //        foreach (var limb in _customer.AffectedLimbs)
    //        {
    //            CursexIngredientMatrix.ReturnIngredientsForCurse(limb.Curse, _customer.CursesStrength[limb.Curse], ref _ingredients);            
    //        }
    //    }

    //    for (int i = 0; i < Shop.NUMBEROFITEMS; i++)
    //    {
    //        Ingredients _ing = Extensions.MaxValueKey(_ingredients);
    //        _ingredientsMoreUsed[i] = _ing;
    //        _ingredients.Remove(_ing);
    //    }
    //    return _ingredientsMoreUsed;
    //}

    #region Saving&Loading

    void SaveGame()
    {
        Debug.Log("Save");

        // Save desired stats from PlayerPrefs
        PlayerPrefs.SetInt("DayCount", DayCount);
        PlayerPrefs.SetInt("Gold", GoldManager.Instance.Gold);

        //Return changes from inspector to test
        foreach (var item in _cursesLockInfo)
        {
            cursesLockInfo[item.curse] = item.isLocked;
        }
        foreach (var item in _ingredientsLockInfo)
        {
            ingredientsLockInfo[item.curse] = item.isLocked;
        }

        foreach (var curse in cursesLockInfo.Keys)
        {
            PlayerPrefs.SetInt(curse.ToString(), cursesLockInfo[curse] ? 1 : 0);
        }
        foreach (var ingredient in ingredientsLockInfo.Keys)
        {
            PlayerPrefs.SetInt(ingredient.ToString(), ingredientsLockInfo[ingredient] ? 1 : 0);
        }
        PlayerPrefs.Save();
    }

    void LoadGame()
    {
        // Load desired stats from PlayerPrefs
        DayCount = PlayerPrefs.GetInt("DayCount", 0);
        GoldManager.Instance.Gold = PlayerPrefs.GetInt("Gold", 0);

        foreach (var curse in cursesLockInfo.Keys.ToList())
        {
            cursesLockInfo[curse] = PlayerPrefs.GetInt(curse.ToString()) == 1 ? true : false;
        }
        foreach (var ingredient in ingredientsLockInfo.Keys.ToList())
        {
            ingredientsLockInfo[ingredient] = PlayerPrefs.GetInt(ingredient.ToString()) == 1 ? true : false;
        }
    }
    #endregion

    #region SceneManagement
    public string CurrentScene()
    {
        return SceneManager.GetActiveScene().ToString();
    }

    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void LoadScene(int sceneBuildNumber)
    {
        SceneManager.LoadScene(sceneBuildNumber);
    }
    #endregion
}

//Debug classes
[Serializable]
public class CursexBool
{
    public Curses curse;
    public bool isLocked;
}

[Serializable]
public class IngredientxBool
{
    public Ingredients curse;
    public bool isLocked;
}
