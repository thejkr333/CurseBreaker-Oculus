using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IngredientStorage : MonoBehaviour
{
    public bool AngelLeaf, Mandrake, CorkWood, WolfsBane, NightShade, DragonTongue;

    [SerializeField] Ingredients ingredient;
    [SerializeField] StorageController StorageController;
    public int CurrentAmount;
    // Start is called before the first frame update
    void Start()
    {
        //gameObject.GetComponent<IngredientSpawner>().GotIngredient = true;
    }

    // Update is called once per frame
    void Update()
    {
        if(CurrentAmount > 0)
        {
            CurrentAmount--;
            //GoldManager.Instance.SellIngredient(4);
            //gameObject.GetComponent<IngredientSpawner>().GotIngredient = false;
        }
        if(CurrentAmount < 0) CurrentAmount = 0;

        CurrentAmount = StorageController.storageInfos[ingredient].amount;

        if(Mandrake == true)
        {
            CurrentAmount = StorageController.MandrakeAmount;
        }
        if (AngelLeaf == true)
        {
            CurrentAmount = StorageController.AngelLeafAmount;
        }
        if (CorkWood == true)
        {
            CurrentAmount = StorageController.CorkWoodAmount;
        }
        if (WolfsBane == true)
        {
            CurrentAmount = StorageController.WolfsBaneAmount;
        }
        if(DragonTongue == true)
        {
            CurrentAmount = StorageController.DragonTongueAmount;
        }
        if(NightShade == true)
        {
            CurrentAmount = StorageController.NightShadeAmount;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.GetComponent<Mandrake>()&&Mandrake==true)
        {
            GoldManager.Instance.SubstractGold(4);
        }
        if (other.gameObject.GetComponent<CorkWood>()&&CorkWood == true)
        {
            GoldManager.Instance.SubstractGold(4);
        }
        if (other.gameObject.GetComponent<WolfsBane>() && WolfsBane == true)
        {
            GoldManager.Instance.SubstractGold(4);
        }
        if (other.gameObject.GetComponent<AngelLeaf>() && AngelLeaf == true)
        {
            GoldManager.Instance.SubstractGold(4);
        }   
        if(other.gameObject.GetComponent<DragonsTongue>() && DragonTongue == true)
        {
            GoldManager.Instance.SubstractGold(4);
        }
        if(other.gameObject.GetComponent<Nightshade>() && NightShade == true)
        {
            GoldManager.Instance.SubstractGold(4);
        }
    }
}
