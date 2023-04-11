using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CoinStorage : MonoBehaviour
{


    private int goldCount, goldEnablingFrom;
    private bool doOnce, morethanSix;
    public GameObject[] Coins;
    public TMP_Text CoinText;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        goldCount = GameManager.Instance.Gold;
        CoinText.text = goldCount.ToString() + " :Gold Coins";
        if(goldCount == 0)
        {
            foreach (var Coins in Coins)
            {
                Coins.gameObject.SetActive(false);
            }
            goldEnablingFrom = -1;
        }
        if(goldCount > 0&&doOnce == false)
        {
            goldEnablingFrom = goldCount-1;
            doOnce = true;
        }
        
        if(goldCount>6 &&doOnce == true)
        {
            foreach (var Coins in Coins)
            {
                Coins.gameObject.SetActive(true);
            }
            morethanSix = true;
        }
        if(goldEnablingFrom>=0 &&doOnce == true&&morethanSix== false)
        {
            Coins[goldEnablingFrom].SetActive(true);
            goldEnablingFrom--;
        }
    }
}
