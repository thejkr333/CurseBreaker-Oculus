using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IngredientSpawner : MonoBehaviour
{
    [SerializeField] GameObject ingredientPrefab;
    [SerializeField] GameObject inside;
    public bool GotMoney;
    Vector3 scaleObj;
    private void Awake()
    {
        inside = this.gameObject;

        scaleObj = inside.transform.localScale;

        GameObject clon = Instantiate(ingredientPrefab);
        clon.transform.position = transform.position;
        clon.transform.localScale = scaleObj;
        inside = clon;
    }

    // Update is called once per frame
    void Update()
    {GameObject.Find("Coins").GetComponent<CoinStorage>().NoMoreMoney = !GotMoney;
        //Check if the object iniside is STILL inside, if not create another one
        if(Vector3.Distance(inside.transform.position, transform.position) > 1&& GotMoney==true)
        {
            GameObject clon = Instantiate(ingredientPrefab);
            clon.transform.position = transform.position;
            clon.transform.localScale = scaleObj;
            inside = clon;

            if (!(GetComponent<IngredientChest>())) return;
                GetComponent<IngredientChest>().SubtractMoney();
        }
    }
}
