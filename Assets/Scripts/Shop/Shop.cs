using System;
using System.Collections.Generic;
using UnityEngine;

public class Shop : MonoBehaviour
{
    public const int NUMBEROFITEMS = 3;

    GameObject[] itemsInShop = new GameObject[NUMBEROFITEMS];

    [SerializeField] List<ItemValue> itemsValueList = new();
    Dictionary<GameObject, int> itemsValue = new();

    float scrollProbabilty;
    private void Awake()
    {
        foreach (ItemValue item in itemsValueList)
        {
            itemsValue.Add(item.item, item.value);
        }

        scrollProbabilty = .05f;

        GameManager.Instance.OnNewDay += Event_NewDay;
        GameManager.Instance.CreateShop += Event_CreateShop;
    }

    private void Event_NewDay()
    {
        throw new NotImplementedException();
    }

    private void Event_CreateShop(Ingredients[] ingredients)
    {
        //Check if you there will be a scroll in the shop
        float _scrollChance = UnityEngine.Random.Range(0, 1);
        bool scroll = false;
        if (scrollProbabilty <= _scrollChance)
        {
            scrollProbabilty = .05f;
            scroll = true;
        }
        else scrollProbabilty += .05f;

        for (int i = 0; i < NUMBEROFITEMS; i++)
        {
            //Create items in the shop
        }

        if (scroll)
        {
            //replace the last item in the shop with a random scroll
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

[Serializable]
public struct ItemValue
{
    public GameObject item;
    public int value;
}
