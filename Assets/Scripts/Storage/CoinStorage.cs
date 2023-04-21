using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CoinStorage : MonoBehaviour
{

    private int goldCount, goldEnablingFrom, goldLimit;

    public GameObject[] Coins;
    private List<bool> activeCoins;

    public TMP_Text CoinText;

    // Start is called before the first frame update
    void Start()
    {
        goldLimit = Coins.Length - 1;
        Update_coins();
    }

    // Update is called once per frame
    public void Update_coins()
    {
        goldCount = GameManager.Instance.Gold;
        CoinText.text = goldCount.ToString() + " :Gold Coins";

        if (goldCount == 0)
        {
            ResetGold();
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

    void ResetGold()
    {
        foreach (var Coins in Coins)
        {
            Coins.gameObject.SetActive(false);
        }
    }

    void SpawnAllGold()
    {
        foreach (var Coins in Coins)
        {
            Coins.gameObject.SetActive(true);
        }
    }

    void SpawnGold()
    {
        ResetGold();

        for (int a = 0; a < goldCount; a++)
        {
            Coins[a].SetActive(true);
        }
    }

}