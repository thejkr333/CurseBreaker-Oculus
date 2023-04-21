using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CoinStorage : MonoBehaviour
{
    private int goldCount, goldLimit;

    //to fix "free" ingredients
    public bool NoMoreMoney;
    [SerializeField] GameObject[] coins;

    public TMP_Text CoinText;

    // Start is called before the first frame update
    void Start()
    {
        goldLimit = coins.Length - 1;
        UpdateCoins();

        GoldManager.Instance.Transaction += UpdateCoins;
    }

    // Update is called once per frame
    public void UpdateCoins()
    {
        goldCount = GoldManager.Instance.Gold;
        CoinText.text = goldCount.ToString() + " :Gold Coins";
        if (goldCount < 0)
        {
            NoMoreMoney = true;
        }
        if(goldCount > 0)
        {
            NoMoreMoney = false;
        }
        if (goldCount == 0)
        {
            DeactivateCoins();
        }

        if (goldCount < goldLimit)
        {
            SpawnGold();
        }

        if (goldCount > 6)
        {
            SpawnAllGold();
        }

    }

    void DeactivateCoins()
    {
        foreach (var Coins in coins)
        {
            Coins.gameObject.SetActive(false);
        }
    }

    void SpawnAllGold()
    {
        foreach (var Coins in coins)
        {
            Coins.gameObject.SetActive(true);
        }
    }

    void SpawnGold()
    {
        DeactivateCoins();

        for (int i = 0; i < goldCount; i++)
        {
            coins[i].SetActive(true);
        }
    }
}