using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IngredientStorage : MonoBehaviour
{

    public bool AngelLeaf, Mandrake, CorkWood, WolfsBane;
    public StorageController StorageController;
    public int CurrentAmount;
    // Start is called before the first frame update
    void Start()
    {
        gameObject.GetComponent<IngredientSpawner>().GotIngredient = true;
    }

    // Update is called once per frame
    void Update()
    {
        if(CurrentAmount <= 0)
        {
            CurrentAmount = 0;
            gameObject.GetComponent<IngredientSpawner>().GotIngredient = false;
        }


        if(Mandrake== true)
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
        
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.GetComponent<Mandrake>()&&Mandrake==true)
        {
            StorageController.MandrakeAmount--;
        }
        if (other.gameObject.GetComponent<CorkWood>()&&CorkWood == true)
        {
            StorageController.CorkWoodAmount--;
        }
        if (other.gameObject.GetComponent<WolfsBane>() && WolfsBane == true)
        {
            StorageController.WolfsBaneAmount--;
        }
        if (other.gameObject.GetComponent<AngelLeaf>() && AngelLeaf == true)
        {
            StorageController.AngelLeafAmount--;
        }
    }
}
