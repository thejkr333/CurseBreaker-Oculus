using System;
using System.Transactions;
using UnityEngine;


public enum TransactionType { Ingredient, Scroll, Rent };
public class Balance
{
    public int initialGold;
    public int ingredientGoldSpent;
    public int scrollGoldSpent;
    public int rentGoldSpent;
    public int goldEarned;
}
public class GoldManager : MonoBehaviour
{
    public Action<Balance, Balance> UpdateUI;

    public Balance dayBalance, cycleBalance;
    public static GoldManager Instance;

    public int Gold;

    [SerializeField] int paymentIncrement, rent, rentIncrement;

    public event Action Transaction;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(this);
            dayBalance = new();
            cycleBalance = new();
        }
        ResetCyclebalances();
    }

    public void GainGold()
    {
        Gold += 20 + paymentIncrement;
        dayBalance.goldEarned += 20 + paymentIncrement;
        cycleBalance.goldEarned += 20 + paymentIncrement;
        Transaction?.Invoke();
        UpdateUI?.Invoke(dayBalance, cycleBalance);
    }
    public void SubstractGold(int cost, TransactionType type)
    {
        Gold -= cost;
        switch (type)
        {
            case TransactionType.Ingredient:
                dayBalance.ingredientGoldSpent += cost;
                cycleBalance.ingredientGoldSpent += cost;
                break;
            case TransactionType.Rent:
                dayBalance.rentGoldSpent += cost;
                cycleBalance.rentGoldSpent += cost;
                break;
            case TransactionType.Scroll:
                dayBalance.scrollGoldSpent += cost;
                cycleBalance.scrollGoldSpent += cost;
                break;
        }

        Transaction?.Invoke();
        UpdateUI?.Invoke(dayBalance, cycleBalance);
    }
    public void SellIngredient(int cost, Vector3 originPos)
    {
        Gold += cost;
        dayBalance.ingredientGoldSpent -= cost;
        cycleBalance.ingredientGoldSpent -= cost;

        Transaction?.Invoke();

        AudioManager.Instance.PlaySoundStatic("sell", originPos);
        UpdateUI?.Invoke(dayBalance, cycleBalance);
    }

    public void ResetDayBalances()
    {
        dayBalance.initialGold = Gold;
        dayBalance.goldEarned = 0;
        dayBalance.ingredientGoldSpent = 0;
        dayBalance.rentGoldSpent = 0;
        dayBalance.scrollGoldSpent = 0;

        UpdateUI?.Invoke(dayBalance, cycleBalance);
    }

    //This method also reseets day balances;
    public void ResetCyclebalances()
    {
        ResetDayBalances();

        cycleBalance.initialGold = Gold;
        cycleBalance.goldEarned = 0;
        cycleBalance.ingredientGoldSpent = 0;
        cycleBalance.rentGoldSpent = 0;
        cycleBalance.scrollGoldSpent = 0;

    }
    public void IncreaseRent()
    {
        rent += rentIncrement;
        Transaction?.Invoke();
    }
    public void SubstractRent()
    {
        if (Gold > rent)
        {
            Gold -= rent;
        }

        Transaction?.Invoke();
        UpdateUI?.Invoke(dayBalance, cycleBalance);
    }
}