using System;
using System.Collections.Generic;
using UnityEngine;

public class StorageController : MonoBehaviour
{
    public Dictionary<Ingredients, StorageInfo> storageInfos = new();

    public int MandrakeAmount, AngelLeafAmount, WolfsBaneAmount, CorkWoodAmount, NightShadeAmount, DragonTongueAmount;
    [SerializeField] Transform[] storagesPosition;

    private void Awake()
    {
        int count = 0;
        foreach (Ingredients ingredient in Enum.GetValues(typeof(Ingredients)))
        {
            StorageInfo storageInfo = new StorageInfo(0, storagesPosition[count++]);
            storageInfos.Add(ingredient, storageInfo);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        //if (other.gameObject.GetComponent<Mandrake>())
        //{
        //    MandrakeAmount++;
        //    other.transform.position = MandrakeStorage.position;
        //}
        //if (other.gameObject.GetComponent<AngelLeaf>())
        //{
        //    AngelLeafAmount++;
        //    other.transform.position = AngelLeafStorage.position;
        //}
        //if (other.gameObject.GetComponent<CorkWood>())
        //{
        //    CorkWoodAmount++;
        //    other.transform.position = CorkWoodStorage.position;
        //}
        //if (other.gameObject.GetComponent<WolfsBane>())
        //{
        //    WolfsBaneAmount++;
        //    other.transform.position = WolfsBaneStorage.position;
        //}
        //if (other.gameObject.GetComponent<Nightshade>())
        //{
        //    NightShadeAmount++;
        //    other.transform.position = NightShadeStorage.position;
        //}
        //if (other.gameObject.GetComponent<DragonsTongue>())
        //{
        //    DragonTongueAmount++;
        //    other.transform.position = DragonTongueStorage.position;
        //}
    }
}

public class StorageInfo
{
    public int amount;
    public Transform position;

    public StorageInfo(int _amount, Transform _position)
    {
        amount = _amount;
        position = _position;
    }
}
