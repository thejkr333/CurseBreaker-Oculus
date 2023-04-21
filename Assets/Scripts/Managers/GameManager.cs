using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public int Gold, Rent, RentIncrement, PaymentIncrement;

    private CoinStorage coinChest;

    [SerializeField] Transform parla;

    [Header("CUSTOMERS")]
    [SerializeField] GameObject customerPrefab;
    const int NUMBEROFCUSTOMERSPERDAY = 3;
    [SerializeField] GameObject[] customersToday = new GameObject[NUMBEROFCUSTOMERSPERDAY];

    int dayCount;

    public event Action OnNewDay;
    public event Action<Ingredients[]> CreateShop;

    //True == unlocked ---- false == locked
    Dictionary<Curses, bool> cursesLockInfo = new Dictionary<Curses, bool>();

    private void Awake()
    {
        coinChest = GameObject.FindObjectOfType<CoinStorage>();

        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }

<<<<<<< Updated upstream
        //Populate the dictionary putting all the curses as locked
        foreach (Curses curse in Enum.GetValues(typeof(Curses)))
        {
            cursesLockInfo.Add(curse, false);
        }

        //Starting unlocked curses
        cursesLockInfo[Curses.Wolfus] = true;
        cursesLockInfo[Curses.Gassle] = true;

        LoadGame();

        this.OnNewDay += NewDay;
=======
        //this.OnNewDay += NewDay;
>>>>>>> Stashed changes
        OnNewDay?.Invoke();
    }

    public void RentIncrease()
    {
        Rent += RentIncrement;
        coinChest.Update_coins();
    }
   public void GoldGain()
    {
        Gold += 10 + PaymentIncrement;
        coinChest.Update_coins();
    }
    public void GoldSubtract(int cost)
    {
        Gold -= cost;
        coinChest.Update_coins();
    }
    public void SellIngredient(int cost)
    {
        Gold += cost;
        coinChest.Update_coins();
    }

    public void RentisDue()
    {
        if (Gold > Rent)
        {
            Gold -= Rent;
        }
    }

    public void DestroyGrabbedThings(GameObject obj)
    {
        obj.transform.position = parla.position;
        Destroy(obj, 1f);
    }

    /*
    void NewDay()
    {
        dayCount++;
        Ingredients[] _ingredients = CreateCustomers();
        CreateShop?.Invoke(_ingredients);
        SaveGame();
    }

    */
    private Ingredients[] CreateCustomers()
    {
        Ingredients[] _ingredientsMoreUsed = new Ingredients[Shop.NUMBEROFITEMS];
        Dictionary<Ingredients, int> _ingredients = new();

        for (int i = 0; i < NUMBEROFCUSTOMERSPERDAY; i++)
        {
            customersToday[i] = Instantiate(customerPrefab);
            customersToday[i].transform.position = parla.position;
            Customer _customer = customersToday[i].GetComponent<Customer>();
            foreach (var limb in _customer.AffectedLimbs)
            {
                CursexIngredientMatrix.ReturnIngredientsForCurse(limb.Curse, _customer.CurseStrength, ref _ingredients);            
            }
        }

        for (int i = 0; i < Shop.NUMBEROFITEMS; i++)
        {
            Ingredients _ing = Extensions.MaxValueKey(_ingredients);
            _ingredientsMoreUsed[i] = _ing;
            _ingredients.Remove(_ing);
        }
        return _ingredientsMoreUsed;
    }

    void SaveGame()
    {
        // Save desired stats from PlayerPrefs
        PlayerPrefs.SetInt("DayCount", dayCount);
        PlayerPrefs.SetInt("Gold", Gold);
        foreach (var curse in cursesLockInfo)
        {
            PlayerPrefs.SetInt(curse.Key.ToString(), curse.Value ? 1 : 0);
        }
        PlayerPrefs.Save();
    }

    void LoadGame()
    {
        // Load desired stats from PlayerPrefs
        dayCount = PlayerPrefs.GetInt("DayCount", 0);
        Gold = PlayerPrefs.GetInt("Gold", 0);
        foreach (var curse in cursesLockInfo)
        {
            cursesLockInfo[curse.Key] = PlayerPrefs.GetInt(curse.Key.ToString()) == 1 ? true : false;
        }
    }
}
