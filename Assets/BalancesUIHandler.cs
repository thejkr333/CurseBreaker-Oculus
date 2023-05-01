using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BalancesUIHandler : MonoBehaviour
{
    // Start is called before the first frame update
    [Header("DAY TEXTS")]
    [SerializeField] TMP_Text dayIngredientsSpent;
    [SerializeField] TMP_Text cycleScrollSpent;
    [SerializeField] TMP_Text dayRentCost;
    [SerializeField] TMP_Text dayGoldEarned;
    [SerializeField] TMP_Text dayProfit;

    [Header("CYCLE TEXTS")]
    [SerializeField] TMP_Text cycleIngredientSpent;
    [SerializeField] TMP_Text dayScrollSpent;
    [SerializeField] TMP_Text cycleRentCost;
    [SerializeField] TMP_Text cycleGoldEarned;
    [SerializeField] TMP_Text cycleProfit;

    private void Start()
    {
        GoldManager.Instance.UpdateUI += UpdateUI;
        UpdateUI(GoldManager.Instance.dayBalance, GoldManager.Instance.cycleBalance);
    }
    public void UpdateUI(Balance dayBalance, Balance cycleBalance)
    {
        UpdateDayUI(dayBalance);
        UpdateCycleUI(cycleBalance);
    }
    public void UpdateDayUI(Balance balance)
    {
        if(balance.ingredientGoldSpent > 0)
        {
            dayIngredientsSpent.color = Color.red;
        }
        else
        {
            dayIngredientsSpent.color = Color.white;
        }
        dayIngredientsSpent.text = "-" + balance.ingredientGoldSpent.ToString();


        if (balance.scrollGoldSpent > 0)
        {
            dayScrollSpent.color = Color.red;
        }
        else
        {
            dayScrollSpent.color = Color.white;
        }
        dayScrollSpent.text = "-" + balance.scrollGoldSpent.ToString();


        if (balance.rentGoldSpent > 0)
        {
            dayRentCost.color = Color.red;
        }
        else
        {
            dayRentCost.color = Color.white;
        }
        dayRentCost.text = balance.rentGoldSpent.ToString();


        if (balance.goldEarned > 0)
        {
            dayGoldEarned.color = Color.green;
        }
        else
        {
            dayGoldEarned.color = Color.white;
        }
        dayGoldEarned.text = "+" + balance.goldEarned.ToString();


        int totalProfit = balance.goldEarned - balance.rentGoldSpent - balance.scrollGoldSpent - balance.ingredientGoldSpent;
        dayProfit.text = "";

        if (totalProfit > 0)
        {
            dayProfit.color = Color.green;
            dayProfit.text = "+";
        }
        else if(totalProfit < 0){
            dayProfit.color = Color.red;

        }
        else
        {
            dayProfit.color = Color.white;
        }
        dayProfit.text += totalProfit.ToString();
    }



    public void UpdateCycleUI(Balance balance)
    {
        if (balance.ingredientGoldSpent > 0)
        {
            cycleIngredientSpent.color = Color.red;
        }
        else
        {
            cycleIngredientSpent.color = Color.white;
        }
        cycleIngredientSpent.text = "-" + balance.ingredientGoldSpent.ToString();


        if (balance.scrollGoldSpent > 0)
        {
            cycleScrollSpent.color = Color.red;
        }
        else
        {
            cycleScrollSpent.color = Color.white;
        }
        cycleScrollSpent.text = "-" + balance.scrollGoldSpent.ToString();


        if (balance.rentGoldSpent > 0)
        {
            cycleRentCost.color = Color.red;
        }
        else
        {
            cycleRentCost.color = Color.white;
        }
        cycleRentCost.text = "-" + balance.rentGoldSpent.ToString();


        if (balance.goldEarned > 0)
        {
            cycleGoldEarned.color = Color.green;
        }
        else
        {
            cycleGoldEarned.color = Color.white;
        }
        cycleGoldEarned.text = "+" + balance.goldEarned.ToString();


        int totalProfit = balance.goldEarned - balance.rentGoldSpent - balance.scrollGoldSpent - balance.ingredientGoldSpent;
        cycleProfit.text = "";

        if (totalProfit > 0)
        {
            cycleProfit.color = Color.green;
            cycleProfit.text = "+";
        }
        else if (totalProfit < 0)
        {
            cycleProfit.color = Color.red;
        }
        else
        {
            cycleProfit.color = Color.white;
        }

        cycleProfit.text += totalProfit.ToString();
    }

}