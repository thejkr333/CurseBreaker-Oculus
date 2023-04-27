using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BalancesUIHandler : MonoBehaviour
{
    // Start is called before the first frame update
    [Header("Day texts")]
    [SerializeField] TMP_Text ingredientCostD;
    [SerializeField] TMP_Text scrollCostD;
    [SerializeField] TMP_Text rentCostD;
    [SerializeField] TMP_Text goldEarnedD;
    [SerializeField] TMP_Text profitD;

    [Header("Cycle texts")]
    [SerializeField] TMP_Text ingredientCostC;
    [SerializeField] TMP_Text scrollCostC;
    [SerializeField] TMP_Text rentCostC;
    [SerializeField] TMP_Text goldEarnedC;
    [SerializeField] TMP_Text profitC;
    void Start()
    {

    }


    public void UpdateUI(Balance dayBal, Balance cycleBal)
    {
        UpdateDayUI(dayBal);
        UpdateCycleUI(cycleBal);
    }
    public void UpdateDayUI(Balance bal)
    {
        if(bal.ingredientGoldSpent > 0)
        {
            ingredientCostD.color = Color.red;
        }
        else
        {
            ingredientCostD.color = Color.white;
        }
        ingredientCostD.text = "-" + bal.ingredientGoldSpent.ToString();


        if (bal.scrollGoldSpent > 0)
        {
            scrollCostD.color = Color.red;
        }
        else
        {
            scrollCostD.color = Color.white;
        }
        scrollCostD.text = "-" + bal.scrollGoldSpent.ToString();


        if (bal.rentGoldSpent > 0)
        {
            rentCostD.color = Color.red;
        }
        else
        {
            rentCostD.color = Color.white;
        }
        rentCostD.text = bal.rentGoldSpent.ToString();


        if (bal.goldEarned > 0)
        {
            goldEarnedD.color = Color.green;
        }
        else
        {
            goldEarnedD.color = Color.white;
        }
        goldEarnedD.text = "+" + bal.goldEarned.ToString();


        int totalProfit = bal.goldEarned - bal.rentGoldSpent - bal.scrollGoldSpent - bal.ingredientGoldSpent;
        if(totalProfit > 0)
        {
            profitD.color = Color.green;
            profitD.text = "+";
        }
        else if(totalProfit < 0){
            profitD.color = Color.red;

        }
        else
        {
            profitD.color = Color.white;
            profitD.text = "";
        }
        profitD.text = profitD.text + totalProfit.ToString();
    }



    public void UpdateCycleUI(Balance bal)
    {
        if (bal.ingredientGoldSpent > 0)
        {
            ingredientCostC.color = Color.red;
        }
        else
        {
            ingredientCostC.color = Color.white;
        }
        ingredientCostC.text = "-" + bal.ingredientGoldSpent.ToString();


        if (bal.scrollGoldSpent > 0)
        {
            scrollCostC.color = Color.red;
        }
        else
        {
            scrollCostC.color = Color.white;
        }
        scrollCostC.text = "-" + bal.scrollGoldSpent.ToString();


        if (bal.rentGoldSpent > 0)
        {
            rentCostC.color = Color.red;
        }
        else
        {
            rentCostC.color = Color.white;
        }
        rentCostC.text = "-" + bal.rentGoldSpent.ToString();


        if (bal.goldEarned > 0)
        {
            goldEarnedC.color = Color.green;
        }
        else
        {
            goldEarnedC.color = Color.white;
        }
        goldEarnedC.text = "+" + bal.goldEarned.ToString();


        int totalProfit = bal.goldEarned - bal.rentGoldSpent - bal.scrollGoldSpent - bal.ingredientGoldSpent;
        if (totalProfit > 0)
        {
            profitC.color = Color.green;
            profitC.text = "+";
        }
        else if (totalProfit < 0)
        {
            profitC.color = Color.red;
        }
        else
        {
            profitC.color = Color.white;
            profitC.text = "";
        }

        profitC.text = profitC.text + totalProfit.ToString();
    }

}